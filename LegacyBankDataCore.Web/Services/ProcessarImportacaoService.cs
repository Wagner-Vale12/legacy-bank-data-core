using System;
using System.IO;
using LegacyBankDataCore.Web.Repositories;

namespace LegacyBankDataCore.Web.Services
{
    public class ProcessarImportacaoService
    {
        private readonly ImportacaoService _importacaoService;
        private readonly MovimentoService _movimentoService;
        private readonly XmlMovimentoReader _xmlReader;
        private readonly ProcessamentoImportacaoRepository _processamentoRepository;

        private readonly HashArquivoService _hashArquivoService;

        public ProcessarImportacaoService(
            ImportacaoService importacaoService,
            MovimentoService movimentoService,
            XmlMovimentoReader xmlReader,
            ProcessamentoImportacaoRepository processamentoRepository,
            HashArquivoService hashArquivoService)
        {
            _importacaoService = importacaoService;
            _movimentoService = movimentoService;
            _xmlReader = xmlReader;
            _processamentoRepository = processamentoRepository;
            _hashArquivoService = hashArquivoService;
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

            var hashArquivo =
                _hashArquivoService.Calcular(caminhoArquivo);

            var importacaoId =
                _importacaoService.Criar(
                    nomeArquivo,
                    hashArquivo);

            var processamentoIniciado = false;

            try
            {
                _importacaoService
                    .IniciarProcessamento(importacaoId);

                processamentoIniciado = true;

                var movimentos =
                    _xmlReader.Ler(caminhoArquivo);

                _movimentoService
                    .ValidarLote(movimentos);

                _processamentoRepository
                    .PersistirEConcluir(
                        importacaoId,
                        movimentos);

                return importacaoId;
            }
            catch (Exception ex)
            {
                if (processamentoIniciado)
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
                }

                throw;
            }
        }
    }
}