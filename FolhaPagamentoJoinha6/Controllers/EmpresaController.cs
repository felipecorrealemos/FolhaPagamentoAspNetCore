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
                List<EmpresaCliente> listaEmpresas = EmpresaCliente.GetListaEmpresasMatriz();
                ViewData["ListaEmpresa"] = listaEmpresas;
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return View();
        }

        // GET: EmpresaController/Create
        public ActionResult Create(int? empresaId)
        {
            if (empresaId.HasValue)
            {
                EmpresaCliente empresa = EmpresaCliente.GetEmpresa(empresaId);
                Endereco endereco = Endereco.GetEndereco(empresa.endereco);

                if (empresa != null)
                {
                    ViewData["Empresa"] = empresa;
                    ViewData["Endereco"] = endereco;
                }
            }

            List<string> estados = Endereco.CarregaEstados();
            ViewData["Estados"] = estados;

            return View();
        }

        // POST: EmpresaController/Create
        public ActionResult Salvar(IFormCollection collection)
        {
            // Conexao conexao = new Conexao();

            try
            {
                //validacao CNPJ
                /* if (EmpresaCliente.VerificaCNPJExiste(collection))
                 {
                     ModelState.AddModelError("cnpjBase", "CNPJ já cadastrado.");
                     Filial filial = new Filial().CarregaObjetoFilial1(collection);

                     ViewData["Filial"] = filial;
                     return View("Create");
                 }
                */
                if (!string.IsNullOrEmpty(collection["empresaId"]))
                {
                    EmpresaCliente.AlteraEmpresaEEndereco(collection, out string? mensagemErro);
                    TempData["SuccessMessage"] = "Registro alterado com sucesso.";
                }

                else
                {
                    if (EmpresaCliente.CriarEmpresaEEndereco(collection, out string? mensagemErro))
                    {
                        TempData["SuccessMessage"] = "Registro cadastrado com sucesso.";
                    }

                   /* else
                    {
                        TempData["ErrorMessage"] = $"Erro, {mensagemErro}.";
                    }*/
                }
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult VerFilial(int empresaId)
        {
            try
            {
                //TempData["empresaId"] = empresaId;
                return RedirectToAction("Index", "Filial", new { empresaId });
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult VerDepartamento(int empresaId)
        {
            try
            {
                //TempData["idEmpresa"] = idEmpresa;
                return RedirectToAction("Index", "Departamento", new { empresaId });
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro, {ex.Message}.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
