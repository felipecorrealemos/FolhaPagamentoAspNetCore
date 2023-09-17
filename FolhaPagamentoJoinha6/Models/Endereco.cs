using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FolhaPagamentoJoinha6.Models
{
    public class Endereco
    {
        public int? enderecoId { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string? cep { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string? rua { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string? bairro { get; set; }

        public string? complemento { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public int? numero { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string? cidade { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string? estado { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string? pais { get; set; }

        private const string mensagemValidacao = "Preenchimento Obrigatório!";

        public static bool CriaEndereco(IFormCollection collection, Conexao objConexao, SqlTransaction sqlTransaction, out int newID, out string? mensagemErro)
        {
            Endereco endereco = new Endereco().CarregaObjeto(collection);
            newID = 0;

            string sql = $"INSERT INTO enderecos (numero, rua, bairro, cidade, estado, cep, pais) " +
                $"VALUES (@numero, @rua, @bairro, @cidade, @estado, @cep, @pais);" +
            $"SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection, sqlTransaction))
                {
                    command.Parameters.AddWithValue("@numero", endereco.numero);
                    command.Parameters.AddWithValue("@rua", endereco.rua?.Trim());
                    command.Parameters.AddWithValue("@bairro", endereco.bairro?.Trim());
                    command.Parameters.AddWithValue("@cidade", endereco.cidade?.Trim());
                    command.Parameters.AddWithValue("@estado", endereco.estado?.Trim());
                    command.Parameters.AddWithValue("@cep", endereco.cep?.Trim());
                    command.Parameters.AddWithValue("@pais", endereco.pais?.Trim());

                    objConexao.ExecutarComandoSql(command, out newID, true);
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

        public static bool AlteraEndereco(IFormCollection collection, Conexao objConexao, SqlTransaction sqlTransaction, out string? mensagemErro)
        {
            Endereco endereco = new Endereco().CarregaObjeto(collection);

            string sql = $"UPDATE enderecos SET cep = @cep, rua = @rua, numero = @numero, bairro = @bairro, " +
                $"complemento = @complemento, cidade = @cidade, estado = @estado, pais = @pais WHERE enderecoId = @enderecoId;";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection, sqlTransaction))
                {
                    command.Parameters.AddWithValue("@enderecoId", endereco.enderecoId);
                    command.Parameters.AddWithValue("@cep", endereco.cep?.Trim());
                    command.Parameters.AddWithValue("@rua", endereco.rua?.Trim());
                    command.Parameters.AddWithValue("@numero", endereco.numero);
                    command.Parameters.AddWithValue("@bairro", endereco.bairro?.Trim());
                    command.Parameters.AddWithValue("@complemento", endereco.complemento?.Trim());
                    command.Parameters.AddWithValue("@cidade", endereco.cidade?.Trim());
                    command.Parameters.AddWithValue("@estado", endereco.estado?.Trim());
                    command.Parameters.AddWithValue("@pais", endereco.pais?.Trim());

                    objConexao.ExecutarComandoSql(command, true);
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

        private Endereco CarregaObjeto(IFormCollection collection)
        {
            Endereco endereco = new Endereco()
            {
                enderecoId = !string.IsNullOrEmpty(collection["enderecoId"]) ? Convert.ToInt32(collection["enderecoId"]) : null,
                numero = Convert.ToInt32(collection["numero"]),
                rua = collection["rua"],
                bairro = collection["bairro"],
                cidade = collection["cidade"],
                estado = collection["estado"],
                cep = collection["cep"],
                pais = collection["pais"],
                complemento = collection["complemento"].ToString()
            };

            return endereco;
        }

        public static Endereco CarregaObjeto(DataRow dataRow)
        {
            Endereco endereco = new Endereco()
            {
                enderecoId = Convert.ToInt32(dataRow["enderecoId"]),
                numero = Convert.ToInt32(dataRow["numero"]),
                rua = dataRow["rua"]?.ToString(),
                bairro = dataRow["bairro"]?.ToString(),
                cidade = dataRow["cidade"]?.ToString(),
                estado = dataRow["estado"]?.ToString(),
                cep = dataRow["cep"]?.ToString(),
                pais = dataRow["pais"]?.ToString(),
                complemento = dataRow["complemento"]?.ToString()
            };

            return endereco;
        }

        public static Endereco? GetEndereco(int? enderecoId)
        {
            Conexao objConexao = new Conexao();
            string sql = $"SELECT * FROM enderecos WHERE enderecoId = '{enderecoId}';";
            DataTable dt = objConexao.RetornaDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                Endereco endereco = CarregaObjeto(dt.Rows[0]);
                return endereco;
            }

            else
            {
                return null;
            }
        }
    }
}