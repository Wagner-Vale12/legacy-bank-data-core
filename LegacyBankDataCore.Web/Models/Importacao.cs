using System;

namespace LegacyBankDataCore.Web.Models
{
    public class Importacao
    {
        public int Id { get; set; }

        public string NomeArquivo { get; set; }

        public string Status { get; set; }

        public int TotalRegistros { get; set; }

        public DateTime DataRecebimento { get; set; }

        public DateTime? DataProcessamento { get; set; }

        public string MensagemErro { get; set; }

        public string HashArquivo { get; set; }
    }
}