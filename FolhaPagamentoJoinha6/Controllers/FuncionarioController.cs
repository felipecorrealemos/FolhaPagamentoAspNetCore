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
            /* List<EmpresaCliente> listaEmpresaCliente = new List<EmpresaCliente>();

             EmpresaCliente empresaCliente = new EmpresaCliente();
             empresaCliente.idEmpresa = 1;
             empresaCliente.RazaoSocial = "nomeTesteRazaoSocial1";
             empresaCliente.cnpjBase = "1231231231/0001";

             Filial filial = new Filial();
             filial.idFilial = 1;
             filial.cnpjFilial = "321231231/0001";

             empresaCliente.listaFilial = new List<Filial>();
             empresaCliente.listaFilial.Add(filial);
             listaEmpresaCliente.Add(empresaCliente);

             return View(listaEmpresaCliente);*/
            Funcionario funcionario = new Funcionario();
            funcionario.idPessoa = 1;
            funcionario.nomePessoa = "Pessoa1";
            funcionario.tipoContrato = "CLT";
            funcionario.cargo = "cargo1";
            funcionario.idFilial = 1;
            funcionario.idDepartamento = 1;

            List<Funcionario> listaFuncionario = new List<Funcionario>();
            listaFuncionario.Add(funcionario);

            return View(listaFuncionario);
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
