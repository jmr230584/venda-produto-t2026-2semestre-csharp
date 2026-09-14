// Importa as classes da biblioteca Npgsql,
// responsável pela comunicação entre C# e PostgreSQL.
using Npgsql;

// Define que a classe pertence ao namespace LojaApi.Data.
// O namespace ajuda a organizar as classes da aplicação.
namespace LojaApi.Data;

// Declara a classe pública responsável pela conexão com o banco.
//
// IAsyncDisposable determina que a classe possui recursos
// que devem ser liberados de forma assíncrona.
public class ConexaoBanco : IAsyncDisposable
{
    // Declara um atributo privado que armazenará a fonte de dados.
    //
    // NpgsqlDataSource administra as configurações e o conjunto
    // de conexões disponíveis para acessar o PostgreSQL.
    //
    // readonly impede que o atributo receba outro valor
    // depois da execução do construtor.
    private readonly NpgsqlDataSource _fonteDados;

    // Construtor da classe ConexaoBanco.
    //
    // IConfiguration permite acessar as configurações
    // dos arquivos appsettings.json e
    // appsettings.Development.json.
    public ConexaoBanco(IConfiguration configuration)
    {
        // Declara uma variável local que receberá
        // a string de conexão com o PostgreSQL.
        string stringConexao =

            // Procura uma string de conexão chamada "Postgres"
            // dentro da seção ConnectionStrings.
            configuration.GetConnectionString("Postgres")

            // O operador ?? verifica se o valor encontrado é nulo.
            // Se for nulo, a exceção localizada à direita será lançada.
            ?? throw new InvalidOperationException(

                // Mensagem apresentada quando a string
                // de conexão não estiver configurada.
                "A conexão Postgres não foi configurada."
            );

        // Cria a fonte de dados utilizando a string de conexão.
        //
        // A fonte de dados ficará responsável por administrar
        // as conexões utilizadas pela aplicação.
        _fonteDados = NpgsqlDataSource.Create(stringConexao);
    }

    // Declara um método público e assíncrono
    // responsável por criar e abrir uma conexão.
    //
    // Task<NpgsqlConnection> informa que o método,
    // quando terminar, devolverá uma conexão aberta.
    public async Task<NpgsqlConnection> CriarConexaoAsync()
    {
        // Solicita uma conexão aberta à fonte de dados.
        //
        // await aguarda a abertura da conexão sem bloquear
        // a execução da aplicação.
        return await _fonteDados.OpenConnectionAsync();
    }

    // Declara um método público e assíncrono
    // para verificar se a conexão está funcionando.
    //
    // Task<bool> indica que o método devolverá
    // true ou false quando terminar.
    public async Task<bool> TestarAsync()
    {
        // Cria e abre uma conexão com o PostgreSQL.
        //
        // await using garante que a conexão será fechada
        // e liberada automaticamente ao final do método,
        // inclusive se ocorrer algum erro.
        await using NpgsqlConnection conexao =
            await CriarConexaoAsync();

        // Cria um comando SQL que será enviado ao PostgreSQL.
        //
        // O comando SELECT 1 não consulta nenhuma tabela.
        // Ele apenas solicita que o banco devolva o número 1.
        //
        // A variável conexao informa em qual conexão
        // o comando deverá ser executado.
        await using NpgsqlCommand comando =
            new NpgsqlCommand("SELECT 1;", conexao);

        // Executa o comando e armazena o primeiro valor retornado.
        //
        // ExecuteScalarAsync é utilizado quando esperamos
        // somente um valor como resultado.
        //
        // object? indica que o resultado pode ser
        // um objeto ou pode ser nulo.
        object? resultado =
            await comando.ExecuteScalarAsync();

        // Converte o resultado para o tipo int
        // e verifica se o valor recebido é igual a 1.
        //
        // Se o resultado for 1, retorna true,
        // indicando que a conexão funcionou.
        //
        // Caso contrário, retorna false.
        return Convert.ToInt32(resultado) == 1;
    }

    // Método responsável por liberar a fonte de dados
    // quando a classe não for mais utilizada.
    //
    // ValueTask é utilizado porque a liberação do recurso
    // pode ser concluída de forma assíncrona.
    public async ValueTask DisposeAsync()
    {
        // Libera os recursos administrados pela fonte de dados,
        // incluindo as conexões mantidas por ela.
        await _fonteDados.DisposeAsync();
    }
}

