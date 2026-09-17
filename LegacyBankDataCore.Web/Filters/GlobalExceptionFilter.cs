using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace LegacyBankDataCore.Web.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(
            HttpActionExecutedContext context)
        {
            Trace.TraceError(
                context.Exception.ToString());

            context.Response =
                context.Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    new
                    {
                        Message =
                            "Ocorreu um erro interno ao processar a solicitação."
                    });
        }
    }
}