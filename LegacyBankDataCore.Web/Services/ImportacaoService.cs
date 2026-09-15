using System;
using LegacyBankDataCore.Web.Repositories;

namespace LegacyBankDataCore.Web.Services
{
    public class ImportacaoService
    {
        private readonly ImportacaoRepository _repository;

        public ImportacaoService(ImportacaoRepository repository)
        {
            _repository = repository;
        }

        public int Criar(string nomeArquivo)
        {
            if (string.IsNullOrWhiteSpace(nomeArquivo))
            {
                throw new ArgumentException(
                    "Nome do arquivo é obrigatório.");
            }

            if (nomeArquivo.Length > 255)
            {
                throw new ArgumentException(
                    "Nome do arquivo não pode ultrapassar 255 caracteres.");
            }

            return _repository.Criar(nomeArquivo);
        }
        public void IniciarProcessamento(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Id da importação deve ser maior que zero.");
            }

            var iniciado = _repository.IniciarProcessamento(id);

            if (!iniciado)
            {
                throw new InvalidOperationException(
                    "Não foi possível iniciar o processamento da importação.");
            }
        }
        public void Concluir(int id, int totalRegistros)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Id da importação deve ser maior que zero.");
            }

            if (totalRegistros < 0)
            {
                throw new ArgumentException(
                    "Total de registros não pode ser negativo.");
            }

            var concluido = _repository.Concluir(
                id,
                totalRegistros);

            if (!concluido)
            {
                throw new InvalidOperationException(
                    "Não foi possível concluir a importação.");
            }
        }
        public void RegistrarErro(int id, string mensagemErro)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Id da importação deve ser maior que zero.");
            }

            if (string.IsNullOrWhiteSpace(mensagemErro))
            {
                throw new ArgumentException(
                    "Mensagem de erro é obrigatória.");
            }

            if (mensagemErro.Length > 1000)
            {
                throw new ArgumentException(
                    "Mensagem de erro não pode ultrapassar 1000 caracteres.");
            }

            var registrado = _repository.RegistrarErro(
                id,
                mensagemErro);

            if (!registrado)
            {
                throw new InvalidOperationException(
                    "Não foi possível registrar erro na importação.");
            }
        }
    }
}