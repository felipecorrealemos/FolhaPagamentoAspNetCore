﻿using FolhaPagamentoJoinha6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FolhaPagamentoJoinha6.Controllers
{
    public class DepartamentoController : Controller
    {
        // GET: DepartamentoController
        public ActionResult Index(int? empresaId)
        {
            int? empresaMae = TempData["empresaId"] as int?;

            if (!empresaMae.HasValue)
            {
                empresaMae = empresaId;
            }

            try
            {
                List<Departamento> listaDepartamento = Departamento.GetListaDepartamento((int)empresaMae);
                EmpresaCliente empresa = EmpresaCliente.GetEmpresa(empresaMae);

                ViewData["empresaMae"] = empresa;
                ViewData["ListaDepartamento"] = listaDepartamento;
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return View();
        }

        // GET: DepartamentoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DepartamentoController/Create
        public ActionResult Create(int? empresaId)
        {
            if (empresaId.HasValue)
            {
                EmpresaCliente empresa = EmpresaCliente.GetEmpresa(empresaId);

                if (empresa != null)
                {
                    ViewData["empresaMae"] = empresa;
                }
            }
            return View();
        }

        public ActionResult Salvar(IFormCollection collection)
        {
            try
            {
                if (Departamento.CriarDepartamento(collection, out string? mensagemErro))
                {
                    TempData["SuccessMessage"] = "Registro cadastrado com sucesso.";
                    TempData["empresaId"] = collection["empresaId"];
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

            return RedirectToAction(nameof(Index), new { empresaId = collection["empresaId"] });
        }

        // POST: DepartamentoController/Create
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

        // GET: DepartamentoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DepartamentoController/Edit/5
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

        // GET: DepartamentoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DepartamentoController/Delete/5
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

        public IActionResult VerCargo(int departamentoId)
        {
            try
            {
                //TempData["idDepartamento"] = idDepartamento;
                return RedirectToAction("Index", "Cargo", new { departamentoId });
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
