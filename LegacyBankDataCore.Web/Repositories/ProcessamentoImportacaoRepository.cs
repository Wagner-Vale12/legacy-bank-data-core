using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using LegacyBankDataCore.Web.Exceptions;
using LegacyBankDataCore.Web.Models;

namespace LegacyBankDataCore.Web.Repositories
{
    public class ProcessamentoImportacaoRepository
    {
        public int PersistirEConcluir(
            int importacaoId,
            List<CriarMovimentoRequest> movimentos)
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            using (var connection =
                new SqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction =
                    connection.BeginTransaction())
                {
                    try
                    {
                        var totalInseridos = 0;

                        foreach (var movimento in movimentos)
                        {
                            InserirMovimento(
                                movimento,
                                connection,
                                transaction);

                            totalInseridos++;
                        }

                        ConcluirImportacao(
                            importacaoId,
                            totalInseridos,
                            connection,
                            transaction);

                        transaction.Commit();

                        return totalInseridos;
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }

        private void InserirMovimento(
            CriarMovimentoRequest request,
            SqlConnection connection,
            SqlTransaction transaction)
        {
            using (var command = new SqlCommand(
                "dbo.SP_MOVIMENTO_INSERIR",
                connection,
                transaction))
            {
                command.CommandType =
                    CommandType.StoredProcedure;

                command.Parameters
                    .Add("@IdExterno", SqlDbType.VarChar, 50)
                    .Value = request.IdExterno;

                command.Parameters
                    .Add("@Conta", SqlDbType.VarChar, 30)
                    .Value = request.Conta;

                command.Parameters
                    .Add("@Tipo", SqlDbType.VarChar, 10)
                    .Value = request.Tipo;

                var valorParameter =
                    command.Parameters.Add(
                        "@Valor",
                        SqlDbType.Decimal);

                valorParameter.Precision = 18;
                valorParameter.Scale = 2;
                valorParameter.Value = request.Valor;

                command.Parameters
                    .Add("@DataMovimento", SqlDbType.Date)
                    .Value = request.DataMovimento;

                try
                {
                    command.ExecuteScalar();
                }
                catch (SqlException ex)
                    when (ex.Number == 2627 ||
                          ex.Number == 2601)
                {
                    throw new MovimentoDuplicadoException();
                }
            }
        }

        private void ConcluirImportacao(
            int importacaoId,
            int totalRegistros,
            SqlConnection connection,
            SqlTransaction transaction)
        {
            using (var command = new SqlCommand(
                "dbo.SP_IMPORTACAO_CONCLUIR",
                connection,
                transaction))
            {
                command.CommandType =
                    CommandType.StoredProcedure;

                command.Parameters
                    .Add("@Id", SqlDbType.Int)
                    .Value = importacaoId;

                command.Parameters
                    .Add("@TotalRegistros", SqlDbType.Int)
                    .Value = totalRegistros;

                var resultado =
                    command.ExecuteScalar();

                var linhasAfetadas =
                    Convert.ToInt32(resultado);

                if (linhasAfetadas != 1)
                {
                    throw new InvalidOperationException(
                        "Não foi possível concluir a importação.");
                }
            }
        }
    }
}