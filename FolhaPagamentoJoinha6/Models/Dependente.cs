using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FolhaPagamentoJoinha6.Models
{
    public class Dependente
    {
        [Required(ErrorMessage = mensagemValidacao)]
        public string? primeiroNome { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string? segundoNome { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string? cpf { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public DateTime? dataNasc { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string? grauParent { get; set; }

        public int? funcionarioId { get; set; }

        private const string mensagemValidacao = "Preenchimento Obrigatório!";

        public static bool CriarDependente(IFormCollection collection, out string? mensagemErro)
        {
            Conexao objConexao = new Conexao();
            Dependente dependente = CarregaObjeto(collection);

            string sql = $"INSERT INTO dependente (primeiroNome, segundoNome, cpf, dataNasc, grauParent, funcionarioId) " +
                $"VALUES (@primeiroNome, @segundoNome, @cpf, @dataNasc, @grauParent, @funcionarioId);";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
                {
                    command.Parameters.AddWithValue("@primeiroNome", dependente.primeiroNome?.Trim());
                    command.Parameters.AddWithValue("@segundoNome", dependente.segundoNome?.Trim());
                    command.Parameters.AddWithValue("@cpf", dependente.cpf?.Trim());
                    command.Parameters.AddWithValue("@dataNasc", dependente.dataNasc);
                    command.Parameters.AddWithValue("@grauParent", dependente.grauParent?.Trim());
                    command.Parameters.AddWithValue("@funcionarioId", dependente.funcionarioId);

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

        public static bool AlteraDependente(IFormCollection collection, Conexao objConexao, SqlTransaction sqlTransaction)
        {
            Dependente dependente = CarregaObjeto(collection);

            string sql = $"UPDATE dependente SET primeiroNome = @primeiroNome, segundoNome = @segundoNome, cpf=@cpf, " +
            $"dataNasc = @dataNasc, grauParent = @grauParent " +
            $"WHERE funcionarioId = @funcionarioId;";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection, sqlTransaction))
                {
                    command.Parameters.AddWithValue("@empresaId", dependente.primeiroNome?.Trim());
                    command.Parameters.AddWithValue("@razaoSocial", dependente.segundoNome?.Trim());
                    command.Parameters.AddWithValue("@cnpj", dependente.cpf?.Trim());
                    command.Parameters.AddWithValue("@dataNasc", dependente.dataNasc);
                    command.Parameters.AddWithValue("@grauParent", dependente.grauParent?.Trim());
                    command.Parameters.AddWithValue("@funcionarioId", dependente.funcionarioId);

                    objConexao.ExecutarComandoSql(command, true);
                }

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<Dependente> GetListaDependente(int funcionarioId)
        {
            Conexao objConexao = new Conexao();
            List<Dependente> listaDependente = new List<Dependente>();

            string sql = $"SELECT * FROM dependente WHERE funcionarioId = {funcionarioId}";
            DataTable dt = objConexao.RetornaDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                Dependente dependente = CarregaObjeto(row);
                listaDependente.Add(dependente);
            }

            return listaDependente;
        }

        public static Dependente? GetDependente(int? cpf)
        {
            Conexao objConexao = new Conexao();
            string sql = $"SELECT * FROM dependente WHERE cpf = '{cpf}';";
            DataTable dt = objConexao.RetornaDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                Dependente dependente = CarregaObjeto(dt.Rows[0]);

                return dependente;
            }

            else
            {
                return null;
            }
        }

        private static Dependente CarregaObjeto(IFormCollection collection)
        {
            Dependente dependente = new Dependente()
            {
                primeiroNome = collection["primeiroNome"].ToString(),
                segundoNome = collection["segundoNome"].ToString(),
                cpf = collection["cpf"].ToString(),
                grauParent = collection["grauParent"].ToString(),
                funcionarioId = Convert.ToInt32(collection["funcionarioId"])
            };

            //verifica se o int é nulo
            //dependente.funcionarioId = int.TryParse(collection["funcionarioId"], out int parsedFuncionarioId) ? parsedFuncionarioId : null;

            dependente.dataNasc = ConverteData(collection["dataNasc"].ToString());

            return dependente;
        }

        private static Dependente CarregaObjeto(DataRow dataRow)
        {
            Dependente dependente = new Dependente()
            {
                primeiroNome = dataRow["primeiroNome"].ToString(),
                segundoNome = dataRow["segundoNome"].ToString(),
                cpf = dataRow["cpf"].ToString(),
                grauParent = dataRow["grauParent"].ToString(),
                funcionarioId = Convert.ToInt32(dataRow["funcionarioId"])
            };

            dependente.dataNasc = ConverteData(dataRow, "dataNasc");

            return dependente;
        }

        private static DateTime? ConverteData(string data)
        {
            if (!string.IsNullOrEmpty(data) && DateTime.TryParse(data, out DateTime dataNascimeto))
            {
                return dataNascimeto;
            }

            return null;
        }

        private static DateTime? ConverteData(DataRow dataRow, string nomeCampo)
        {
            if (dataRow[nomeCampo] != DBNull.Value)
            {
                return Convert.ToDateTime(dataRow[nomeCampo]);
            }

            return null;
        }
    }
}
