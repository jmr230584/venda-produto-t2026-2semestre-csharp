using System.ComponentModel.DataAnnotations;

namespace LojaApi.DTOs;

public class CriarItemVendaDto
{
/* 
    O cliente deve informar um identificador de produto válido.
    Define que o valor informado deve estar entre 1
    e o maior valor permitido pelo tipo int.
    Na prática, impede valores iguais a zero ou negativos.
*/
    [Range(1, int.MaxValue)]
    public int ProdutoId { get; set; }

/* Uma venda não poderá possuir quantidade zero ou negativa.
    Aplica a mesma validação para a quantidade.
    Portanto, a quantidade mínima permitida é 1. 
*/
    [Range(1, int.MaxValue)]
    public int Quantidade { get; set; }

/* 
[Range] define o intervalo de valores aceitos.
   O primeiro valor (1) é o mínimo permitido.
   int.MaxValue representa o maior valor possível para um int.
   Assim, essa validação impede valores zero e negativos. 
   */
}