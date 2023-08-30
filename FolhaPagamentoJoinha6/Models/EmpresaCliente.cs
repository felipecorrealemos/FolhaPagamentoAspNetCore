using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class EmpresaCliente
    {
        public int? idEmpresa { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string razaoSocial { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string cnpj { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string nomeFantasia { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string apelido { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string empresaMae { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public bool ehMatriz { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string email { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string telefone { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string observacao { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public Endereco endereco { get; set; }


        private const string mensagemValidacao = "Preenchimento Obrigatório!";

        public static bool CriarEmpresaEFilial1(IFormCollection collection, out string? mensagemErro)
        {
            EmpresaCliente empresa = CarregaObjeto(collection);
            Conexao objConexao = new Conexao();

            string sql = $"INSERT INTO tb_empresaCliente (razaoSocial, cnpjBase) " +
                $"VALUES (@razaosocial, @cnpjbase); " +
                $"SELECT SCOPE_IDENTITY();";

            int newID;

            using (SqlTransaction transaction = objConexao.sqlConnection.BeginTransaction())
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection, transaction))
                    {
                        command.Parameters.AddWithValue("@razaosocial", empresa.razaoSocial.Trim());
                        command.Parameters.AddWithValue("@cnpjbase", empresa.cnpjBase.Trim());

                        objConexao.ExecutarComandoSql(command, out newID, true);
                    }

                    Filial.CriarFilial(collection, newID);

                    //commit aqui
                    objConexao.Commit(transaction, objConexao.sqlConnection);
                    mensagemErro = null;
                    return true;
                }

                catch (Exception ex)
                {
                    objConexao.Rollback(transaction, objConexao.sqlConnection);
                    mensagemErro = ex.Message;
                    return false;
                }
            }
        }

        public static Filial GetEmpresaEFilial1(int idEmpresa)
        {
            Conexao objConexao = new Conexao();
            Filial filial = new Filial();

            string sql = "SELECT * FROM tb_filial " +
                "INNER JOIN tb_empresaCliente ON tb_filial.idEmpresa = tb_empresaCliente.idEmpresa " +
                $"WHERE tb_filial.idEmpresa = '{idEmpresa}'";
            DataTable dt = objConexao.RetornaDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                filial.idEmpresa = Convert.ToInt32(row["idEmpresa"]);
                filial.razaoSocial = row["razaoSocial"].ToString();
                filial.cnpjBase = row["cnpjBase"].ToString();
                filial.idFilial = Convert.ToInt32(row["idEmpresa"]);
                filial.cnpjFilial = row["cnpjFilial"].ToString();
                filial.nomeFantasia = row["nomeFantasia"]?.ToString();
                filial.email = row["email"]?.ToString();
                filial.observacoes = row["observacoes"]?.ToString();
            }

            return filial;
        }

        public List<EmpresaCliente> GetListaEmpresas()
        {
            Conexao objConexao = new Conexao();
            List<EmpresaCliente> listaEmpresa = new List<EmpresaCliente>();

            string sql = "SELECT * FROM tb_empresaCliente ORDER BY idEmpresa ASC";
            DataTable dt = objConexao.RetornaDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                EmpresaCliente empresa = new EmpresaCliente()
                {
                    idEmpresa = Convert.ToInt32(row["idEmpresa"]),
                    razaoSocial = row["razaoSocial"].ToString(),
                    cnpjBase = row["cnpjBase"].ToString(),
                };

                listaEmpresa.Add(empresa);
            }

            return listaEmpresa;
        }

        public static EmpresaCliente? GetEmpresa(int? id)
        {
            Conexao objConexao = new Conexao();
            string sql = $"SELECT * FROM tb_empresaCliente WHERE idEmpresa = '{id}';";
            DataTable dt = objConexao.RetornaDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                EmpresaCliente empresa = new EmpresaCliente()
                {
                    idEmpresa = Convert.ToInt32(dt.Rows[0]["idEmpresa"]),
                    razaoSocial = dt.Rows[0]["razaoSocial"].ToString(),
                    cnpjBase = dt.Rows[0]["cnpjBase"].ToString(),
                };

                return empresa;
            }

            else
            {
                return null;
            }
        }

        public static void AlterarEmpresa(IFormCollection collection)
        {
            EmpresaCliente empresa = new EmpresaCliente()
            {
                idEmpresa = Convert.ToInt32(collection["idEmpresa"]),
                razaoSocial = collection["razaoSocial"],
                cnpjBase = collection["cnpjBase"],
            };

            Conexao objConexao = new Conexao();

            string sql = "UPDATE tb_empresaCliente SET razaoSocial = @razaoSocial, cnpjBase = @cnpjBase " +
                "WHERE idEmpresa = @idEmpresa";

            using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
            {
                command.Parameters.AddWithValue("@idEmpresa", empresa.idEmpresa);
                command.Parameters.AddWithValue("@razaoSocial", empresa.razaoSocial.Trim());
                command.Parameters.AddWithValue("@cnpjBase", empresa.cnpjBase.Trim());

                objConexao.ExecutarComandoSql(command, false);
            }
        }

        public static bool VerificaCNPJExiste(IFormCollection collection)
        {
            Conexao objConexao = new Conexao();
            string sql = $"SELECT * FROM tb_empresaCliente WHERE cnpjBase = '{collection["cnpjBase"]}' AND idEmpresa <> '{collection?["idEmpresa"]}';";
            DataTable dt = objConexao.RetornaDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public static EmpresaCliente CarregaObjeto(IFormCollection collection)
        {
            EmpresaCliente empresaCliente = new EmpresaCliente()
            {
                cnpjBase = collection["cnpjBase"],
                razaoSocial = collection["razaoSocial"]
            };

            return empresaCliente;
        }
    }
}