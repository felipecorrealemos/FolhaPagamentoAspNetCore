using FolhaPagamentoJoinha6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics;

namespace FolhaPagamentoJoinha6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Funcionario()
        {
            return RedirectToAction("Index", "FuncionarioController");
        }

        public IActionResult Index(int? empresaId)
        {

            if (empresaId == null)
            {
                empresaId = 3016;
            }

            List<Departamento> listaDepartamento = Departamento.GetListaDepartamento((int)empresaId);
            ViewData["ListaDepartamentos"] = listaDepartamento;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}