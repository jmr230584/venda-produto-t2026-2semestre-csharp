using System.ComponentModel.DataAnnotations;

namespace LojaApi.DTOs;

public class CriarVendaDto
{
/* 
    A venda deve possuir pelo menos um item.

    [Required] define que a lista de itens é obrigatória.
    Isso impede que a requisição seja enviada sem informar
    a propriedade Itens.

    [MinLength(1)] define que a lista deve possuir
    pelo menos um elemento.

    Na prática, impede a criação de uma venda
    sem nenhum produto informado.
*/
    [Required]
    [MinLength(1)]
    public List<CriarItemVendaDto> Itens { get; set; } = new();

/* 
    List<CriarItemVendaDto> representa uma lista de itens
    enviados para compor a venda.

    Cada elemento da lista deverá seguir as validações
    definidas na classe CriarItemVendaDto.

    = new() cria uma lista vazia automaticamente,
    evitando que a propriedade fique nula dentro da aplicação.
*/
}