using JogoGourmet.Domain;
using JogoGourmet.Persistence;

namespace JogoGourmet.UI;

public interface IJogoGourmetAppService
{
    void Start();
}

internal class JogoGourmetAppService : IJogoGourmetAppService
{
    private readonly JogoGourmetContext _context;

    public const string DiferencaEntre = "{0} é ________ mas {1} não: ";
    public const string Acertei = "Acertei de novo!";
    public const string PenseEmUmPrato = "Pense em um prato que você gosta";
    public const string QualPratoPensou = "Qual prato você pensou?";
    public const string OPratoQueVocêPensou = "O prato que você pensou é {0}?";

    public JogoGourmetAppService(JogoGourmetContext context)
    {
        _context = context;
    }

    public void Start()
    {
        while (true)
        {
            ExibirMsgInicial();
            var tiposPratos = GetTipoPratosOrdernados();
            bool acertei = TentarAdivinhar(tiposPratos);
            if (acertei)
            {
                ExibirAcertei();
                Thread.Sleep(3000);
            }
            else
                AdicionarNovaOpcao();

            Console.Clear();
        }
    }

    /// <summary>
    /// TO DO : Essa operação pode ser movida para um serviço de domino
    /// TO DO : Aproveitar os tipos que já existe e inserir um novo prato, não precisa criar um novo tipo toda vez.
    /// </summary>
    /// <param name="acertei"></param>
    private void AdicionarNovaOpcao()
    {
        var boloChocolate = _context.GetBoloChocolate();
        var prato = GetRespostaTexto(QualPratoPensou);
        var tipoPrato = GetRespostaTexto(string.Format(DiferencaEntre, prato, boloChocolate.Nome));
        InserirNovoTipo(prato, tipoPrato);
    }

    private void InserirNovoTipo(string prato, string tipoPrato)
    {
        var novoTipoPrato = TipoPrato.Create(tipoPrato);
        novoTipoPrato.AddPrato(Prato.Create(prato));
        _context.AddTipoPrato(novoTipoPrato);
    }

    private bool TentarAdivinhar(List<TipoPrato> tiposPratos)
    {
        foreach (var tipoPrato in tiposPratos.OrderBy(x => x.Order))
        {
            if (!string.IsNullOrEmpty(tipoPrato.Nome))
            {
                var messageTipo = string.Format(OPratoQueVocêPensou, tipoPrato.Nome);
                if (!GetResposta(messageTipo))
                    continue;
            }
            return TentarAdivinharPrato(tipoPrato.Pratos);
        }
        return false;
    }

    private bool TentarAdivinharPrato(IEnumerable<Prato> pratos)
    {
        foreach (var prato in pratos)
        {
            var message = string.Format(OPratoQueVocêPensou, prato.Nome);
            if (GetResposta(message))
            {
                return true;
            }
        }
        return false;
    }

    private void ExibirAcertei()
    {
        Console.WriteLine();
        Console.WriteLine(Acertei);
        Console.WriteLine();
    }

    private void ExibirMsgInicial()
    {
        Console.WriteLine();
        Console.WriteLine(PenseEmUmPrato);
        Console.WriteLine();
    }

    private List<TipoPrato> GetTipoPratosOrdernados()
    {
        return _context.TiposPratos.OrderBy(x => x.Order).ToList();
    }

    private bool GetResposta(string message)
    {
        while (true)
        {
            Console.WriteLine(message);
            string? respostaStr = Console.ReadLine()?.Trim().ToUpperInvariant();

            if (respostaStr == "S")
            {
                return true;
            }
            else if (respostaStr == "N")
            {
                return false;
            }
            else
            {
                ApresentarMensagemInvalida();
            }
        }
    }

    private static string GetRespostaTexto(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? resposta = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(resposta))
            {
                return resposta;
            }
            else
            {
                Console.WriteLine("Resposta não pode ser em branco");
            }
        }
    }

    private static void ApresentarMensagemInvalida()
    {
        Console.WriteLine("Resposta inválida");
        Console.WriteLine();
        Console.WriteLine("Digite S para sim e N para não");
        Console.WriteLine();
    }

}
