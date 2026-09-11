using System;

namespace LegacyBankDataCore.Web.Exceptions
{
    public class MovimentoDuplicadoException : Exception
    {
        public MovimentoDuplicadoException()
            : base("Já existe um movimento com o IdExterno informado.")
        {
        }
    }
}