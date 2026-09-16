using System;
using System.IO;
using System.Security.Cryptography;

namespace LegacyBankDataCore.Web.Services
{
    public class HashArquivoService
    {
        public string Calcular(string caminhoArquivo)
        {
            if (string.IsNullOrWhiteSpace(caminhoArquivo))
            {
                throw new ArgumentException(
                    "Caminho do arquivo é obrigatório.");
            }

            if (!File.Exists(caminhoArquivo))
            {
                throw new FileNotFoundException(
                    "Arquivo não encontrado.",
                    caminhoArquivo);
            }

            using (var stream = File.OpenRead(caminhoArquivo))
            using (var sha256 = SHA256.Create())
            {
                var hashBytes =
                    sha256.ComputeHash(stream);

                return BitConverter
                    .ToString(hashBytes)
                    .Replace("-", "")
                    .ToLowerInvariant();
            }
        }
    }
}