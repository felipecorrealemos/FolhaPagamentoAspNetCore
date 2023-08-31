using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Transactions;
using System.Web;

namespace FolhaPagamentoJoinha6.Models
{
    public class Endereco
    {
        [Required(ErrorMessage = mensagemValidacao)]
        public int? numero { get; set; }
        [Required(ErrorMessage = mensagemValidacao)]
        public string? rua { get; set; }
        public string? bairro { get; set; }
        public string? cidade { get; set; }
        public string? estado { get; set; }
        public string? cep { get; set; }
        public string? pais { get; set; }

        private const string mensagemValidacao = "Preenchimento Obrigatório!";

        public static bool CriaEndereco(IFormCollection collection, Conexao objConexao, SqlTransaction sqlTransaction, out int newID, out string? mensagemErro)
        {
            Endereco endereco = new Endereco().CarregaObjeto(collection);
            newID = 0;

            string sql = $"INSERT INTO tb_endereco (numero, rua, bairro, cidade, estado, cep, pais) " +
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

        private Endereco CarregaObjeto(IFormCollection collection)
        {
            Endereco endereco = new Endereco()
            {
                numero = Convert.ToInt32(collection["numero"]),
                rua = collection["rua"],
                bairro = collection["bairro"],
                cidade = collection["cidade"],
                estado = collection["estado"],
                cep = collection["cep"],
                pais = collection["pais"]
            };

            return endereco;
        }
    }
}