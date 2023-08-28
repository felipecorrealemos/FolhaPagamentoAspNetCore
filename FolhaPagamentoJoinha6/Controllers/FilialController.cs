using FolhaPagamentoJoinha6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FolhaPagamentoJoinha6.Controllers
{
    public class FilialController : Controller
    {
        // GET: FilialController
        public ActionResult Index()
        {
            int? idEmpresa = TempData["idEmpresa"] as int?;

            if (idEmpresa.HasValue)
            {
                List<Filial> listaFilial = new Filial().GetListaFilial((int)idEmpresa);

                ViewData["ListaFilial"] = listaFilial;
            }

            return View();
        }

        // GET: FilialController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FilialController/Create
        public ActionResult Create(int idEmpresa)
        {
            Filial filial = new Filial() { idEmpresa = idEmpresa };
            EmpresaCliente empresaCliente = EmpresaCliente.GetEmpresa(idEmpresa);
            filial.razaoSocial = empresaCliente.razaoSocial;
            filial.cnpjBase = empresaCliente.cnpjBase;

            return View(filial);
        }

        // POST: FilialController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Filial.CriarFilial(collection, Convert.ToInt32(collection["idEmpresa"]));
                TempData["SuccessMessage"] = "Registro cadatrado com sucesso.";

            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }


            TempData["idEmpresa"] = Convert.ToInt32(collection["idEmpresa"]);
            return RedirectToAction(nameof(Index));
        }

        // GET: FilialController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FilialController/Edit/5
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

        // GET: FilialController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FilialController/Delete/5
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
