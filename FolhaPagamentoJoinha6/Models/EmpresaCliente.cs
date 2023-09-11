using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class EmpresaCliente
    {
        public int? empresaId { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string cnpj { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string razaoSocial { get; set; }

        public string? nomeFantasia { get; set; }

        public string? apelido { get; set; }

        public int? endereco { get; set; }

        public string? observacao { get; set; }

        public string? telefone { get; set; }

        public string? email { get; set; }

        public bool ehMatriz { get; set; }

        public string? empresaMae { get; set; }

        private const string mensagemValidacao = "Preenchimento Obrigatório!";

        private static bool CriarEmpresa(IFormCollection collection, int idEndereco, Conexao objConexao, SqlTransaction sqlTransaction, out int idEmpresaMae, out string? mensagemErro)
        {
            EmpresaCliente empresa = CarregaObjeto(collection);
            empresa.endereco = idEndereco;

            string sql = $"INSERT INTO empresa_clientes (razaoSocial, cnpj, nomeFantasia, apelido, empresaMae, " +
                $"ehMatriz, telefone, email, observacao, endereco) " +
                $"VALUES (@razaoSocial, @cnpj, @nomeFantasia, @apelido, @empresaMae, " +
                $"@ehMatriz, @telefone, @email, @observacao, @endereco); " +
                $"SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection, sqlTransaction))
                {
                    command.Parameters.AddWithValue("@razaoSocial", empresa.razaoSocial?.Trim());
                    command.Parameters.AddWithValue("@cnpj", empresa.cnpj?.Trim());
                    command.Parameters.AddWithValue("@nomeFantasia", empresa.nomeFantasia?.Trim());
                    command.Parameters.AddWithValue("@apelido", empresa.apelido?.Trim());
                    command.Parameters.AddWithValue("@empresaMae", empresa.empresaMae?.Trim());
                    command.Parameters.AddWithValue("@ehMatriz", SqlDbType.Bit).Value = empresa.ehMatriz;
                    command.Parameters.AddWithValue("@endereco", empresa.endereco);
                    command.Parameters.AddWithValue("@telefone", empresa.telefone);
                    command.Parameters.AddWithValue("@email", empresa.email);
                    command.Parameters.AddWithValue("@observacao", empresa.observacao);

                    objConexao.ExecutarComandoSql(command, out idEmpresaMae, true);
                }

                mensagemErro = null;
                return true;
            }

            catch (Exception ex)
            {
                mensagemErro = ex.Message;
                idEmpresaMae = 0;
                return false;
            }
        }

        public static bool CriarEmpresaEEndereco(IFormCollection collection, out string? mensagemErro)
        {
            Conexao objConexao = new Conexao();
            SqlTransaction transaction = objConexao.sqlConnection.BeginTransaction();

            //cria o endereco
            if (!Endereco.CriaEndereco(collection, objConexao, transaction, out int idEndereco, out mensagemErro))
            {
                return false;
            }

            //cria empresa com o idEndereco
            if (!CriarEmpresa(collection, idEndereco, objConexao, transaction, out int idEmpresaMae, out mensagemErro))
            {
                objConexao.Rollback(transaction, objConexao.sqlConnection);
                return false;
            }

            //verifica se possui o campo ehMatriz no formulario para criar a empresa matriz
            if (collection.TryGetValue("ehMatriz", out var ehMatrizValue))
            {
                if (!AlteraIdEmpresaMae(idEmpresaMae, objConexao, transaction, out mensagemErro))
                {
                    objConexao.Rollback(transaction, objConexao.sqlConnection);
                    return false;
                }
            }

            objConexao.Commit(transaction, objConexao.sqlConnection);
            mensagemErro = null;
            return true;
        }

        public static List<EmpresaCliente> GetListaEmpresasMatriz()
        {
            Conexao objConexao = new Conexao();
            List<EmpresaCliente> listaEmpresa = new List<EmpresaCliente>();

            string sql = "SELECT * FROM empresa_clientes WHERE ehMatriz = 1 ORDER BY empresaId ASC";
            DataTable dt = objConexao.RetornaDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                EmpresaCliente empresa = CarregaObjeto(row);
                listaEmpresa.Add(empresa);
            }

            return listaEmpresa;
        }

        public static EmpresaCliente? GetEmpresa(int? id)
        {
            Conexao objConexao = new Conexao();
            string sql = $"SELECT * FROM empresa_clientes WHERE empresaId = '{id}';";
            DataTable dt = objConexao.RetornaDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                EmpresaCliente empresa = CarregaObjeto(dt.Rows[0]);

                return empresa;
            }

            else
            {
                return null;
            }
        }

        public static List<EmpresaCliente>? GetFiliais(int? empresaMae)
        {
            Conexao objConexao = new Conexao();
            string sql = $"SELECT * FROM empresa_clientes WHERE empresaMae = '{empresaMae}' AND ehMatriz = 'false';";
            DataTable dt = objConexao.RetornaDataTable(sql);

            List<EmpresaCliente> listaEmpresa = new List<EmpresaCliente>();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    EmpresaCliente empresa = CarregaObjeto(row);
                    listaEmpresa.Add(empresa);
                }


                return listaEmpresa;
            }

            else
            {
                return null;
            }
        }

        private static bool AlteraIdEmpresaMae(int idEmpresaMae, Conexao objConexao, SqlTransaction sqlTransaction, out string? mensagemErro)
        {
            string sql = $"UPDATE empresa_clientes SET empresaMae = {idEmpresaMae} WHERE empresaId = {idEmpresaMae}";

            SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection, sqlTransaction);

            try
            {
                objConexao.ExecutarComandoSql(command, true);
                mensagemErro = null;
                return true;
            }

            catch (Exception ex)
            {
                mensagemErro = ex.Message;
                return false;
            }
        }

        public static void AlterarEmpresa(IFormCollection collection)
        {
            EmpresaCliente empresa = new EmpresaCliente()
            {
                empresaId = Convert.ToInt32(collection["empresaId"]),
                razaoSocial = collection["razaoSocial"],
                // cnpjBase = collection["cnpjBase"],
            };

            Conexao objConexao = new Conexao();

            string sql = "UPDATE empresa_clientes SET razaoSocial = @razaoSocial, cnpj = @cnpj" +
                "WHERE empresaId = @empresaId";

            using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
            {
                command.Parameters.AddWithValue("@empresaId", empresa.empresaId);
                command.Parameters.AddWithValue("@razaoSocial", empresa.razaoSocial.Trim());
                command.Parameters.AddWithValue("@cnpj", empresa.cnpj.Trim());

                objConexao.ExecutarComandoSql(command, false);
            }
        }

        public static bool VerificaCNPJExiste(IFormCollection collection)
        {
            Conexao objConexao = new Conexao();
            string sql = $"SELECT * FROM empresa_clientes WHERE cnpj = '{collection["cnpj"]}' AND empresaId <> '{collection?["empresaId"]}';";
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
                razaoSocial = collection["razaoSocial"].ToString(),
                cnpj = collection["cnpj"].ToString(),
                nomeFantasia = collection["nomeFantasia"].ToString(),
                apelido = collection["apelido"].ToString(),
                empresaMae = collection["empresaMae"].ToString(),
                endereco = Convert.ToInt32(collection["endereco"]),
                telefone = collection["telefone"].ToString(),
                email = collection["email"].ToString(),
                observacao = collection["observacao"].ToString()
            };

            if (collection.TryGetValue("ehMatriz", out var ehMatrizValue))
            {
                if (bool.TryParse(ehMatrizValue, out var valorBool))
                {
                    if (valorBool)
                    {
                        empresaCliente.ehMatriz = true;
                    }
                }
            }

            /*if(collection["ehMatriz"].Count > 1)
            {

            }*/

            return empresaCliente;
        }

        public static EmpresaCliente CarregaObjeto(DataRow dataRow)
        {
            EmpresaCliente empresaCliente = new EmpresaCliente()
            {
                empresaId = Convert.ToInt32(dataRow["empresaId"]),
                razaoSocial = dataRow["razaoSocial"]?.ToString(),
                cnpj = dataRow["cnpj"]?.ToString(),
                nomeFantasia = dataRow["nomeFantasia"]?.ToString(),
                apelido = dataRow["apelido"]?.ToString(),
                empresaMae = dataRow["empresaMae"]?.ToString(),
                ehMatriz = Convert.ToBoolean(dataRow["ehMatriz"]),
                endereco = Convert.ToInt32(dataRow["empresaId"]),
                telefone = dataRow["telefone"].ToString(),
                email = dataRow["email"].ToString(),
                observacao = dataRow["observacao"].ToString()
            };

            return empresaCliente;
        }
    }
}