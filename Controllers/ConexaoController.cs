// Importa o namespace onde está localizada
// a classe ConexaoBanco.
using LojaApi.Data;

// Importa os recursos do ASP.NET Core MVC.
//
// Esse namespace fornece classes e atributos como:
// ControllerBase, ApiController, Route, HttpGet e IActionResult.
using Microsoft.AspNetCore.Mvc;

// Define que esta classe pertence ao namespace
// LojaApi.Controllers.
//
// O namespace ajuda a organizar os Controllers da aplicação.
namespace LojaApi.Controllers;

// Informa ao ASP.NET Core que esta classe
// será utilizada como Controller de uma API.
//
// Esse atributo também ativa comportamentos automáticos,
// como a validação dos dados recebidos pela API.
[ApiController]

// Define a rota principal deste Controller.
//
// Todas as rotas declaradas nesta classe começarão com:
// api/conexao
[Route("api/conexao")]

// Declara a classe pública ConexaoController.
//
// A classe herda de ControllerBase para receber recursos
// necessários à criação de Controllers de uma API,
// como os métodos Ok(), NotFound() e BadRequest().
public class ConexaoController : ControllerBase
{
    // Declara um atributo privado que armazenará
    // uma referência para a classe ConexaoBanco.
    //
    // readonly determina que o atributo somente poderá
    // receber um valor durante sua declaração
    // ou dentro do construtor.
    private readonly ConexaoBanco _conexaoBanco;

    // Construtor da classe ConexaoController.
    //
    // O ASP.NET Core fornecerá automaticamente um objeto
    // ConexaoBanco por meio da injeção de dependência.
    public ConexaoController(ConexaoBanco conexaoBanco)
    {
        // Armazena no atributo privado o objeto recebido
        // pelo parâmetro do construtor.
        _conexaoBanco = conexaoBanco;
    }

    // Define que o método abaixo responderá
    // a requisições HTTP do tipo GET.
    //
    // O trecho "testar" é acrescentado à rota principal.
    //
    // A rota completa será:
    // GET /api/conexao/testar
    [HttpGet("testar")]

    // Declara o método público e assíncrono Testar.
    //
    // Task indica que o método executará
    // uma operação assíncrona.
    //
    // IActionResult permite que o método devolva
    // uma resposta HTTP.
    public async Task<IActionResult> Testar()
    {
        // Chama o método TestarAsync() da classe ConexaoBanco.
        //
        // O await aguarda o término do teste de conexão
        // sem bloquear a aplicação.
        //
        // O resultado true ou false será armazenado
        // na variável conectou.
        bool conectou =
            await _conexaoBanco.TestarAsync();

        // O método Ok() cria uma resposta HTTP
        // com o código 200 — OK.
        //
        // O conteúdo colocado dentro de Ok()
        // será convertido automaticamente para JSON.
        return Ok(

            // Cria um objeto anônimo.
            //
            // Um objeto anônimo é um objeto criado
            // sem declarar uma classe específica para ele.
            new
            {
                // Cria a propriedade mensagem
                // que será enviada na resposta JSON.
                mensagem =

                    // O operador ternário verifica
                    // o valor da variável conectou.
                    conectou

                        // Se conectou for true,
                        // esta mensagem será utilizada.
                        ? "Conexão realizada com sucesso."

                        // Se conectou for false,
                        // esta mensagem será utilizada.
                        : "Não foi possível conectar."
            }
        );
    }
}