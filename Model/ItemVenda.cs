namespace LojaApi.Entities;

public class ItemVenda
{
    private int _idItemVenda;
    private int _idProduto;
    private string _nomeProduto = "";
    private int _quantidade;
    private decimal _valorUnitario;

    // Utilizado para criar um objeto com os dados
    // de um item que já está cadastrado no banco.
    public ItemVenda(
        int idItemVenda,
        int idProduto,
        string nomeProduto,
        int quantidade,
        decimal valorUnitario)
    {
        this.IdItemVenda = idItemVenda;
        this.IdProduto = idProduto;
        this.NomeProduto = nomeProduto;
        this.Quantidade = quantidade;
        this.ValorUnitario = valorUnitario;
    }

    public int IdItemVenda
    {
        get
        {
            return _idItemVenda;
        }

        private set
        {
            // Um item carregado do banco deve possuir
            // um identificador maior que zero.
            if (value <= 0)
            {
                throw new ArgumentException(
                    "O identificador do item deve ser maior que zero."
                );
            }

            _idItemVenda = value;
        }
    }

    public int IdProduto
    {
        get
        {
            return _idProduto;
        }

        private set
        {
            // O item deve estar associado a um produto válido.
            if (value <= 0)
            {
                throw new ArgumentException(
                    "O identificador do produto deve ser maior que zero."
                );
            }

            _idProduto = value;
        }
    }

    public string NomeProduto
    {
        get
        {
            return _nomeProduto;
        }

        private set
        {
            // Impede que o item possua um nome de produto vazio.
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "O nome do produto não pode ficar vazio."
                );
            }

            _nomeProduto = value.Trim();
        }
    }

    public int Quantidade
    {
        get
        {
            return _quantidade;
        }

        private set
        {
            // A quantidade vendida deve ser maior que zero.
            if (value <= 0)
            {
                throw new ArgumentException(
                    "A quantidade deve ser maior que zero."
                );
            }

            _quantidade = value;
        }
    }

    public decimal ValorUnitario
    {
        get
        {
            return _valorUnitario;
        }

        private set
        {
            // Impede um valor unitário igual ou menor que zero.
            if (value <= 0)
            {
                throw new ArgumentException(
                    "O valor unitário deve ser maior que zero."
                );
            }

            _valorUnitario = value;
        }
    }

    // Propriedade calculada: não precisa de um atributo privado
    // nem de um set, pois seu valor depende de outras propriedades.
    //
    // Utiliza o preço registrado no item da venda,
    // e não o preço atual do cadastro do produto.
    public decimal ValorTotalItem
    {
        get
        {
            return Quantidade * ValorUnitario;
        }
    }
}