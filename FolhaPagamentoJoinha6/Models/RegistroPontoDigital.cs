using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class RegistroPontoDigital
    {
        public int idMarcador { get; set; }
        public int idFuncionario { get; set; }
        public bool entradaSaida { get; set; }
        public DateTime dataHora { get; set; }
        public int tipoRegistro { get; set; }
    }
}