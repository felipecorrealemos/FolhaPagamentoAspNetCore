using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class Departamento
    {
        public int idDepartamento { get; set; }
        public string nomeDepartamento { get; set; }
        public int idFilial { get; set; }
        public int idFuncionarioResponsavel { get; set; }
        public string emailDepartamento { get; set; }
        public string telefone { get; set; }
    }
}