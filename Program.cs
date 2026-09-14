using LojaApi.Data;

WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);

// Habilita o uso de Controllers.
builder.Services.AddControllers();

// Registra a classe de conexão para que o ASP.NET Core
// possa fornecê-la ao ConexaoController.
builder.Services.AddSingleton<ConexaoBanco>();

WebApplication app = builder.Build();

// Disponibiliza as rotas declaradas nos Controllers.
app.MapControllers();

// Inicia a aplicação.
app.Run();