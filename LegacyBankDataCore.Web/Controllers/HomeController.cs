using System;
using System.Web.Mvc;
using LegacyBankDataCore.Web.Models;

namespace LegacyBankDataCore.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var movimento = new MovimentoViewModel
            {
                IdExterno = "ABC123",
                Conta = "12345",
                Tipo = "ENTRADA",
                Valor = 1500.00m,
                Data = new DateTime(2026, 9, 14)
            };

            return View(movimento);
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Novo(MovimentoViewModel movimento)
        {
            return Content(
                "Id Externo: " + movimento.IdExterno +
                " | Conta: " + movimento.Conta +
                " | Tipo: " + movimento.Tipo
            );
        }
    }
}