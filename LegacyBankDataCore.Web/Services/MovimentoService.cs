using LegacyBankDataCore.Web.Models;
using LegacyBankDataCore.Web.Repositories;
using System;
using System.Collections.Generic;

namespace LegacyBankDataCore.Web.Services
{
    public class MovimentoService
    {
        private readonly MovimentoRepository _repository;

        public MovimentoService(MovimentoRepository repository)
        {
            _repository = repository;
        }

        public List<Movimento> Listar()
        {
            return _repository.Listar();
        }
      public Movimento BuscarPorId(int id)
        {
            return _repository.BuscarPorId(id);
        }
        public int Criar(CriarMovimentoRequest request)
        {
            Validar(request);

            var id = _repository.Inserir(request);

            return id;
        }

        private void Validar(CriarMovimentoRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.IdExterno))
            {
                throw new ArgumentException(
                    "IdExterno é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(request.Conta))
            {
                throw new ArgumentException(
                    "Conta é obrigatória.");
            }

            if (request.Tipo != "ENTRADA" &&
                request.Tipo != "SAIDA")
            {
                throw new ArgumentException(
                    "Tipo deve ser ENTRADA ou SAIDA.");
            }

            if (request.Valor <= 0)
            {
                throw new ArgumentException(
                    "Valor deve ser maior que zero.");
            }
        }
        public int CriarLote(List<CriarMovimentoRequest> movimentos)
        {
            ValidarLote(movimentos);

            return _repository.InserirLote(movimentos);
        }
        public void ValidarLote(List<CriarMovimentoRequest> movimentos)
        {
            if (movimentos == null)
            {
                throw new ArgumentNullException(nameof(movimentos));
            }

            if (movimentos.Count == 0)
            {
                throw new ArgumentException(
                    "A lista de movimentos não pode estar vazia.");
            }

            foreach (var movimento in movimentos)
            {
                Validar(movimento);
            }
        }
        public MovimentoPaginadoResult ListarPaginado(
            string termo,
            string tipo,
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

                if (termo.Length > 50)
                {
                    throw new ArgumentException(
                        "Termo de pesquisa deve possuir no máximo 50 caracteres.");
                }
            }

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                tipo = tipo.Trim().ToUpperInvariant();

                if (tipo == "TODOS")
                {
                    tipo = null;
                }
                else if (
                    tipo != "ENTRADA" &&
                    tipo != "SAIDA")
                {
                    throw new ArgumentException(
                        "Tipo deve ser ENTRADA ou SAIDA.");
                }
            }

            return _repository.ListarPaginado(
                termo,
                tipo,
                pagina,
                tamanhoPagina);
        }
    }
}