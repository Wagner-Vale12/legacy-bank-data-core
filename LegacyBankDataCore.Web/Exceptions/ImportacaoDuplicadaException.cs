using System;

namespace LegacyBankDataCore.Web.Exceptions
{
    public class ImportacaoDuplicadaException : Exception
    {
        public ImportacaoDuplicadaException()
            : base("Este arquivo já foi processado anteriormente.")
        {
        }
    }
}