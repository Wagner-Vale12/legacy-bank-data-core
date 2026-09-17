using System;
using System.IO;
using LegacyBankDataCore.Web.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyBankDataCore.Tests
{
    [TestClass]
    public class XmlSchemaValidatorTests
    {
        private string ObterCaminhoXsd()
        {
            return Path.GetFullPath(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\LegacyBankDataCore.Web\App_Data\Schemas\movimentos.xsd"));
        }

        private string ObterCaminhoXml(string nomeArquivo)
        {
            return Path.GetFullPath(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\LegacyBankDataCore.Web\App_Data\Importacoes",
                    nomeArquivo));
        }

        [TestMethod]
        public void Validar_XmlValido_NaoDeveLancarExcecao()
        {
            // Arrange
            var validator =
                new XmlSchemaValidator();

            var caminhoXml =
                ObterCaminhoXml(
                    "movimentos_reprocessamento.xml");

            var caminhoXsd =
                ObterCaminhoXsd();

            // Act
            validator.Validar(
                caminhoXml,
                caminhoXsd);

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Validar_XmlForaDoLayout_DeveLancarArgumentException()
        {
            // Arrange
            var validator =
                new XmlSchemaValidator();

            var caminhoXml =
                ObterCaminhoXml(
                    "movimentos_xsd_invalido.xml");

            var caminhoXsd =
                ObterCaminhoXsd();

            // Act
            var exception =
                Assert.ThrowsException<ArgumentException>(
                    () => validator.Validar(
                        caminhoXml,
                        caminhoXsd));

            // Assert
            StringAssert.Contains(
                exception.Message,
                "não está de acordo com o layout esperado");
        }

        [TestMethod]
        public void Validar_XmlMalformado_DeveLancarArgumentException()
        {
            // Arrange
            var validator =
                new XmlSchemaValidator();

            var caminhoXml =
                ObterCaminhoXml(
                    "movimentos_malformado.xml");

            var caminhoXsd =
                ObterCaminhoXsd();

            // Act
            var exception =
                Assert.ThrowsException<ArgumentException>(
                    () => validator.Validar(
                        caminhoXml,
                        caminhoXsd));

            // Assert
            StringAssert.Contains(
                exception.Message,
                "XML está malformado");
        }
    }
}