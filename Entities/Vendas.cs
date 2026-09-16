namespace LojaApi.Entities;

public class Venda
{
    private int _idVenda;
    private DateTime _dataHora;
    private decimal _valorTotal;
    private bool _ativa;

    /*
    Temos dois contrutores nessa classe, tecnicamente, os dois são um exemplo de sobrecarga de construtores: 
    mesma classe, mesmo nome Venda, mas parâmetros diferentes.
    Explicação técnica para o próximo semestre    
    */

    // Utilizado para criar uma venda nova.
    // O ID não é recebido porque será gerado pelo PostgreSQL.
    public Venda(decimal valorTotal)
    {
        this.DataHora = DateTime.Now;
        this.ValorTotal = valorTotal;
        this.Ativa = true;
    }

    // Construtor de reconstrução
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
}
