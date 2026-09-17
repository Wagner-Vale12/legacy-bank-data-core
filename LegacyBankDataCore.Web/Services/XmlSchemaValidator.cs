using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LegacyBankDataCore.Web.Services
{
    public class XmlSchemaValidator
    {
        public void Validar(
            string caminhoXml,
            string caminhoXsd)
        {
            if (string.IsNullOrWhiteSpace(caminhoXml))
            {
                throw new ArgumentException(
                    "Caminho do XML é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(caminhoXsd))
            {
                throw new ArgumentException(
                    "Caminho do XSD é obrigatório.");
            }

            if (!File.Exists(caminhoXml))
            {
                throw new FileNotFoundException(
                    "Arquivo XML não encontrado.",
                    caminhoXml);
            }

            if (!File.Exists(caminhoXsd))
            {
                throw new FileNotFoundException(
                    "Arquivo XSD não encontrado.",
                    caminhoXsd);
            }

            var erros = new List<string>();

            var settings = new XmlReaderSettings();

            settings.ValidationType =
                ValidationType.Schema;

            settings.Schemas.Add(
                null,
                caminhoXsd);

            settings.DtdProcessing =
                DtdProcessing.Prohibit;

            settings.XmlResolver = null;

            settings.MaxCharactersInDocument =
                10 * 1024 * 1024;

            settings.ValidationEventHandler +=
                (sender, args) =>
                {
                    erros.Add(args.Message);
                };

            try
            {
                using (var reader =
                    XmlReader.Create(
                        caminhoXml,
                        settings))
                {
                    while (reader.Read())
                    {
                    }
                }
            }
            catch (XmlException ex)
            {
                throw new ArgumentException(
                    "O arquivo XML está malformado.",
                    ex);
            }

            if (erros.Count > 0)
            {
                throw new ArgumentException(
                    "O arquivo XML não está de acordo " +
                    "com o layout esperado. " +
                    string.Join(" | ", erros));
            }
        }
    }
}