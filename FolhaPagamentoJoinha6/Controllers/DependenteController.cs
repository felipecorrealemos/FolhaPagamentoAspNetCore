using FolhaPagamentoJoinha6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FolhaPagamentoJoinha6.Controllers
{
    public class DependenteController : Controller
    {
        // GET: DependenteController
        public ActionResult Index(int funcionarioId)
        {
            try
            {
                List<Dependente> listaDependente = Dependente.GetListaDependente(funcionarioId);
                ViewData["ListaDependente"] = listaDependente;
                ViewData["funcionarioId"] = funcionarioId;
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return View();
        }

        // GET: DependenteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DependenteController/Create
        public ActionResult Create(int funcionarioId)
        {
            ViewData["funcionarioId"] = funcionarioId;
            return View();
        }

        public ActionResult Salvar(IFormCollection collection)
        {
            try
            {
                if (Dependente.CriarDependente(collection, out string mensagemErro))
                {
                    TempData["SuccessMessage"] = "Registro cadastrado com sucesso.";
                    int funcionarioId = Convert.ToInt32(collection["funcionarioId"]);

                    return RedirectToAction(nameof(Index), new { funcionarioId });
                }

                else
                {
                    TempData["ErrorMessage"] = $"Erro {mensagemErro}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: DependenteController/Create
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

        // GET: DependenteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DependenteController/Edit/5
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

        // GET: DependenteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DependenteController/Delete/5
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
