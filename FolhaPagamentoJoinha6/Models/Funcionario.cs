using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class Funcionario : Pessoa
    {
        // public int idFuncionario { get; set; }
        public string cargo { get; set; }
        public float promocoes { get; set; }
        public bool optanteVT { get; set; }
        public int idFilial { get; set; }
        public int idDepartamento { get; set; }
        public string tipoContrato { get; set; }
        public int jornadaTrabalho { get; set; }
        public bool pagaPensao { get; set; }

        public DadosPensao dadosPensao { get; set; }

        public List<AtestadoMedico> listaAtestadoMedico { get; set; }
        public List<Certificacoes> listaCertificacoes { get; set; }


        public static List<AtestadoMedico> GetAtestadosMedicos(int idFuncionario)
        {
            Funcionario funcionario = new Funcionario();
            funcionario.listaAtestadoMedico = new List<AtestadoMedico>();//ClasseConexaoGET(ComandoSQL GetAtestadoMedico_idFuncionario);
            return funcionario.listaAtestadoMedico;
        }

        public static bool SetAtestadoMedico(int idFuncionario, List<AtestadoMedico> listaAtestadoMedico, out string mensagemRetorno)
        {
            Funcionario funcionario = new Funcionario();

            //validacao da lista
            if (funcionario.InsertListaAtestadoMedico(listaAtestadoMedico, out mensagemRetorno))
            {
                //retorno positivo OK - Mensagem sucesso
                return true;
            }

            else
            {
                // retorno negativo Erro - Mensagem erro
                return false;
            }
        }

        public static bool SetAtestadoMedico(int idFuncionario, AtestadoMedico AtestadoMedico, out string mensagemRetorno)
        {

            Funcionario funcionario = new Funcionario();
            List<AtestadoMedico> listaAtestadoMedico = new List<AtestadoMedico>();
            listaAtestadoMedico.Add(AtestadoMedico);

            //validacao da lista
            if (funcionario.InsertListaAtestadoMedico(listaAtestadoMedico, out mensagemRetorno))
            {
                //retorno positivo OK - Mensagem sucesso
                return true;
            }

            else
            {
                // retorno negativo Erro - Mensagem erro
                return false;
            }
        }


        private bool InsertListaAtestadoMedico(List<AtestadoMedico> listaAtestadoMedico, out string mensagemRetorno)
        {
            try
            {
                for (int i = 0; i < listaAtestadoMedico.Count; i++)
                {
                    //insert sql idFuncionario => funcionario.listaAtestadoMedico[i];
                }

                mensagemRetorno = "Registros OK";

                return true;
            }

            catch (Exception ex)
            {
                mensagemRetorno = $"Erro ao registrar tal: {ex.Message}";

                return false;
            }
        }

        public static void GetCertificacoes(int idFuncionario)
        {

        }

        public static void SetCertificacao(int idFuncionario, Certificacoes certificacao)
        {

        }

        public void PensaoDevida(bool value, int dependentes, float valor)
        {

        }
    }
}