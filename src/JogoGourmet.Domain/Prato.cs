namespace JogoGourmet.Domain;

public class Prato
{
    public Prato(string nome)
    {
        Nome = nome;
    }

    public string Nome { get; private set; }

    public static Prato Create(string nome)
    {
        return new Prato(nome);
    }
}