namespace JogoGourmet.UI.Models;

public class JogoGourmetResult
{
    public JogoGourmetResult(bool acertei, string tipoPratoComparador)
    {
        Acertei = acertei;
        TipoPratoComparador = tipoPratoComparador;
    }

    public bool Acertei { get; set; }
    public string TipoPratoComparador { get; set; }
}
