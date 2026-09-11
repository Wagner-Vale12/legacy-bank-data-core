using LegacyBankDataCore.Web.Exceptions;
using LegacyBankDataCore.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LegacyBankDataCore.Web.Repositories
{
    public class MovimentoRepository
    {
        public List<Movimento> Listar()
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            var movimentos = new List<Movimento>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_MOVIMENTO_LISTAR",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var movimento = new Movimento
                            {
                                Id = reader.GetInt32(
                                    reader.GetOrdinal("Id")),

                                IdExterno =
                                    reader["IdExterno"].ToString(),

                                Conta =
                                    reader["Conta"].ToString(),

                                Tipo =
                                    reader["Tipo"].ToString(),

                                Valor = reader.GetDecimal(
                                    reader.GetOrdinal("Valor")),

                                DataMovimento = reader.GetDateTime(
                                    reader.GetOrdinal("DataMovimento")),

                                CriadoEm = reader.GetDateTime(
                                    reader.GetOrdinal("CriadoEm"))
                            };

                            movimentos.Add(movimento);
                        }
                    }
                }
            }

            return movimentos;
        }

        public Movimento BuscarPorId(int id)
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_MOVIMENTO_BUSCAR_POR_ID",
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

                        return new Movimento
                        {
                            Id = reader.GetInt32(
                                reader.GetOrdinal("Id")),

                            IdExterno =
                                reader["IdExterno"].ToString(),

                            Conta =
                                reader["Conta"].ToString(),

                            Tipo =
                                reader["Tipo"].ToString(),

                            Valor = reader.GetDecimal(
                                reader.GetOrdinal("Valor")),

                            DataMovimento = reader.GetDateTime(
                                reader.GetOrdinal("DataMovimento")),

                            CriadoEm = reader.GetDateTime(
                                reader.GetOrdinal("CriadoEm"))
                        };
                    }
                }
            }
        }
        public int Inserir(CriarMovimentoRequest request)
        {
            var connectionString =
                ConfigurationManager
                    .ConnectionStrings["LegacyBankDataCore"]
                    .ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "dbo.SP_MOVIMENTO_INSERIR",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

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
                        var resultado = command.ExecuteScalar();

                        return Convert.ToInt32(resultado);
                    }
                    catch (SqlException ex)
                        when (ex.Number == 2627 || ex.Number == 2601)
                    {
                        throw new MovimentoDuplicadoException();
                    }
                }
            }
        }
    }
}



