using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using LegacyBankDataCore.Web.Models;

namespace LegacyBankDataCore.Web.Services
{
    public class XmlMovimentoReader
    {
        public List<CriarMovimentoRequest> Ler(string caminhoArquivo)
        {
            var documento = XDocument.Load(caminhoArquivo);

            var movimentos =
                documento
                    .Descendants("Movimento")
                    .Select(elemento => new CriarMovimentoRequest
                    {
                        IdExterno =
                            elemento.Element("IdExterno")?.Value,

                        Conta =
                            elemento.Element("Conta")?.Value,

                        Tipo =
                            elemento.Element("Tipo")?.Value,

                        Valor = decimal.Parse(
                            elemento.Element("Valor")?.Value,
                            CultureInfo.InvariantCulture),

                        DataMovimento = DateTime.ParseExact(
                            elemento.Element("DataMovimento")?.Value,
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture)
                    })
                    .ToList();

            return movimentos;
        }
    }
}