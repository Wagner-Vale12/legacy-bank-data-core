using LegacyBankDataCore.Web.Exceptions;
using LegacyBankDataCore.Web.Models;
using LegacyBankDataCore.Web.Repositories;
using LegacyBankDataCore.Web.Services;
using System;
using System.IO;
using System.Net;
using System.Web.Hosting;
using System.Web.Http;

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
        [HttpPost]
        [Route("api/importacoes/processar")]
        public IHttpActionResult Processar(
    ProcessarImportacaoRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.NomeArquivo))
            {
                return BadRequest(
                    "Nome do arquivo é obrigatório.");
            }

            var nomeArquivo =
                Path.GetFileName(request.NomeArquivo);

            if (nomeArquivo != request.NomeArquivo)
            {
                return BadRequest(
                    "Nome do arquivo inválido.");
            }

            var pastaImportacoes =
                HostingEnvironment.MapPath(
                    "~/App_Data/Importacoes");

            var caminhoArquivo =
                Path.Combine(
                    pastaImportacoes,
                    nomeArquivo);

            var importacaoRepository =
                new ImportacaoRepository();

            var movimentoRepository =
                new MovimentoRepository();

            var importacaoService =
                new ImportacaoService(
                    importacaoRepository);

            var movimentoService =
                new MovimentoService(
                    movimentoRepository);

            var xmlReader =
                new XmlMovimentoReader();

            var processarService =
                new ProcessarImportacaoService(
                    importacaoService,
                    movimentoService,
                    xmlReader);

            try
            {
                var importacaoId =
                    processarService.Processar(
                        caminhoArquivo);

                return Ok(new
                {
                    Id = importacaoId,
                    Status = "CONCLUIDA",
                    Mensagem =
                        "Arquivo processado com sucesso."
                });
            }
            catch (FileNotFoundException ex)
            {
                return Content(
                    HttpStatusCode.NotFound,
                    new
                    {
                        Message = ex.Message
                    });
            }
            catch (MovimentoDuplicadoException ex)
            {
                return Content(
                    HttpStatusCode.Conflict,
                    new
                    {
                        Message = ex.Message
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}