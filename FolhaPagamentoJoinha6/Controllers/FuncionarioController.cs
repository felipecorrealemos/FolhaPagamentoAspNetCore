using FolhaPagamentoJoinha6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FolhaPagamentoJoinha6.Controllers
{
    public class FuncionarioController : Controller
    {
        // GET: FuncionarioController
        public ActionResult Index()
        {
           /*
            List<Funcionario> listaFuncionario = new List<Funcionario>();
            listaFuncionario.Add(funcionario);

            return View(listaFuncionario);*/
           return View();
        }

        public ActionResult GerarPontoMensal(int? qtdHorasSem, int? qtdDiasTrab)
        {
            try
            {
                //TimeSpan timeSpan = RegistroPonto.GerarRegistroPontoMesInteiroAutomatico(40, 20);
                List<RegistroPontoDiario> listaPonto = RegistroPonto.GerarListaRegistroPontoMesInteiroAutomatico(40, 20, out string horasExtras);

                ViewData["horasExtras"] = horasExtras;
                ViewData["listaRegistroPontoDiario"] = listaPonto;
                //  TempData["SuccessMessage"] = "Registro calculado com sucesso.";
            }

            catch (Exception ex)
            {
                // TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return View("RegistroPontoAutomatico");
        }

        public ActionResult RegistroPontoAutomatico()
        {
            return View();
        }

        // GET: FuncionarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FuncionarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FuncionarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FuncionarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FuncionarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FuncionarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FuncionarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
