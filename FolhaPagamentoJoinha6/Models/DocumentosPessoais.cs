namespace FolhaPagamentoJoinha6.Models
{
    public class DocumentosPessoais
    {
        public int idUsuario { get; set; }
        public string RG { get; set; }
        public int CPF { get; set; }
        public int PIS { get; set; }

        public List<AtestadoMedico> listaAtestadoMedico { get; set; }
        public List<Certificacoes> listaCertificacoes { get; set; }
    }
}
