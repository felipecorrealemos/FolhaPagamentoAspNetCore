using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class Endereco
    {
        public int numero { get; set; }
        public string rua { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
    }
}