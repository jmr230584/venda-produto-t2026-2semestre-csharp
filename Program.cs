/* 
    Importa a classe ConexaoBanco.

    Essa classe será registrada no sistema
    de injeção de dependência e utilizada
    pelas classes que precisam acessar
    o PostgreSQL.
*/
using LojaApi.Data;

/* 
    Importa os Middlewares da aplicação.

    Nesse caso, permite utilizar
    o TratamentoErrosMiddleware,
    responsável por transformar exceções
    em respostas HTTP adequadas.
*/
using LojaApi.Middlewares;

/* 
    Importa os Repositories da aplicação.

    Eles serão registrados para que
    o ASP.NET Core possa fornecê-los
    automaticamente aos Services.

    Exemplos:
    - ProdutoRepository;
    - VendaRepository;
    - ItemVendaRepository.
*/
using LojaApi.Repositories;

/* 
    Importa os Services da aplicação.

    Eles serão registrados para que
    o ASP.NET Core possa fornecê-los
    automaticamente aos Controllers.

    Exemplos:
    - ProdutoService;
    - VendaService.
*/
using LojaApi.Services;


/* 
    Cria o configurador principal da aplicação.

    WebApplicationBuilder carrega:
    - configurações;
    - serviços;
    - arquivos appsettings;
    - recursos do ASP.NET Core.

    args contém os argumentos recebidos
    quando a aplicação é executada.
*/
WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);


/* 
    Habilita o uso de Controllers na aplicação.

    Isso permite que o ASP.NET Core
    reconheça classes como:
    - ConexaoController;
    - ProdutosController;
    - VendasController.

    Sem essa configuração,
    os Controllers não seriam utilizados.
*/
builder.Services.AddControllers();


/* 
    Registra a classe ConexaoBanco
    no sistema de injeção de dependência.

    AddSingleton cria apenas uma instância
    de ConexaoBanco durante toda a execução
    da aplicação.

    Essa instância será reutilizada
    sempre que alguma classe precisar
    de um objeto ConexaoBanco.
*/
builder.Services.AddSingleton<ConexaoBanco>();


/* 
    Registra o ProdutoRepository.

    AddScoped cria uma instância
    para cada requisição HTTP.

    Durante uma mesma requisição,
    a mesma instância poderá ser reutilizada.
*/
builder.Services.AddScoped<ProdutoRepository>();


/* 
    Registra o VendaRepository
    no sistema de injeção de dependência.

    O VendaService poderá recebê-lo
    automaticamente pelo construtor.
*/
builder.Services.AddScoped<VendaRepository>();


/* 
    Registra o ItemVendaRepository.

    Esse Repository será utilizado
    principalmente durante o cadastro
    dos itens de uma venda.
*/
builder.Services.AddScoped<ItemVendaRepository>();


/* 
    Registra o ProdutoService.

    Esse Service coordena os casos de uso
    relacionados aos produtos.
*/
//builder.Services.AddScoped<ProdutoService>();


/* 
    Registra o VendaService.

    Esse Service coordena os casos de uso
    e as regras de negócio relacionadas
    às vendas.
*/
builder.Services.AddScoped<VendaService>();


/* 
    Constrói a aplicação utilizando
    todas as configurações e serviços
    registrados anteriormente.

    await using garante que os recursos
    da aplicação sejam liberados corretamente
    quando ela for encerrada.
*/
await using WebApplication app =
    builder.Build();


/* 
    Adiciona o TratamentoErrosMiddleware
    ao pipeline da aplicação.

    A partir deste ponto,
    as requisições passarão por esse Middleware.

    Se alguma camada lançar uma exceção,
    ele poderá capturá-la e transformar
    em uma resposta HTTP adequada.

    Exemplos:
    - 404 Not Found;
    - 409 Conflict;
    - 400 Bad Request;
    - 500 Internal Server Error.
*/
app.UseMiddleware<TratamentoErrosMiddleware>();


/* 
    Disponibiliza as rotas
    declaradas nos Controllers.

    Exemplos:
    GET /api/vendas
    POST /api/vendas
    GET /api/produtos
*/
app.MapControllers();


/* 
    Inicia a aplicação.

    StartAsync inicia o servidor,
    mas permite continuar executando
    as linhas abaixo.

    Isso possibilita imprimir
    informações no terminal depois
    que a aplicação já foi iniciada.
*/
await app.StartAsync();


/* 
    Apenas adiciona uma linha vazia
    no terminal para melhorar
    a organização das mensagens.
*/
Console.WriteLine();


/* 
    Exibe no terminal a URL
    utilizada para listar vendas.
*/
Console.WriteLine(
    "Listar vendas: http://localhost:5037/api/vendas"
);


/* 
    Exibe no terminal a rota
    utilizada para cadastrar uma venda.

    A informação POST ajuda a lembrar
    qual método HTTP deve ser utilizado.
*/
Console.WriteLine(
    "Cadastrar venda: POST http://localhost:5037/api/vendas"
);


/* 
    Exibe a rota didática
    utilizada para testar a conexão
    com o PostgreSQL.
*/
Console.WriteLine(
    "Teste conexão: http://localhost:5037/api/conexao/testar"
);


/* 
    Mantém a aplicação em execução.

    WaitForShutdownAsync aguarda
    até que o servidor seja encerrado.

    Normalmente, no terminal,
    isso acontece quando utilizamos:

    Ctrl + C
*/
await app.WaitForShutdownAsync();