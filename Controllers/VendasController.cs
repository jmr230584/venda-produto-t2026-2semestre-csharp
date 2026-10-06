/* 
    Importa os DTOs utilizados pelo Controller.

    Nesse caso, permite utilizar a classe
    CriarVendaDto, responsável por receber
    os dados enviados na criação da venda.
*/
using LojaApi.DTOs;

/* 
    Importa as entidades da aplicação.

    Nesse Controller, permite utilizar
    a entidade Venda como tipo de retorno
    das operações.
*/
using LojaApi.Entities;

/* 
    Importa os Services da aplicação.

    O Controller utiliza o VendaService
    para executar os casos de uso relacionados
    às vendas.

    O Controller não acessa o banco diretamente.
*/
using LojaApi.Services;

/* 
    Importa os recursos do ASP.NET Core MVC.

    Esse namespace fornece classes e atributos como:
    - ControllerBase;
    - ApiController;
    - Route;
    - HttpGet;
    - HttpPost;
    - ActionResult;
    - IActionResult.
*/
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers;

/* 
    Informa ao ASP.NET Core que esta classe
    será utilizada como Controller de uma API.

    [ApiController] também ativa alguns
    comportamentos automáticos, como
    a validação dos DTOs recebidos.
*/
[ApiController]

/* 
    Define a rota principal deste Controller.

    Todas as rotas declaradas nesta classe
    começarão com:

    /api/vendas
*/
[Route("api/vendas")]

/* 
    Controller responsável por receber
    as requisições HTTP relacionadas às vendas.

    Ele recebe a requisição,
    chama o VendaService
    e devolve a resposta HTTP.

    O Controller não deve:
    - escrever SQL;
    - validar estoque;
    - calcular o total da venda;
    - acessar diretamente o PostgreSQL.
*/
public class VendasController : ControllerBase
{
/* 
    Armazena uma referência para o VendaService.

    O VendaService contém os casos de uso
    e as regras de negócio relacionadas
    às vendas.

    private impede o acesso direto
    por outras classes.

    readonly impede que o atributo
    receba outro objeto depois
    da execução do construtor.
*/
    private readonly VendaService _vendaService;

/* 
    Construtor do VendasController.

    O ASP.NET Core fornecerá automaticamente
    um objeto VendaService por meio
    da injeção de dependência.

    Esse objeto será armazenado
    no atributo _vendaService.
*/
    public VendasController(
        VendaService vendaService)
    {
        _vendaService = vendaService;
    }

/* 
    Define uma rota HTTP GET.

    Como não existe nenhum trecho adicional
    no atributo [HttpGet], a rota completa será:

    GET /api/vendas

    Essa rota será utilizada para
    listar as vendas ativas.
*/
    [HttpGet]

/* 
    Método responsável por receber
    a requisição de listagem de vendas.

    Task indica que o método
    executa uma operação assíncrona.

    ActionResult<List<Venda>> informa
    que a resposta poderá conter
    uma lista de objetos Venda
    junto com um código HTTP.
*/
    public async Task<ActionResult<List<Venda>>> Listar()
    {
/* 
    Solicita ao VendaService
    a listagem das vendas ativas.

    O Controller não executa a consulta
    diretamente no banco.

    Essa responsabilidade é encaminhada
    para as outras camadas da aplicação.
*/
        List<Venda> vendas =
            await _vendaService.ListarAtivasAsync();

/* 
    Ok() cria uma resposta HTTP:

    200 OK

    A lista de vendas será convertida
    automaticamente para JSON
    pelo ASP.NET Core.
*/
        return Ok(vendas);
    }

/* 
    Define uma rota HTTP POST.

    Como não existe nenhum trecho adicional
    no atributo [HttpPost], a rota completa será:

    POST /api/vendas

    Essa rota será utilizada
    para registrar uma nova venda.
*/
    [HttpPost]

/* 
    Método responsável por receber
    uma solicitação de criação de venda.

    CriarVendaDto representa os dados
    enviados no corpo da requisição.

    O ASP.NET Core converte o JSON recebido
    em um objeto CriarVendaDto.

    As validações básicas declaradas
    no DTO serão verificadas automaticamente
    por causa do [ApiController].
*/
    public async Task<ActionResult<Venda>> Criar(
        CriarVendaDto dto)
    {
/* 
    Envia o DTO para o VendaService.

    O Service será responsável por:
    - validar produtos repetidos;
    - verificar se os produtos existem;
    - verificar o estoque;
    - obter os preços do banco;
    - calcular o valor total;
    - registrar a venda;
    - registrar os itens;
    - atualizar o estoque;
    - controlar a transação.

    Ao finalizar, o Service devolve
    o objeto Venda criado.
*/
        Venda venda =
            await _vendaService.CriarAsync(dto);

/* 
    Created() cria uma resposta HTTP:

    201 Created

    O primeiro argumento informa
    o endereço do recurso criado.

    Exemplo:
    /api/vendas/4

    O segundo argumento contém
    o objeto Venda criado,
    que será convertido para JSON.
*/
        return Created(
            $"/api/vendas/{venda.IdVenda}",
            venda
        );
    }
}