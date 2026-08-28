using System.Web.Http;

namespace LegacyBankDataCore.Web.Controllers
{
    public class MovimentosController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var movimento = new
            {
                IdExterno = "API001",
                Conta = "12345",
                Tipo = "ENTRADA",
                Valor = 2500.00m
            };

            return Ok(movimento);
        }
    }
}