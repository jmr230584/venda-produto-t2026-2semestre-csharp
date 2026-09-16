// Importa o namespace que contém a entidade Venda.
//
// A entidade Venda será utilizada como tipo dos objetos
// que serão devolvidos pelo Service.
using LojaApi.Entities;

// Importa o namespace que contém o VendaRepository.
//
// O Service utilizará o Repository para acessar
// os dados armazenados no PostgreSQL.
using LojaApi.Repositories;

// Define que esta classe pertence ao namespace
// LojaApi.Services.
//
// Esse namespace organiza as classes responsáveis
// pelos casos de uso e pelas regras de negócio.
namespace LojaApi.Services;

// Declara a classe pública VendaService.
//
// O Service fica entre o Controller e o Repository.
public class VendaService
{
    // Declara um atributo privado que armazenará
    // uma referência para o VendaRepository.
    //
    // private impede que o atributo seja acessado
    // diretamente por outras classes.
    //
    // readonly impede que o atributo receba outro objeto
    // depois da execução do construtor.
    private readonly VendaRepository _vendaRepository;

    // Construtor da classe VendaService.
    //
    // O ASP.NET Core fornecerá automaticamente
    // um objeto VendaRepository por meio
    // da injeção de dependência.
    public VendaService(VendaRepository vendaRepository)
    {
        // Armazena no atributo privado o Repository
        // recebido pelo parâmetro do construtor.
        _vendaRepository = vendaRepository;
    }

    // Declara um método público e assíncrono
    // responsável pelo caso de uso de listar
    // as vendas ativas.
    //
    // Task indica que o método executará
    // uma operação assíncrona.
    //
    // List<Venda> indica que, quando terminar,
    // o método devolverá uma lista de objetos Venda.
    public async Task<List<Venda>> ListarAtivasAsync()
    {
        // Chama o método ListarAtivasAsync()
        // existente no VendaRepository.
        //
        // O Repository acessará o PostgreSQL,
        // executará o SELECT e montará a lista.
        //
        // await aguarda o término da consulta
        // sem bloquear a aplicação.
        //
        // return devolve a lista recebida
        // para a classe que chamou o Service.
        return await _vendaRepository.ListarAtivasAsync();
    }
}