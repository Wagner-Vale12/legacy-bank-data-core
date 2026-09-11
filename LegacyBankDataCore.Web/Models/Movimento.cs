using System;

namespace LegacyBankDataCore.Web.Models
{
    public class Movimento
    {
        public int Id { get; set; }

        public string IdExterno { get; set; }

        public string Conta { get; set; }

        public string Tipo { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataMovimento { get; set; }

        public DateTime CriadoEm { get; set; }
    }
}