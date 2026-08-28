using System;

namespace LegacyBankDataCore.Web.Models
{
    public class MovimentoViewModel
    {
        public string IdExterno { get; set; }

        public string Conta { get; set; }

        public string Tipo { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }
    }
}