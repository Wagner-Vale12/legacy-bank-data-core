using System;
using System.IO;

namespace LegacyBankDataCore.Web.Services
{
    public class ProcessarImportacaoService
    {
        private readonly ImportacaoService _importacaoService;
        private readonly MovimentoService _movimentoService;
        private readonly XmlMovimentoReader _xmlReader;

        public ProcessarImportacaoService(
            ImportacaoService importacaoService,
            MovimentoService movimentoService,
            XmlMovimentoReader xmlReader)
        {
            _importacaoService = importacaoService;
            _movimentoService = movimentoService;
            _xmlReader = xmlReader;
        }

        public int Processar(string caminhoArquivo)
        {
            if (string.IsNullOrWhiteSpace(caminhoArquivo))
            {
                throw new ArgumentException(
                    "Caminho do arquivo é obrigatório.");
            }

            if (!File.Exists(caminhoArquivo))
            {
                throw new FileNotFoundException(
                    "Arquivo XML não encontrado.",
                    caminhoArquivo);
            }

            var nomeArquivo =
                Path.GetFileName(caminhoArquivo);

            var importacaoId =
                _importacaoService.Criar(nomeArquivo);

            _importacaoService
                .IniciarProcessamento(importacaoId);

            try
            {
                var movimentos =
                    _xmlReader.Ler(caminhoArquivo);

                var totalInseridos =
                    _movimentoService.CriarLote(movimentos);

                _importacaoService.Concluir(
                    importacaoId,
                    totalInseridos);

                return importacaoId;
            }
            catch (Exception ex)
            {
                var mensagemErro = ex.Message;

                if (mensagemErro.Length > 1000)
                {
                    mensagemErro =
                        mensagemErro.Substring(0, 1000);
                }

                _importacaoService.RegistrarErro(
                    importacaoId,
                    mensagemErro);

                throw;
            }
        }
    }
}