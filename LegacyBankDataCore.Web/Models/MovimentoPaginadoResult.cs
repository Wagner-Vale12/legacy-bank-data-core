using System.Collections.Generic;

namespace LegacyBankDataCore.Web.Models
{
    public class MovimentoPaginadoResult
    {
        public MovimentoPaginadoResult()
        {
            Itens = new List<Movimento>();
        }

        public List<Movimento> Itens { get; set; }

        public int TotalRegistros { get; set; }
    }
}