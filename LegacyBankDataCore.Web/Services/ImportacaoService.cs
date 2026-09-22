using LegacyBankDataCore.Web.Models;
using LegacyBankDataCore.Web.Repositories;
using System;
using System.Collections.Generic;

namespace LegacyBankDataCore.Web.Services
{
    public class ImportacaoService
    {
        private readonly ImportacaoRepository _repository;

        public ImportacaoService(ImportacaoRepository repository)
        {
            _repository = repository;
        }

        public int Criar(string nomeArquivo, string hashArquivo)
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

            if (string.IsNullOrWhiteSpace(hashArquivo))
            {
                throw new ArgumentException(
                    "Hash do arquivo é obrigatório.");
            }

            if (hashArquivo.Length != 64)
            {
                throw new ArgumentException(
                    "Hash do arquivo deve possuir 64 caracteres.");
            }

            return _repository.Criar(
                nomeArquivo,
                hashArquivo);
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
        public Importacao BuscarPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Id da importação deve ser maior que zero.");
            }

            return _repository.BuscarPorId(id);
        }
    public void Reprocessar(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Id da importação deve ser maior que zero.");
            }

            var importacao =
                _repository.BuscarPorId(id);

            if (importacao == null)
            {
                throw new InvalidOperationException(
                    "Importação não encontrada.");
            }

            if (importacao.Status != "ERRO")
            {
                throw new InvalidOperationException(
                    "Somente importações com status ERRO podem ser reprocessadas.");
            }

            var reprocessado =
                _repository.Reprocessar(id);

            if (!reprocessado)
            {
                throw new InvalidOperationException(
                    "Não foi possível iniciar o reprocessamento da importação.");
            }
        }
        public List<Importacao> Listar()
        {
            return _repository.Listar();
        }

        public ImportacaoPaginadoResult ListarPaginado(
            string termo,
            string status,
            int pagina,
            int tamanhoPagina)
                {
            if (pagina < 1)
            {
                throw new ArgumentException(
                    "Página deve ser maior ou igual a 1.");
            }

            if (tamanhoPagina < 1 || tamanhoPagina > 100)
            {
                throw new ArgumentException(
                    "Tamanho da página deve estar entre 1 e 100.");
            }

            if (!string.IsNullOrWhiteSpace(termo))
            {
                termo = termo.Trim();

                if (termo.Length > 255)
                {
                    throw new ArgumentException(
                        "Termo de pesquisa deve possuir no máximo 255 caracteres.");
                }
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                status = status.Trim().ToUpperInvariant();

                if (status == "TODOS")
                {
                    status = null;
                }
                else if (
                    status != "RECEBIDA" &&
                    status != "PROCESSANDO" &&
                    status != "CONCLUIDA" &&
                    status != "ERRO")
                {
                    throw new ArgumentException(
                        "Status inválido.");
                }
            }

            return _repository.ListarPaginado(
                termo,
                status,
                pagina,
                tamanhoPagina);
        }
    }
        
}