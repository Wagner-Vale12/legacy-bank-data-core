using System;
using System.Web.Http;
using LegacyBankDataCore.Web.Models;
using LegacyBankDataCore.Web.Repositories;
using LegacyBankDataCore.Web.Services;
using System.Net;
using LegacyBankDataCore.Web.Exceptions;

namespace LegacyBankDataCore.Web.Controllers
{
    public class MovimentosController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var repository = new MovimentoRepository();

            var service = new MovimentoService(repository);

            var movimentos = service.Listar();

            return Ok(movimentos);
        }

        [HttpGet]
        [Route("api/movimentos/paginado")]
        public IHttpActionResult ListarPaginado(
            string termo = null,
            string tipo = null,
            int pagina = 1,
            int tamanhoPagina = 10)
                {
            var repository =
                new MovimentoRepository();

            var service =
                new MovimentoService(repository);

            try
            {
                var resultado =
                    service.ListarPaginado(
                        termo,
                        tipo,
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var repository = new MovimentoRepository();

            var service = new MovimentoService(repository);

            var movimento = service.BuscarPorId(id);

            if (movimento == null)
            {
                return NotFound();
            }

            return Ok(movimento);
        }
        [HttpPost]
        public IHttpActionResult Post(CriarMovimentoRequest request)
        {
            var repository = new MovimentoRepository();
            var service = new MovimentoService(repository);

            try
            {
                var id = service.Criar(request);

                return Ok(new
                {
                    Id = id,
                    Mensagem = "Movimento cadastrado com sucesso."
                });
            }
            catch (MovimentoDuplicadoException ex)
            {
                return Content(
                    HttpStatusCode.Conflict,
                    new
                    {
                        Message = ex.Message
                    }
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}