using LegacyBankDataCore.Web.Exceptions;
using LegacyBankDataCore.Web.Models;
using System;
using System.Collections.Generic;
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
        public Importacao BuscarPorId(int id)
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_IMPORTACAO_BUSCAR_POR_ID",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters
                        .Add("@Id", SqlDbType.Int)
                        .Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        return new Importacao
                        {
                            Id = reader.GetInt32(
                                reader.GetOrdinal("Id")),

                            NomeArquivo =
                                reader["NomeArquivo"].ToString(),

                            Status =
                                reader["Status"].ToString(),

                            TotalRegistros =
                                reader.GetInt32(
                                    reader.GetOrdinal("TotalRegistros")),

                            DataRecebimento =
                                reader.GetDateTime(
                                    reader.GetOrdinal("DataRecebimento")),

                            DataProcessamento =
                                reader["DataProcessamento"] == DBNull.Value
                                    ? (DateTime?)null
                                    : reader.GetDateTime(
                                        reader.GetOrdinal("DataProcessamento")),

                            MensagemErro =
                                reader["MensagemErro"] == DBNull.Value
                                    ? null
                                    : reader["MensagemErro"].ToString(),

                            HashArquivo =
                                reader["HashArquivo"] == DBNull.Value
                                    ? null
                                    : reader["HashArquivo"].ToString()
                        };
                    }
                }
            }
        }
        public bool Reprocessar(int id)
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_IMPORTACAO_REPROCESSAR",
                    connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;

                    command.Parameters
                        .Add("@Id", SqlDbType.Int)
                        .Value = id;

                    var resultado =
                        command.ExecuteScalar();

                    var linhasAfetadas =
                        Convert.ToInt32(resultado);

                    return linhasAfetadas == 1;
                }
            }
        }

        public List<Importacao> Listar()
        {
            var importacoes = new List<Importacao>();

            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_IMPORTACAO_LISTAR",
                    connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            importacoes.Add(new Importacao
                            {
                                Id = reader.GetInt32(
                                    reader.GetOrdinal("Id")),

                                NomeArquivo =
                                    reader["NomeArquivo"].ToString(),

                                Status =
                                    reader["Status"].ToString(),

                                TotalRegistros =
                                    reader.GetInt32(
                                        reader.GetOrdinal("TotalRegistros")),

                                DataRecebimento =
                                    reader.GetDateTime(
                                        reader.GetOrdinal("DataRecebimento")),

                                DataProcessamento =
                                    reader["DataProcessamento"] == DBNull.Value
                                        ? (DateTime?)null
                                        : reader.GetDateTime(
                                            reader.GetOrdinal("DataProcessamento")),

                                MensagemErro =
                                    reader["MensagemErro"] == DBNull.Value
                                        ? null
                                        : reader["MensagemErro"].ToString(),

                                HashArquivo =
                                    reader["HashArquivo"] == DBNull.Value
                                        ? null
                                        : reader["HashArquivo"].ToString()
                            });
                        }
                    }
                }
            }

            return importacoes;
        }
        public ImportacaoPaginadoResult ListarPaginado(
    string termo,
    string status,
    int pagina,
    int tamanhoPagina)
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            var resultado =
                new ImportacaoPaginadoResult();

            using (var connection =
                new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_IMPORTACAO_LISTAR_PAGINADO",
                    connection))
                {
                    command.CommandType =
                        CommandType.StoredProcedure;

                    command.Parameters
                        .Add("@Termo", SqlDbType.VarChar, 255)
                        .Value =
                            string.IsNullOrWhiteSpace(termo)
                                ? (object)DBNull.Value
                                : termo;

                    command.Parameters
                        .Add("@Status", SqlDbType.VarChar, 20)
                        .Value =
                            string.IsNullOrWhiteSpace(status)
                                ? (object)DBNull.Value
                                : status;

                    command.Parameters
                        .Add("@Pagina", SqlDbType.Int)
                        .Value = pagina;

                    command.Parameters
                        .Add("@TamanhoPagina", SqlDbType.Int)
                        .Value = tamanhoPagina;

                    using (var reader =
                        command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Itens.Add(
                                new Importacao
                                {
                                    Id = reader.GetInt32(
                                        reader.GetOrdinal("Id")),

                                    NomeArquivo =
                                        reader["NomeArquivo"].ToString(),

                                    Status =
                                        reader["Status"].ToString(),

                                    TotalRegistros =
                                        reader.GetInt32(
                                            reader.GetOrdinal(
                                                "TotalRegistros")),

                                    DataRecebimento =
                                        reader.GetDateTime(
                                            reader.GetOrdinal(
                                                "DataRecebimento")),

                                    DataProcessamento =
                                        reader["DataProcessamento"] ==
                                        DBNull.Value
                                            ? (DateTime?)null
                                            : reader.GetDateTime(
                                                reader.GetOrdinal(
                                                    "DataProcessamento")),

                                    MensagemErro =
                                        reader["MensagemErro"] ==
                                        DBNull.Value
                                            ? null
                                            : reader["MensagemErro"]
                                                .ToString(),

                                    HashArquivo =
                                        reader["HashArquivo"] ==
                                        DBNull.Value
                                            ? null
                                            : reader["HashArquivo"]
                                                .ToString()
                                });
                        }

                        if (reader.NextResult() &&
                            reader.Read())
                        {
                            resultado.TotalRegistros =
                                reader.GetInt32(
                                    reader.GetOrdinal(
                                        "TotalRegistros"));
                        }
                    }
                }
            }

            return resultado;
        }
    }
}