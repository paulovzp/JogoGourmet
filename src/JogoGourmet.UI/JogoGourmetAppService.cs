using JogoGourmet.Domain;
using JogoGourmet.Persistence;
using JogoGourmet.UI.Models;

namespace JogoGourmet.UI;

public interface IJogoGourmetAppService
{
    void Start();
}

internal class JogoGourmetAppService : IJogoGourmetAppService
{
    private readonly JogoGourmetContext _context;

    public const string DiferencaEntre = "{0} é ________ mas {1} não:";
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
            var result = TentarAdivinhar(tiposPratos);
            if (result.Acertei)
            {
                ExibirAcertei();
                Thread.Sleep(3000);
            }
            else
                AdicionarNovaOpcao(result);

            Console.Clear();
        }
    }

    /// <summary>
    /// TO DO : Essa operação pode ser movida para um serviço de domino
    /// TO DO : Aproveitar os tipos que já existe e inserir um novo prato, não precisa criar um novo tipo toda vez.
    /// </summary>
    /// <param name="acertei"></param>
    private void AdicionarNovaOpcao(JogoGourmetResult result)
    {
        var prato = GetRespostaTexto(QualPratoPensou);
        var tipoPrato = GetRespostaTexto(string.Format(DiferencaEntre, prato, result.TipoPratoComparador));
        InserirNovoTipo(prato, tipoPrato);
    }

    private void InserirNovoTipo(string prato, string tipoPrato)
    {
        var novoTipoPrato = TipoPrato.Create(tipoPrato);
        novoTipoPrato.AddPrato(Prato.Create(prato));
        _context.AddTipoPrato(novoTipoPrato);
    }

    private JogoGourmetResult TentarAdivinhar(List<TipoPrato> tiposPratos)
    {
        foreach (var tipoPrato in tiposPratos.OrderBy(x => x.Order))
        {
            if (!string.IsNullOrEmpty(tipoPrato.Nome))
            {
                var messageTipo = string.Format(OPratoQueVocêPensou, tipoPrato.Nome);
                if (!GetResposta(messageTipo))
                    continue;
            }
            return TentarAdivinharPrato(tipoPrato);
        }
        return new JogoGourmetResult(false, string.Empty);
    }

    private JogoGourmetResult TentarAdivinharPrato(TipoPrato tipoPrato)
    {
        var currentPrato = string.Empty;
        foreach (var prato in tipoPrato.Pratos)
        {
            currentPrato = prato.Nome;
            var message = string.Format(OPratoQueVocêPensou, currentPrato);
            if (GetResposta(message))
            {
                return new JogoGourmetResult(true, string.Empty);
            }
        }

        return new JogoGourmetResult(false, currentPrato);
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
            Console.WriteLine();
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
