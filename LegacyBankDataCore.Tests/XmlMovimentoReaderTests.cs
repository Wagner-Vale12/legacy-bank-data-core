using System;
using System.IO;
using LegacyBankDataCore.Web.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyBankDataCore.Tests
{
    [TestClass]
    public class XmlMovimentoReaderTests
    {
        [TestMethod]
        public void Ler_DeveRetornarTresMovimentosComDadosCorretos()
        {
            // Arrange
            var caminhoArquivo = Path.GetFullPath(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\LegacyBankDataCore.Web\App_Data\Importacoes\movimentos_exemplo.xml"
                )
            );

            var reader = new XmlMovimentoReader();

            // Act
            var movimentos = reader.Ler(caminhoArquivo);

            // Assert
            Assert.AreEqual(3, movimentos.Count);

            var primeiroMovimento = movimentos[0];

            Assert.AreEqual(
                "XML001",
                primeiroMovimento.IdExterno);

            Assert.AreEqual(
                "10001",
                primeiroMovimento.Conta);

            Assert.AreEqual(
                "ENTRADA",
                primeiroMovimento.Tipo);

            Assert.AreEqual(
                1500.50m,
                primeiroMovimento.Valor);

            Assert.AreEqual(
                new DateTime(2026, 9, 15),
                primeiroMovimento.DataMovimento);
        }
    }
}