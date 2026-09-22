using System.Collections.Generic;

namespace LegacyBankDataCore.Web.Models
{
    public class ImportacaoPaginadoResult
    {
        public ImportacaoPaginadoResult()
        {
            Itens = new List<Importacao>();
        }

        public List<Importacao> Itens { get; set; }

        public int TotalRegistros { get; set; }
    }
}