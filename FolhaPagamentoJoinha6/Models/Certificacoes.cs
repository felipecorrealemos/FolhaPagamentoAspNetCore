using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class Certificacoes
    {
        public int idCertificacao { get; set; }
        public string nomeInstituicao { get; set; }
        public string nomeCurso { get; set; }
        public DateTime dataInicioCurso { get; set; }
        public DateTime? dataFinalCurso { get; set; }
        public string tempoCurso { get; set; }
    }
}