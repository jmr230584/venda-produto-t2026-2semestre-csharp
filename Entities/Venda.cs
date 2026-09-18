namespace LojaApi.Entities;

public class Venda
{
    private int _idVenda;
    private DateTime _dataHora;
    private decimal _valorTotal;
    private bool _ativa;

    // Armazena os itens associados à venda.
    //
    // readonly impede a substituição da lista por outra,
    // mas permite adicionar elementos à lista existente.
    private readonly List<ItemVenda> _itens = new();

    // Utilizado para criar uma venda nova.
    // O ID não é recebido porque será gerado pelo PostgreSQL.
    public Venda(decimal valorTotal)
    {
        this.DataHora = DateTime.Now;
        this.ValorTotal = valorTotal;
        this.Ativa = true;
    }

    // Utilizado para criar um objeto com os dados
    // de uma venda que já está cadastrada no banco.
    public Venda(
        int idVenda,
        DateTime dataHora,
        decimal valorTotal,
        bool ativa)
    {
        this.IdVenda = idVenda;
        this.DataHora = dataHora;
        this.ValorTotal = valorTotal;
        this.Ativa = ativa;
    }

    public int IdVenda
    {
        get
        {
            return _idVenda;
        }

        private set
        {
            // Impede que o objeto receba um identificador negativo.
            // O valor 0 é permitido porque representa uma venda
            // que ainda não foi cadastrada no banco.
            if (value < 0)
            {
                throw new ArgumentException(
                    "O identificador não pode ser negativo."
                );
            }

            _idVenda = value;
        }
    }

    public DateTime DataHora
    {
        get
        {
            return _dataHora;
        }

        private set
        {
            // Impede que a venda seja criada com uma data futura.
            // A tolerância de um minuto evita problemas causados
            // por pequenas diferenças durante a execução.
            if (value > DateTime.Now.AddMinutes(1))
            {
                throw new ArgumentException(
                    "A data da venda não pode ser futura."
                );
            }

            _dataHora = value;
        }
    }

    public decimal ValorTotal
    {
        get
        {
            return _valorTotal;
        }

        private set
        {
            // Impede a criação de uma venda com valor
            // igual ou menor que zero.
            if (value <= 0)
            {
                throw new ArgumentException(
                    "O total deve ser maior que zero."
                );
            }

            _valorTotal = value;
        }
    }

    public bool Ativa
    {
        get
        {
            return _ativa;
        }

        private set
        {
            _ativa = value;
        }
    }

    // Disponibiliza os itens para consulta.
    //
    // IReadOnlyList permite percorrer e consultar os elementos,
    // mas não oferece métodos para adicionar ou remover itens.
    //
    // AsReadOnly protege a lista interna contra alterações diretas.
    public IReadOnlyList<ItemVenda> Itens
    {
        get
        {
            return _itens.AsReadOnly();
        }
    }

    // Adiciona à venda um item carregado pelo Repository.
    //
    // Este método apenas monta o objeto em memória.
    // Ele não executa INSERT nem altera o banco de dados.
    public void AdicionarItem(ItemVenda item)
    {
        // Impede a inclusão de uma referência nula.
        if (item is null)
        {
            throw new ArgumentNullException(
                nameof(item),
                "O item da venda não pode ser nulo."
            );
        }

        _itens.Add(item);
    }
}