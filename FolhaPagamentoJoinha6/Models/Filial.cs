using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class Filial : EmpresaCliente
    {
        public int idFilial { get; set; }
        
        [Required(ErrorMessage = mensagemValidacao)]
        public string cnpjFilial { get; set; }
        public string? email { get; set; }
        public Endereco endereco { get; set; }
        [Required(ErrorMessage = mensagemValidacao)]
        public string? nomeFantasia { get; set; }
        public List<Departamento> listaDepartamentos { get; set; }
        public string? observacoes { get; set; }
        public float rat { get; set; }
        public float fap { get; set; }

        private const string mensagemValidacao = "Preenchimento Obrigatório!";

        public static void CriarFilial(IFormCollection collection, int idEmpresa)
        {
            Filial filial = new Filial().CarregaObjetoFilial1(collection);
            filial.idEmpresa = idEmpresa;
            Conexao objConexao = new Conexao();

            string sql = $"INSERT INTO tb_filial (idEmpresa, cnpjFilial, eMail, nomeFantasia, observacoes)" +
                $"VALUES (@idEmpresa, @cnpjFilial, @eMail, @nomeFantasia, @observacoes);";

            using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
            {
                command.Parameters.AddWithValue("@idEmpresa", filial.idEmpresa);
                command.Parameters.AddWithValue("@cnpjFilial", filial.cnpjFilial.Trim());
                command.Parameters.AddWithValue("@eMail", filial.email.Trim());
                command.Parameters.AddWithValue("@nomeFantasia", filial.nomeFantasia.Trim());
                command.Parameters.AddWithValue("@observacoes", filial.observacoes.Trim());

                objConexao.ExecutarComandoSql(command);
            }
        }

        public static void AlterarFilial1(IFormCollection collection)
        {
            Filial filial = new Filial().CarregaObjetoFilial1(collection);
            int idEmpresa = Convert.ToInt32(collection["idEmpresa"]);
            Conexao objConexao = new Conexao();

            string sql = "UPDATE tb_filial SET cnpjFilial = @cnpjFilial, eMail = @email, " +
                "nomeFantasia = @nomeFantasia, observacoes = @observacoes " +
                $"WHERE idEmpresa = {idEmpresa};";

            using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
            {
                command.Parameters.AddWithValue("@cnpjFilial", filial.cnpjFilial?.Trim());
                command.Parameters.AddWithValue("@email", filial.email?.Trim());
                command.Parameters.AddWithValue("@nomeFantasia", filial.nomeFantasia?.Trim());
                command.Parameters.AddWithValue("@observacoes", filial.observacoes?.Trim());

                objConexao.ExecutarComandoSql(command);
            }
        }

        public Filial CarregaObjetoFilial1(IFormCollection collection)
        {
            Filial filial = new Filial()
            {
                razaoSocial = collection["razaoSocial"],
                nomeFantasia = collection["nomeFantasia"],
                cnpjBase = collection["cnpjBase"],
                cnpjFilial = collection?["cnpjFilial"],
                email = collection["email"],
                observacoes = collection["observacoes"]
            };

            if (!string.IsNullOrEmpty(collection["idEmpresa"]))
            {
                filial.idEmpresa = Convert.ToInt32(collection["idEmpresa"]);
            }

            /*
            filial.endereco = new Endereco();
            filial.endereco.rua = collection["rua"];
            filial.endereco.numero = Convert.ToInt16(collection["numero"]);
            filial.endereco.bairro = collection["bairro"];
            filial.endereco.cidade = collection["cidade"];
            filial.endereco.uf = collection["uf"];
            */

            return filial;
        }

        public Filial CarregaObjetoFilial1(DataRow row)
        {
            Filial filial = new Filial()
            {
                idFilial = Convert.ToInt32(row["idFilial"]),
                idEmpresa = Convert.ToInt32(row["idEmpresa"]),
                nomeFantasia = row["nomeFantasia"]?.ToString(),
                cnpjFilial = row["cnpjFilial"]?.ToString(),
                email = row["email"]?.ToString(),
                observacoes = row["observacoes"]?.ToString(),
                razaoSocial = row["razaoSocial"]?.ToString()
            };

            /*
            filial.endereco = new Endereco();
            filial.endereco.rua = collection["rua"];
            filial.endereco.numero = Convert.ToInt16(collection["numero"]);
            filial.endereco.bairro = collection["bairro"];
            filial.endereco.cidade = collection["cidade"];
            filial.endereco.uf = collection["uf"];
            */

            return filial;
        }

        public List<Filial> GetListaFilial(int idEmpresa)
        {
            Conexao objConexao = new Conexao();
            List<Filial> listaFilial = new List<Filial>();

            string sql = $"SELECT * FROM tb_filial " +
                $"INNER JOIN tb_empresaCliente ON tb_filial.idEmpresa = tb_empresaCliente.idEmpresa " +
                $"WHERE tb_filial.idEmpresa = '{idEmpresa}' ORDER BY tb_filial.idEmpresa ASC";
            DataTable dt = objConexao.RetornaDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                Filial filial = CarregaObjetoFilial1(row);

                listaFilial.Add(filial);
            }

            return listaFilial;
        }


    }
}