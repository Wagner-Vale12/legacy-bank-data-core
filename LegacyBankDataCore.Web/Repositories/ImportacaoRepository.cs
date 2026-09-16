using LegacyBankDataCore.Web.Exceptions;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LegacyBankDataCore.Web.Repositories
{
    public class ImportacaoRepository
    {
        public int Criar(string nomeArquivo, string hashArquivo)
        {
            var connectionString =
       ConfigurationManager
           .ConnectionStrings["LegacyBankDataCore"]
           .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_IMPORTACAO_CRIAR",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters
                        .Add("@NomeArquivo", SqlDbType.VarChar, 255)
                        .Value = nomeArquivo;

                    command.Parameters
                        .Add("@HashArquivo", SqlDbType.Char, 64)
                        .Value = hashArquivo;

                    try
                    {
                        var resultado = command.ExecuteScalar();

                        return Convert.ToInt32(resultado);
                    }
                    catch (SqlException ex)
                        when (ex.Number == 2601 || ex.Number == 2627)
                    {
                        throw new ImportacaoDuplicadaException();
                    }
                }
            }
        }
        public bool IniciarProcessamento(int id)
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_IMPORTACAO_INICIAR_PROCESSAMENTO",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters
                        .Add("@Id", SqlDbType.Int)
                        .Value = id;

                    var resultado = command.ExecuteScalar();

                    var linhasAfetadas = Convert.ToInt32(resultado);

                    return linhasAfetadas == 1;
                }
            }
        }
        public bool Concluir(int id, int totalRegistros)
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_IMPORTACAO_CONCLUIR",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters
                        .Add("@Id", SqlDbType.Int)
                        .Value = id;

                    command.Parameters
                        .Add("@TotalRegistros", SqlDbType.Int)
                        .Value = totalRegistros;

                    var resultado = command.ExecuteScalar();

                    var linhasAfetadas = Convert.ToInt32(resultado);

                    return linhasAfetadas == 1;
                }
            }
        }
        public bool RegistrarErro(int id, string mensagemErro)
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_IMPORTACAO_REGISTRAR_ERRO",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters
                        .Add("@Id", SqlDbType.Int)
                        .Value = id;

                    command.Parameters
                        .Add("@MensagemErro", SqlDbType.VarChar, 1000)
                        .Value = mensagemErro;

                    var resultado = command.ExecuteScalar();

                    var linhasAfetadas = Convert.ToInt32(resultado);

                    return linhasAfetadas == 1;
                }
            }
        }
    }
}