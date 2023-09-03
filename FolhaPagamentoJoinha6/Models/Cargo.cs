using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace FolhaPagamentoJoinha6.Models
{
    public class Cargo
    {
        public int? idCargo { get; set; }
        public string? cargoNome { get; set; }
        public float salarioBase { get; set; }
        public int idDepartamento { get; set; }

        public static bool CriarCargo(IFormCollection collection, out string? mensagemErro)
        {
            Cargo cargo = CarregaObjeto(collection);
            Conexao objConexao = new Conexao();

            string sql = $"INSERT INTO tb_cargo (cargoNome, salarioBase,idDepartamento ) " +
                $"VALUES (@cargoNome, @salarioBase, @idDepartamento);";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
                {
                    command.Parameters.AddWithValue("@cargoNome", cargo.cargoNome?.Trim());
                    command.Parameters.AddWithValue("@salarioBase", cargo.salarioBase);
                    command.Parameters.AddWithValue("@idDepartamento", cargo.idDepartamento);
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

        public static List<Cargo> GetListaCargo(int idDepartamento)
        {
            Conexao objConexao = new Conexao();
            List<Cargo> listaCargo = new List<Cargo>();

            string sql = $"SELECT * FROM tb_cargo WHERE idDepartamento = {idDepartamento}";
            DataTable dt = objConexao.RetornaDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                Cargo cargo = CarregaObjeto(row);
                listaCargo.Add(cargo);
            }

            return listaCargo;
        }

        private static Cargo CarregaObjeto(IFormCollection collection)
        {
            Cargo cargo = new Cargo()
            {
                idCargo = !string.IsNullOrEmpty(collection["idCargo"]) ? Convert.ToInt32(collection["idCargo"]) : null,
                cargoNome = collection["cargoNome"].ToString(),
                salarioBase = Convert.ToInt32(collection["salarioBase"]),
                idDepartamento = Convert.ToInt32(collection["idDepartamento"])
            };

            return cargo;
        }

        private static Cargo CarregaObjeto(DataRow dataRow)
        {
            Cargo cargo = new Cargo()
            {
                idCargo = !dataRow.IsNull("idCargo") ? Convert.ToInt32(dataRow["idCargo"]) : null,
                cargoNome = dataRow["cargoNome"]?.ToString() ?? "",
                salarioBase = Convert.ToInt32(dataRow["salarioBase"]),
                idDepartamento = Convert.ToInt32(dataRow["idDepartamento"])
            };

            return cargo;
        }
    }
}
