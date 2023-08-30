using FolhaPagamentoJoinha6.Models;
using Microsoft.AspNetCore.Mvc;

namespace FolhaPagamentoJoinha6.Controllers
{
    public class EmpresaController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                List<EmpresaCliente> listaEmpresas = new EmpresaCliente().GetListaEmpresas();
                ViewData["ListaEmpresa"] = listaEmpresas;
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return View();
        }

        // GET: EmpresaController/Create
        public ActionResult Create(int? idEmpresa)
        {
            if (idEmpresa.HasValue)
            {
                Filial empresaEFilial1 = EmpresaCliente.GetEmpresaEFilial1((int)idEmpresa);

                if (empresaEFilial1 != null)
                {
                    ViewData["Filial"] = empresaEFilial1;
                }
            }

            return View();
        }

        // POST: EmpresaController/Create
        public ActionResult Salvar(IFormCollection collection)
        {
            Conexao conexao = new Conexao();

            try
            {
                //validacao CNPJ
                if (EmpresaCliente.VerificaCNPJExiste(collection))
                {
                    ModelState.AddModelError("cnpjBase", "CNPJ já cadastrado.");
                    Filial filial = new Filial().CarregaObjetoFilial1(collection);

                    ViewData["Filial"] = filial;
                    return View("Create");
                }

                if (!string.IsNullOrEmpty(collection["idEmpresa"]))
                {
                    EmpresaCliente.AlterarEmpresa(collection);
                    Filial.AlterarFilial1(collection);
                    TempData["SuccessMessage"] = "Registro alterado com sucesso.";
                }

                else
                {
                    if (EmpresaCliente.CriarEmpresaEFilial1(collection, out string? mensagemErro))
                    {
                        TempData["SuccessMessage"] = "Registro cadastrado com sucesso.";
                    }

                    else
                    {
                        TempData["ErrorMessage"] = $"Erro, {mensagemErro}.";
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult VerFilial(int idEmpresa)
        {
            try
            {
                TempData["idEmpresa"] = idEmpresa;
                return RedirectToAction("Index", "Filial");
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
