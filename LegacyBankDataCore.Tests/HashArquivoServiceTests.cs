using System;
using System.IO;
using LegacyBankDataCore.Web.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyBankDataCore.Tests
{
    [TestClass]
    public class HashArquivoServiceTests
    {
        [TestMethod]
        public void Calcular_MesmoArquivo_DeveRetornarMesmoHash()
        {
            // Arrange
            var caminhoArquivo = Path.GetFullPath(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\LegacyBankDataCore.Web\App_Data\Importacoes\movimentos_exemplo.xml"
                )
            );

            var service = new HashArquivoService();

            // Act
            var primeiroHash =
                service.Calcular(caminhoArquivo);

            var segundoHash =
                service.Calcular(caminhoArquivo);

            // Assert
            Assert.AreEqual(
                primeiroHash,
                segundoHash);
        }

        [TestMethod]
        public void Calcular_DeveRetornarHashCom64Caracteres()
        {
            // Arrange
            var caminhoArquivo = Path.GetFullPath(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\LegacyBankDataCore.Web\App_Data\Importacoes\movimentos_exemplo.xml"
                )
            );

            var service = new HashArquivoService();

            // Act
            var hash =
                service.Calcular(caminhoArquivo);

            // Assert
            Assert.IsNotNull(hash);

            Assert.AreEqual(
                64,
                hash.Length);
        }
    }
}