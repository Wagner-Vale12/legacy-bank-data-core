using System;
using System.Web.Http;
using System.Net;
using LegacyBankDataCore.Web.Models;
using LegacyBankDataCore.Web.Repositories;
using LegacyBankDataCore.Web.Services;

namespace LegacyBankDataCore.Web.Controllers
{
    public class ImportacoesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(CriarImportacaoRequest request)
        {
            if (request == null)
            {
                return BadRequest("Dados da importação são obrigatórios.");
            }

            var repository = new ImportacaoRepository();
            var service = new ImportacaoService(repository);

            try
            {
                var id = service.Criar(request.NomeArquivo);

                return Ok(new
                {
                    Id = id,
                    Status = "RECEBIDA",
                    Mensagem = "Importação registrada com sucesso."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("api/importacoes/{id:int}/iniciar")]
        public IHttpActionResult Iniciar(int id)
        {
            var repository = new ImportacaoRepository();
            var service = new ImportacaoService(repository);

            try
            {
                service.IniciarProcessamento(id);

                return Ok(new
                {
                    Id = id,
                    Status = "PROCESSANDO",
                    Mensagem = "Processamento iniciado com sucesso."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Content(
                    HttpStatusCode.Conflict,
                    new
                    {
                        Message = ex.Message
                    }
                );
            }
        }
        [HttpPut]
        [Route("api/importacoes/{id:int}/concluir")]
        public IHttpActionResult Concluir(int id, int totalRegistros)
        {
            var repository = new ImportacaoRepository();
            var service = new ImportacaoService(repository);

            try
            {
                service.Concluir(id, totalRegistros);

                return Ok(new
                {
                    Id = id,
                    Status = "CONCLUIDA",
                    TotalRegistros = totalRegistros,
                    Mensagem = "Importação concluída com sucesso."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Content(
                    HttpStatusCode.Conflict,
                    new
                    {
                        Message = ex.Message
                    }
                );
            }
        }
        [HttpPut]
        [Route("api/importacoes/{id:int}/erro")]
        public IHttpActionResult RegistrarErro(
    int id,
    RegistrarErroImportacaoRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                    "Dados do erro são obrigatórios.");
            }

            var repository = new ImportacaoRepository();
            var service = new ImportacaoService(repository);

            try
            {
                service.RegistrarErro(
                    id,
                    request.MensagemErro);

                return Ok(new
                {
                    Id = id,
                    Status = "ERRO",
                    MensagemErro = request.MensagemErro,
                    Mensagem = "Erro registrado na importação com sucesso."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Content(
                    HttpStatusCode.Conflict,
                    new
                    {
                        Message = ex.Message
                    }
                );
            }
        }
    }
}