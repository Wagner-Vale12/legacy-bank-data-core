using System.IO;
using LegacyBankDataCore.Web.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyBankDataCore.Tests
{
    [TestClass]
    public class ProcessarImportacaoServiceTests
    {
        [TestMethod]
        public void Processar_ArquivoInexistente_DeveLancarFileNotFoundException()
        {
            // Arrange
            var service = new ProcessarImportacaoService(
                null,
                null,
                null);

            var caminhoArquivo =
                @"C:\arquivo_que_nao_existe\movimentos.xml";

            // Act + Assert
            Assert.ThrowsException<FileNotFoundException>(
                () => service.Processar(caminhoArquivo)
            );
        }
    }
}