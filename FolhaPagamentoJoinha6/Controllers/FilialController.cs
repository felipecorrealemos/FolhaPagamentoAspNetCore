﻿using FolhaPagamentoJoinha6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FolhaPagamentoJoinha6.Controllers
{
    public class FilialController : Controller
    {
        // GET: FilialController
        public ActionResult Index(int? idEmpresa)
        {
            int? empresaMae = TempData["idEmpresa"] as int?;

            if (!empresaMae.HasValue)
            {
                empresaMae = idEmpresa;
            }

            List<EmpresaCliente> listaFilial = EmpresaCliente.GetFiliais((int)empresaMae);
            EmpresaCliente empresa = EmpresaCliente.GetEmpresa(empresaMae);

            ViewData["empresaMae"] = empresa;
            ViewData["ListaFilial"] = listaFilial;


            return View();
        }

        // GET: FilialController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FilialController/Create
        public ActionResult Create(int? empresaMae)
        {
            EmpresaCliente empresaCliente = EmpresaCliente.GetEmpresa(empresaMae);

            if (empresaCliente != null)
            {
                ViewData["empresaMae"] = empresaCliente;
            }

            return View();
        }

        public ActionResult Salvar(IFormCollection collection)
        {
            try
            {
                if (EmpresaCliente.CriarEmpresaEEndereco(collection, out string? mensagemErro))
                {
                    TempData["SuccessMessage"] = "Registro cadastrado com sucesso.";
                    TempData["idEmpresa"] = collection["idEmpresa"];
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

            return RedirectToAction(nameof(Index), new { idEmpresa = collection["idEmpresa"] });
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
