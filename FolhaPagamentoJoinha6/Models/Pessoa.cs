namespace FolhaPagamentoJoinha6.Models
{
    public class Pessoa
    {
        public int idPessoa { get; set; }
        public string nomePessoa { get; set; }
        public string nomeSocial { get; set; }
        public Endereco endereco { get; set; }
        public DocumentosPessoais documentosPessoais { get; set; }
        public bool PCD { get; set; }
        public DateTime dataNascimento { get; set; }
        public int escolaridade { get; set; }
        public int grupoEtnico { get; set; }

    }
}
