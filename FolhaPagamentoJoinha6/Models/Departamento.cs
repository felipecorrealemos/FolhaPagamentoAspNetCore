using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class Departamento
    {
        public int? idDepartamento { get; set; }
        public string? departamentoNome { get; set; }
        public int? idEmpresa { get; set; }
        public int? totalCargo { get; set; }

        public static bool CriarDepartamento(IFormCollection collection, out string? mensagemErro)
        {
            Departamento departamento = CarregaObjeto(collection);
            Conexao objConexao = new Conexao();

            string sql = $"INSERT INTO tb_departamento (departamentoNome, idEmpresa) " +
                $"VALUES (@departamentoNome, @idEmpresa);";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
                {
                    command.Parameters.AddWithValue("@departamentoNome", departamento.departamentoNome?.Trim());
                    command.Parameters.AddWithValue("@idEmpresa", departamento.idEmpresa);
                    objConexao.ExecutarComandoSql(command, false);
                }

                mensagemErro = null;
                return true;
            }

            catch (Exception ex)
            {
                mensagemErro = ex.Message;
                return false;
            }
        }

        public static List<Departamento> GetListaDepartamento(int idEmpresa)
        {
            Conexao objConexao = new Conexao();
            List<Departamento> listaDepartamento = new List<Departamento>();

            string sql = $"SELECT d.*,(SELECT COUNT(*) FROM tb_cargo c " +
                $"WHERE c.idDepartamento = d.idDepartamento) AS TotalCargos " +
                $"FROM tb_departamento d WHERE d.idEmpresa = {idEmpresa};";
            DataTable dt = objConexao.RetornaDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                Departamento departamento = CarregaObjeto(row);
                listaDepartamento.Add(departamento);
            }

            return listaDepartamento;
        }

        public static Departamento GetDepartamento(int idDepartamento)
        {
            Conexao objConexao = new Conexao();

            string sql = $"SELECT * FROM tb_departamento WHERE idDepartamento = {idDepartamento}";
            DataTable dt = objConexao.RetornaDataTable(sql);
            Departamento departamento = new Departamento();

            if (dt.Rows.Count > 0)
            {
                departamento = CarregaObjeto(dt.Rows[0]);
            }

            return departamento;
        }

        public static Departamento CarregaObjeto(IFormCollection collection)
        {
            Departamento departamento = new Departamento()
            {
                idDepartamento = !string.IsNullOrEmpty(collection["idDepartamento"]) ? Convert.ToInt32(collection["idDepartamento"]) : 0,
                departamentoNome = collection["departamentoNome"].ToString(),
                idEmpresa = Convert.ToInt32(collection["idEmpresa"])
            };

            return departamento;
        }

        public static Departamento CarregaObjeto(DataRow dataRow)
        {
            Departamento departamento = new Departamento()
            {
                idDepartamento = Convert.ToInt32(dataRow["idDepartamento"]),
                departamentoNome = dataRow["departamentoNome"].ToString(),
                idEmpresa = Convert.ToInt32(dataRow["idEmpresa"]),
                //totalCargo = Convert.ToInt32(dataRow["TotalCargos"])
            };

            if (dataRow.Table.Columns.Contains("TotalCargos"))
            {
                departamento.totalCargo = Convert.ToInt32(dataRow["TotalCargos"]);
            }

            return departamento;
        }
    }
}