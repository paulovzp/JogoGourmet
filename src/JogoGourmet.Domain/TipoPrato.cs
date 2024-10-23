namespace JogoGourmet.Domain;

public class TipoPrato
{
    public int Order { get; private set; }
    public string Nome { get; private set; }
    public IReadOnlyCollection<Prato> Pratos { get { return _pratos; } }
    private List<Prato> _pratos = [];

    public TipoPrato(string nome, int order)
    {
        Nome = nome;
        Order = order;
    }

    public void AddPrato(Prato prato)
    {
        _pratos.Add(prato);
    }

    public void SetOrder(int order)
    {
        Order = order;
    }

    public static TipoPrato Create(string nome, int order = 0)
    {
        return new TipoPrato(nome, order);
    }
}
