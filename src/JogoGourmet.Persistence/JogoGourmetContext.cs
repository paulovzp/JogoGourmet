using JogoGourmet.Domain;

namespace JogoGourmet.Persistence
{
    public class JogoGourmetContext
    {
        public IReadOnlyCollection<TipoPrato> TiposPratos { get { return _tiposPratos; } }
        private List<TipoPrato> _tiposPratos = [];

        /// <summary>
        /// Trabalhar com o context aqui traz flexibilidade
        /// A lista pode ta em memoria, em um banco de dados, em um arquivo, etc
        /// </summary>
        public JogoGourmetContext()
        {
            InitDataBase();
        }

        private void InitDataBase()
        {
            TipoPrato tipoPrato = TipoPrato.Create("massa", 1);
            tipoPrato.AddPrato(Prato.Create("Lasanha"));

            _tiposPratos.Add(tipoPrato);

            TipoPrato tipoPrato2 = TipoPrato.Create(string.Empty, _tiposPratos.Count + 1);
            tipoPrato2.AddPrato(Prato.Create("Bolo de Chocolate"));
            _tiposPratos.Add(tipoPrato2);
        }

        public void AddPrato(string tipoPrato, string prato)
        {
            var tipo = _tiposPratos.First(x => x.Nome == tipoPrato);
            tipo.AddPrato(Prato.Create(prato));
        }

        /// <summary>
        /// TO DO: Mover para um serviço de domínio a regra de reordenar o tipo de prato default (vazio)
        /// </summary>
        /// <param name="tipoPrato"></param>
        public void AddTipoPrato(TipoPrato tipoPrato)
        {
            tipoPrato.SetOrder(_tiposPratos.Count - 1);
            _tiposPratos.Add(tipoPrato);
            var tipoVazio = GetTiposPratosVazio();
            tipoVazio.SetOrder(_tiposPratos.Count);
        }

        /// <summary>
        /// TO DO: Mover para um serviço de domínio, pois a regra de negócio é que o tipo de prato vazio sempre seja o último
        /// </summary>
        /// <returns></returns>
        public TipoPrato GetTiposPratosVazio()
        {
            return _tiposPratos.First(x => string.IsNullOrEmpty(x.Nome));
        }

        /// <summary>
        /// TO DO:  Mover para um serviço de domínio, e criar um flag para saber o prato default daquele tipo
        /// </summary>
        /// <returns></returns>
        public Prato GetBoloChocolate()
        {
            return GetTiposPratosVazio().Pratos.First();
        }


    }
}
