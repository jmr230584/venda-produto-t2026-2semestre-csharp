using LojaApi.Data;
using LojaApi.Repositories;
using LojaApi.Services;

// Cria o configurador da aplicação e carrega suas configurações.
WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);

// Habilita os Controllers, incluindo o de conexão e o de vendas.
builder.Services.AddControllers();

// Registra uma única instância de ConexaoBanco para a aplicação.
// Essa classe administra a fonte de dados e o conjunto de conexões.
builder.Services.AddSingleton<ConexaoBanco>();

// Registra o Repository responsável pelas consultas de vendas.
// AddScoped permite reutilizar a mesma instância durante uma requisição.
builder.Services.AddScoped<VendaRepository>();

// Registra o Service responsável por coordenar as operações de vendas.
builder.Services.AddScoped<VendaService>();

// Constrói a aplicação.
// await using libera seus recursos quando ela for encerrada.
await using WebApplication app = builder.Build();

// Disponibiliza as rotas dos Controllers registrados.
app.MapControllers();

// Inicia a API e aguarda a conclusão da inicialização.
// Diferentemente de Run(), permite continuar executando o código abaixo.
await app.StartAsync();

// Exibe os endereços após as mensagens de inicialização.
// A rota de teste será a última linha impressa neste bloco.
Console.WriteLine();
Console.WriteLine("Listar vendas: http://localhost:5037/api/vendas");
Console.WriteLine("http://localhost:5037/api/conexao/testar");

// Mantém a API em execução até o encerramento com Ctrl + C.
await app.WaitForShutdownAsync();