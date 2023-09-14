using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace FolhaPagamentoJoinha6.Models
{
    public class Cargo
    {
        public int? cargoId { get; set; }
        public string? cargoNome { get; set; }
        public float salarioBase { get; set; }
        public int departamentoId { get; set; }

        public static bool CriarCargo(IFormCollection collection, out string? mensagemErro)
        {
            Cargo cargo = CarregaObjeto(collection);
            Conexao objConexao = new Conexao();

            string sql = $"INSERT INTO cargos (cargoNome, salarioBase,departamentoId ) " +
                $"VALUES (@cargoNome, @salarioBase, @departamentoId);";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
                {
                    command.Parameters.AddWithValue("@cargoNome", cargo.cargoNome?.Trim());
                    command.Parameters.AddWithValue("@salarioBase", cargo.salarioBase);
                    command.Parameters.AddWithValue("@departamentoId", cargo.departamentoId);
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

        public static List<Cargo> GetListaCargo(int departamentoId)
        {
            Conexao objConexao = new Conexao();
            List<Cargo> listaCargo = new List<Cargo>();

            string sql = $"SELECT * FROM cargos WHERE departamentoId = {departamentoId}";
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
                cargoId = !string.IsNullOrEmpty(collection["cargoId"]) ? Convert.ToInt32(collection["cargoId"]) : null,
                cargoNome = collection["cargoNome"].ToString(),
                salarioBase = Convert.ToInt32(collection["salarioBase"]),
                departamentoId = Convert.ToInt32(collection["departamentoId"])
            };

            return cargo;
        }

        private static Cargo CarregaObjeto(DataRow dataRow)
        {
            Cargo cargo = new Cargo()
            {
                cargoId = !dataRow.IsNull("cargoId") ? Convert.ToInt32(dataRow["cargoId"]) : null,
                cargoNome = dataRow["cargoNome"]?.ToString() ?? "",
                salarioBase = Convert.ToInt32(dataRow["salarioBase"]),
                departamentoId = Convert.ToInt32(dataRow["departamentoId"])
            };

            return cargo;
        }
    }
}
