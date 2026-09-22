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
                    });
            }
        }

        [HttpPut]
        [Route("api/importacoes/{id:int}/concluir")]
        public IHttpActionResult Concluir(
            int id,
            int totalRegistros)
        {
            var repository = new ImportacaoRepository();
            var service = new ImportacaoService(repository);

            try
            {
                service.Concluir(
                    id,
                    totalRegistros);

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
                    });
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

            var repository =
                new ImportacaoRepository();

            var service =
                new ImportacaoService(repository);

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
                    Mensagem =
                        "Erro registrado na importação com sucesso."
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
                    });
            }
        }

        [HttpPost]
        [Route("api/importacoes/processar")]
        public IHttpActionResult Processar(
            ProcessarImportacaoRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(
                    request.NomeArquivo))
            {
                return BadRequest(
                    "Nome do arquivo é obrigatório.");
            }

            var nomeArquivo =
                Path.GetFileName(
                    request.NomeArquivo);

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

            var processamentoRepository =
                new ProcessamentoImportacaoRepository();

            var hashArquivoService =
                new HashArquivoService();

            var xmlSchemaValidator =
                new XmlSchemaValidator();

            var processarService =
                new ProcessarImportacaoService(
                    importacaoService,
                    movimentoService,
                    xmlReader,
                    processamentoRepository,
                    hashArquivoService,
                    xmlSchemaValidator);

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
            catch (ImportacaoDuplicadaException ex)
            {
                return Content(
                    HttpStatusCode.Conflict,
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

        [HttpPut]
        [Route("api/importacoes/{id:int}/reprocessar")]
        public IHttpActionResult Reprocessar(int id)
        {
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

            var processamentoRepository =
                new ProcessamentoImportacaoRepository();

            var hashArquivoService =
                new HashArquivoService();

            var xmlSchemaValidator =
                new XmlSchemaValidator();

            try
            {
                var importacao =
                    importacaoService.BuscarPorId(id);

                if (importacao == null)
                {
                    return NotFound();
                }

                var pastaImportacoes =
                    HostingEnvironment.MapPath(
                        "~/App_Data/Importacoes");

                var caminhoArquivo =
                    Path.Combine(
                        pastaImportacoes,
                        importacao.NomeArquivo);

                var processarService =
                    new ProcessarImportacaoService(
                        importacaoService,
                        movimentoService,
                        xmlReader,
                        processamentoRepository,
                        hashArquivoService,
                        xmlSchemaValidator);

                var importacaoId =
                    processarService.Reprocessar(
                        id,
                        caminhoArquivo);

                return Ok(new
                {
                    Id = importacaoId,
                    Status = "CONCLUIDA",
                    Mensagem =
                        "Importação reprocessada com sucesso."
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
            catch (InvalidOperationException ex)
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

        [HttpGet]
        [Route("api/importacoes/{id:int}")]
        public IHttpActionResult BuscarPorId(int id)
        {
            var repository =
                new ImportacaoRepository();

            var service =
                new ImportacaoService(
                    repository);

            try
            {
                var importacao =
                    service.BuscarPorId(id);

                if (importacao == null)
                {
                    return NotFound();
                }

                return Ok(importacao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/importacoes")]
        public IHttpActionResult Listar()
        {
            var repository =
                new ImportacaoRepository();

            var service =
                new ImportacaoService(
                    repository);

            var importacoes =
                service.Listar();

            return Ok(importacoes);
        }
        [HttpGet]
        [Route("api/importacoes/paginado")]
        public IHttpActionResult ListarPaginado(
            string termo = null,
            string status = null,
            int pagina = 1,
            int tamanhoPagina = 10)
                {
            var repository =
                new ImportacaoRepository();

            var service =
                new ImportacaoService(repository);

            try
            {
                var resultado =
                    service.ListarPaginado(
                        termo,
                        status,
                        pagina,
                        tamanhoPagina);

                var totalPaginas =
                    (int)Math.Ceiling(
                        resultado.TotalRegistros /
                        (double)tamanhoPagina);

                return Ok(new
                {
                    Itens = resultado.Itens,
                    TotalRegistros =
                        resultado.TotalRegistros,
                    Pagina = pagina,
                    TamanhoPagina = tamanhoPagina,
                    TotalPaginas = totalPaginas
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}