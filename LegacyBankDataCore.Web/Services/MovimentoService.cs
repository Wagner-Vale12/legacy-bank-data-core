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

            return _repository.InserirLote(movimentos);
        }
    }
}