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
        public int? departamentoId { get; set; }
        public string? departamentoNome { get; set; }
        public int? empresaId { get; set; }
        public int? totalCargo { get; set; }

        public static bool CriarDepartamento(IFormCollection collection, out string? mensagemErro)
        {
            Departamento departamento = CarregaObjeto(collection);
            Conexao objConexao = new Conexao();

            string sql = $"INSERT INTO departamentos (departamentoNome, empresaId) " +
                $"VALUES (@departamentoNome, @empresaId);";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
                {
                    command.Parameters.AddWithValue("@departamentoNome", departamento.departamentoNome?.Trim());
                    command.Parameters.AddWithValue("@empresaId", departamento.empresaId);
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

        public static List<Departamento> GetListaDepartamento(int empresaId)
        {
            Conexao objConexao = new Conexao();
            List<Departamento> listaDepartamento = new List<Departamento>();

            string sql = $"SELECT d.*,(SELECT COUNT(*) FROM cargos c " +
                $"WHERE c.departamentoId = d.departamentoId) AS TotalCargos " +
                $"FROM departamentos d WHERE d.empresaId = {empresaId};";
            DataTable dt = objConexao.RetornaDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                Departamento departamento = CarregaObjeto(row);
                listaDepartamento.Add(departamento);
            }

            return listaDepartamento;
        }

        public static Departamento GetDepartamento(int departamentoId)
        {
            Conexao objConexao = new Conexao();

            string sql = $"SELECT * FROM departamentos WHERE departamentoId = {departamentoId}";
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
                departamentoId = !string.IsNullOrEmpty(collection["departamentoId"]) ? Convert.ToInt32(collection["departamentoId"]) : 0,
                departamentoNome = collection["departamentoNome"].ToString(),
                empresaId = Convert.ToInt32(collection["empresaId"])
            };

            return departamento;
        }

        public static Departamento CarregaObjeto(DataRow dataRow)
        {
            Departamento departamento = new Departamento()
            {
                departamentoId = Convert.ToInt32(dataRow["departamentoId"]),
                departamentoNome = dataRow["departamentoNome"].ToString(),
                empresaId = Convert.ToInt32(dataRow["empresaId"]),
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