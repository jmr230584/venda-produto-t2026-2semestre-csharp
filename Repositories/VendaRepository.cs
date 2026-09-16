// Importa o namespace que contém a classe ConexaoBanco.
using LojaApi.Data;

// Importa o namespace que contém a entidade Venda.
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
    // responsável por listar as vendas ativas.
    //
    // Task indica que o método é assíncrono.
    //
    // List<Venda> indica que, quando terminar,
    // o método devolverá uma lista de vendas.
    public async Task<List<Venda>> ListarAtivasAsync()
    {
        // Cria uma lista vazia para armazenar
        // os objetos Venda encontrados no banco.
        //
        // O C# identifica que new() representa
        // a criação de uma List<Venda>.
        List<Venda> vendas = new();

        // Solicita uma conexão aberta com o PostgreSQL.
        //
        // await aguarda a abertura da conexão.
        //
        // await using garante que a conexão será fechada
        // e liberada automaticamente ao final do método.
        await using NpgsqlConnection conexao =
            await _conexaoBanco.CriarConexaoAsync();

        // Declara a variável que armazenará
        // o comando SQL.
        //
        // As três aspas permitem escrever um texto
        // utilizando várias linhas.
        string sql = """
            SELECT
                id_venda,
                data_hora,
                valor_total,
                ativa
            FROM venda
            WHERE ativa = TRUE
            ORDER BY data_hora DESC;
            """;

        // Cria o comando que será executado no PostgreSQL.
        //
        // O primeiro argumento contém o SQL.
        //
        // O segundo argumento informa a conexão
        // em que o comando será executado.
        await using NpgsqlCommand comando =
            new NpgsqlCommand(sql, conexao);

        // Executa o comando SQL.
        //
        // ExecuteReaderAsync() é utilizado quando
        // o SELECT pode devolver uma ou mais linhas.
        //
        // O resultado será armazenado em um DataReader,
        // que permite ler as linhas retornadas pelo banco.
        await using NpgsqlDataReader leitor =
            await comando.ExecuteReaderAsync();

        // Percorre as linhas retornadas pelo SELECT.
        //
        // ReadAsync() avança para a próxima linha.
        //
        // Enquanto existir uma linha para ser lida,
        // o bloco será executado.
        while (await leitor.ReadAsync())
        {
            // Cria um objeto Venda com os dados
            // da linha atual do resultado.
            Venda venda = new Venda(

                // Lê a coluna localizada na posição 0.
                //
                // Posição 0 corresponde a id_venda.
                leitor.GetInt32(0),

                // Lê a coluna localizada na posição 1.
                //
                // Posição 1 corresponde a data_hora.
                leitor.GetDateTime(1),

                // Lê a coluna localizada na posição 2.
                //
                // Posição 2 corresponde a valor_total.
                leitor.GetDecimal(2),

                // Lê a coluna localizada na posição 3.
                //
                // Posição 3 corresponde a ativa.
                leitor.GetBoolean(3)
            );

            // Adiciona o objeto Venda à lista.
            vendas.Add(venda);
        }

        // Devolve a lista com todas as vendas
        // ativas encontradas no banco.
        return vendas;
    }
}