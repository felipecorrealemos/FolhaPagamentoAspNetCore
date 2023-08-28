using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class AtestadoMedico
    {
        public int idAtestadoMedico { get; set; }
        public int idFuncionario { get; set; }
        public DateTime dataHora { get; set; }
        public int qtdDiasAfastado { get; set; }
        public string medico { get; set; }
    }
}