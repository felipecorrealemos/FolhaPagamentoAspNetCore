using FolhaPagamentoJoinha6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FolhaPagamentoJoinha6.Controllers
{
    public class CargoController : Controller
    {
        // GET: CargoController
        public ActionResult Index(int? departamentoId)
        {
            int? RetornoDep = TempData["departamentoId"] as int?;

            if (!RetornoDep.HasValue)
            {
                RetornoDep = departamentoId;
            }

            //Verificar o retorno do botao back to list da pagina create, esta retornando null.

            try
            {
                List<Cargo> listaCargo = Cargo.GetListaCargo((int)RetornoDep);
                Departamento departamento = Departamento.GetDepartamento((int)RetornoDep);

                ViewData["ListaCargo"] = listaCargo;
                ViewData["Departamento"] = departamento;
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return View();
        }

        // GET: CargoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CargoController/Create
        public ActionResult Create(int? departamentoId)
        {
            if (departamentoId.HasValue)
            {
                Departamento departamento = Departamento.GetDepartamento((int)departamentoId);

                if (departamento != null)
                {
                    ViewData["departamento"] = departamento;
                }
            }
            return View();
        }

        public ActionResult Salvar(IFormCollection collection)
        {
            try
            {
                if (Cargo.CriarCargo(collection, out string? mensagemErro))
                {
                    TempData["SuccessMessage"] = "Registro cadastrado com sucesso.";
                    TempData["departamentoId"] = collection["departamentoId"];
                }

                else
                {
                    TempData["ErrorMessage"] = $"Erro, {mensagemErro}.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return RedirectToAction(nameof(Index), new { departamentoId = collection["departamentoId"] });
        }

        // POST: CargoController/Create
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

        // GET: CargoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CargoController/Edit/5
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

        // GET: CargoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CargoController/Delete/5
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
