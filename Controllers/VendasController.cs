using LojaApi.Entities;
using LojaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers;

[ApiController]
[Route("api/vendas")]
public class VendasController : ControllerBase
{
    private readonly VendaService _vendaService;

    public VendasController(VendaService vendaService)
    {
        _vendaService = vendaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Venda>>> Listar()
    {
        List<Venda> vendas =
            await _vendaService.ListarAtivasAsync();

        return Ok(vendas);
    }
}