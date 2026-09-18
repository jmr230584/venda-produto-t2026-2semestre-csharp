// Importa o namespace que contém a classe ConexaoBanco.
using LojaApi.Data;

// Importa o namespace que contém as entidades Venda e ItemVenda.
using LojaApi.Entities;

// Importa as classes da biblioteca Npgsql,
// utilizada para acessar o PostgreSQL.
using Npgsql;

// Define que a classe pertence ao namespace
// LojaApi.Repositories.
namespace LojaApi.Repositories;

// Declara o Repository responsável por executar
// comandos SQL relacionados à entidade Venda.
public class VendaRepository
{
    // Armazena uma referência para a classe de conexão.
    //
    // private impede o acesso direto por outras classes.
    //
    // readonly impede que o atributo receba outro objeto
    // depois da execução do construtor.
    private readonly ConexaoBanco _conexaoBanco;

    // Construtor da classe VendaRepository.
    //
    // O objeto ConexaoBanco será fornecido automaticamente
    // pelo sistema de injeção de dependência do ASP.NET Core.
    public VendaRepository(ConexaoBanco conexaoBanco)
    {
        // Armazena no atributo privado o objeto
        // recebido pelo parâmetro do construtor.
        _conexaoBanco = conexaoBanco;
    }

    // Declara um método público e assíncrono
    // responsável por listar as vendas ativas com seus itens.
    //
    // Task<List<Venda>> representa uma operação que,
    // ao terminar, disponibiliza uma lista de vendas.
    public async Task<List<Venda>> ListarAtivasAsync()
    {
        // Cria a lista que será devolvida ao Service.
        List<Venda> vendas = new();

        // Associa o identificador de cada venda ao objeto criado.
        //
        // O JOIN repete os dados da venda em cada linha de item.
        // O Dictionary permite localizar rapidamente a venda
        // já criada, evitando objetos duplicados no resultado.
        Dictionary<int, Venda> vendasPorId = new();

        // Solicita uma conexão aberta com o PostgreSQL.
        //
        // await aguarda a conclusão da abertura da conexão.
        //
        // await using garante o descarte da conexão
        // ao sair do escopo, inclusive se ocorrer uma exceção.
        await using NpgsqlConnection conexao =
            await _conexaoBanco.CriarConexaoAsync();

        // As três aspas permitem escrever um texto
        // utilizando várias linhas.
        //
        // LEFT JOIN mantém a venda no resultado
        // mesmo que ela ainda não possua itens.
        //
        // O valor unitário vem de item_venda para preservar
        // o preço registrado no momento da venda.
        string sql = """
            SELECT
                v.id_venda,
                v.data_hora,
                v.valor_total,
                v.ativa,
                iv.id_item_venda,
                iv.id_produto,
                p.nome AS nome_produto,
                iv.quantidade,
                iv.valor_unitario
            FROM venda AS v
            LEFT JOIN item_venda AS iv
                ON iv.id_venda = v.id_venda
            LEFT JOIN produto AS p
                ON p.id_produto = iv.id_produto
            WHERE v.ativa = TRUE
            ORDER BY
                v.data_hora DESC,
                v.id_venda DESC,
                iv.id_item_venda;
            """;

        // Cria o comando que será executado no PostgreSQL.
        //
        // O primeiro argumento contém o SQL.
        // O segundo informa a conexão utilizada.
        await using NpgsqlCommand comando =
            new NpgsqlCommand(sql, conexao);

        // Executa o SELECT e obtém um DataReader,
        // que permite ler as linhas retornadas pelo banco.
        await using NpgsqlDataReader leitor =
            await comando.ExecuteReaderAsync();

        // Obtém a posição de cada coluna pelo nome.
        //
        // Assim, a leitura dos valores não depende de números
        // escritos diretamente em cada chamada de GetInt32,
        // GetString ou nos demais métodos de leitura.
        int colunaIdVenda = leitor.GetOrdinal("id_venda");
        int colunaDataHora = leitor.GetOrdinal("data_hora");
        int colunaValorTotal = leitor.GetOrdinal("valor_total");
        int colunaAtiva = leitor.GetOrdinal("ativa");

        int colunaIdItemVenda = leitor.GetOrdinal("id_item_venda");
        int colunaIdProduto = leitor.GetOrdinal("id_produto");
        int colunaNomeProduto = leitor.GetOrdinal("nome_produto");
        int colunaQuantidade = leitor.GetOrdinal("quantidade");
        int colunaValorUnitario = leitor.GetOrdinal("valor_unitario");

        // Percorre as linhas retornadas pelo SELECT.
        //
        // ReadAsync avança para a próxima linha.
        // Enquanto existir uma linha, o bloco será executado.
        while (await leitor.ReadAsync())
        {
            // Lê o identificador da venda da linha atual.
            int idVenda = leitor.GetInt32(colunaIdVenda);

            // Procura a venda no Dictionary.
            //
            // Se encontrar, a variável venda recebe o objeto
            // que já foi criado em uma linha anterior.
            //
            // Se não encontrar, criamos o objeto uma única vez.
            if (!vendasPorId.TryGetValue(idVenda, out Venda? venda))
            {
                // Utiliza o construtor que recebe os dados
                // de uma venda já cadastrada no banco.
                venda = new Venda(
                    idVenda,
                    leitor.GetDateTime(colunaDataHora),
                    leitor.GetDecimal(colunaValorTotal),
                    leitor.GetBoolean(colunaAtiva)
                );

                // Registra o objeto para encontrá-lo
                // nas próximas linhas da mesma venda.
                vendasPorId.Add(idVenda, venda);

                // Adiciona a venda à lista de retorno
                // somente no momento em que ela é criada.
                vendas.Add(venda);
            }

            // Uma venda sem itens também aparece no LEFT JOIN.
            // Nesse caso, as colunas do item ficam nulas.
            //
            // Só criamos o ItemVenda quando há um item associado.
            if (!leitor.IsDBNull(colunaIdItemVenda))
            {
                // Cria o objeto correspondente ao item
                // presente na linha atual do resultado.
                ItemVenda item = new ItemVenda(
                    leitor.GetInt32(colunaIdItemVenda),
                    leitor.GetInt32(colunaIdProduto),
                    leitor.GetString(colunaNomeProduto),
                    leitor.GetInt32(colunaQuantidade),
                    leitor.GetDecimal(colunaValorUnitario)
                );

                // Associa o item à venda correspondente.
                //
                // A lista e o Dictionary guardam referências
                // para o mesmo objeto Venda. Portanto, o item
                // também estará presente na lista de retorno.
                venda.AdicionarItem(item);
            }
        }

        // Devolve as vendas ativas com seus respectivos itens.
        return vendas;
    }
}