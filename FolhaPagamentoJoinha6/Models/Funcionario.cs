using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FolhaPagamentoJoinha6.Models
{
    public class Funcionario
    {
        public int funcionarioId { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string primeiroNome { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string ultimoNome { get; set; }

        public string? nomeSocial { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string rg { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string cpf { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public DateTime? dataNasc { get; set; }

        public int endereco { get; set; }

        public string telefone { get; set; }
        public string email { get; set; }
        public bool pcd { get; set; }
        public string etnia { get; set; }
        public string certificacoes { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public string escolaridade { get; set; }

        [Required(ErrorMessage = mensagemValidacao)]
        public DateTime? dataAdmissao { get; set; }

        public DateTime? dataDemissao { get; set; }

        public int cargoId { get; set; }

        /*public bool optanteVT { get; set; }
        public int idFilial { get; set; }
        public int idDepartamento { get; set; }
        public string tipoContrato { get; set; }
        public int jornadaTrabalho { get; set; }
        public bool pagaPensao { get; set; }

        public DadosPensao dadosPensao { get; set; }*/

        private List<AtestadoMedico> listaAtestadoMedico { get; set; }
        private List<Certificacoes> listaCertificacoes { get; set; }

        private const string mensagemValidacao = "Preenchimento Obrigatório!";

        public static bool CriaFuncionarioEEndereco(IFormCollection collection, out string? mensagemErro)
        {
            Conexao objConexao = new Conexao();
            SqlTransaction transaction = objConexao.sqlConnection.BeginTransaction();

            //cria o endereco
            if (!Endereco.CriaEndereco(collection, objConexao, transaction, out int idEndereco, out mensagemErro))
            {
                return false;
            }

            //cria funcionario com o idEndereco
            if (!Criarfuncionario(collection, idEndereco, objConexao, transaction, out mensagemErro))
            {
                objConexao.Rollback(transaction, objConexao.sqlConnection);
                return false;
            }

            objConexao.Commit(transaction, objConexao.sqlConnection);
            mensagemErro = null;
            return true;
        }

        private static bool Criarfuncionario(IFormCollection collection, int enderecoId, Conexao objConexao, SqlTransaction sqlTransaction, out string? mensagemErro)
        {
            Funcionario funcionario = CarregaObjeto(collection);
            funcionario.endereco = enderecoId;

            string sql = $"INSERT INTO funcionarios (primeiroNome, ultimoNome, nomeSocial, rg, cpf, dataNasc, " +
                $"telefone, email, pcd, etnia, certificacoes, escolaridade, dataAdmissao, endereco) " +
                $"VALUES (@primeiroNome, @ultimoNome, @nomeSocial, @rg, @cpf, @dataNasc, " +
                $"@telefone, @email, @pcd, @etnia, @certificacoes, @escolaridade, @dataAdmissao, @endereco);";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection, sqlTransaction))
                {
                    command.Parameters.AddWithValue("@primeiroNome", funcionario.primeiroNome.Trim());
                    command.Parameters.AddWithValue("@ultimoNome", funcionario.ultimoNome?.Trim());
                    command.Parameters.AddWithValue("@nomeSocial", funcionario.nomeSocial?.Trim());
                    command.Parameters.AddWithValue("@rg", funcionario.rg?.Trim());
                    command.Parameters.AddWithValue("@cpf", funcionario.cpf?.Trim());
                    command.Parameters.AddWithValue("@dataNasc", funcionario.dataNasc);
                    command.Parameters.AddWithValue("@telefone", funcionario.telefone);
                    command.Parameters.AddWithValue("@email", funcionario.email);
                    command.Parameters.AddWithValue("@pcd", SqlDbType.Bit).Value = funcionario.pcd;
                    command.Parameters.AddWithValue("@etnia", funcionario.etnia);
                    command.Parameters.AddWithValue("@certificacoes", funcionario.certificacoes);
                    command.Parameters.AddWithValue("@escolaridade", funcionario.escolaridade);
                    command.Parameters.AddWithValue("@dataAdmissao", funcionario.dataAdmissao);
                    command.Parameters.AddWithValue("@endereco", funcionario.endereco);
                    //command.Parameters.AddWithValue("@dataDemissao", funcionario.dataDemissao);
                    //command.Parameters.AddWithValue("@cargoId", funcionario.cargoId);

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

        public static List<Funcionario> GetListafuncionario()
        {
            Conexao objConexao = new Conexao();
            List<Funcionario> listaFuncionario = new List<Funcionario>();

            string sql = "SELECT * FROM funcionarios ORDER BY funcionarioId ASC";
            DataTable dt = objConexao.RetornaDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                Funcionario funcionario = CarregaObjeto(row);
                listaFuncionario.Add(funcionario);
            }

            return listaFuncionario;
        }

        public static Funcionario? Getfuncionario(int? funcionarioId)
        {
            Conexao objConexao = new Conexao();
            string sql = $"SELECT * FROM funcionarios WHERE funcionarioId = '{funcionarioId}';";
            DataTable dt = objConexao.RetornaDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                Funcionario funcionario = CarregaObjeto(dt.Rows[0]);

                return funcionario;
            }

            else
            {
                return null;
            }
        }

        public static void AlterarFuncionario(IFormCollection collection)
        {
            Funcionario funcionario = CarregaObjeto(collection);
            Conexao objConexao = new Conexao();

            string sql = "UPDATE funcionarios SET razaoSocial = @razaoSocial, cnpj = @cnpj" +
                "primeiroNome = @primeiroNome, ultimoNome = @ultimoNome, nomeSocial = @nomeSocial, rg = @rg, " +
                "cpf = @cpf, dataNasc = @dataNasc, telefone = @telefone, email = @email, pcd = @pcd, " +
                "etnia = @etnia, certificacoes = @certificacoes, escolaridade = @escolaridade, " +
                "dataAdmissao = @dataAdmissao, dataDemissao = @dataDemissao, cargoId = @cargoId) " +
                "WHERE funcionarioId = @funcionarioId";

            using (SqlCommand command = new SqlCommand(sql, objConexao.sqlConnection))
            {
                command.Parameters.AddWithValue("@primeiroNome", funcionario.primeiroNome.Trim());
                command.Parameters.AddWithValue("@ultimoNome", funcionario.ultimoNome?.Trim());
                command.Parameters.AddWithValue("@nomeSocial", funcionario.nomeSocial?.Trim());
                command.Parameters.AddWithValue("@rg", funcionario.rg?.Trim());
                command.Parameters.AddWithValue("@cpf", funcionario.cpf?.Trim());
                command.Parameters.AddWithValue("@dataNasc", funcionario.dataNasc);
                command.Parameters.AddWithValue("@telefone", funcionario.telefone);
                command.Parameters.AddWithValue("@email", funcionario.email);
                command.Parameters.AddWithValue("@pcd", funcionario.pcd);
                command.Parameters.AddWithValue("@email", funcionario.email);
                command.Parameters.AddWithValue("@etnia", funcionario.etnia);
                command.Parameters.AddWithValue("@certificacoes", funcionario.certificacoes);
                command.Parameters.AddWithValue("@escolaridade", funcionario.escolaridade);
                command.Parameters.AddWithValue("@dataAdmissao", funcionario.dataAdmissao);
                command.Parameters.AddWithValue("@dataDemissao", funcionario.dataDemissao);
                command.Parameters.AddWithValue("@cargoId", funcionario.cargoId);

                objConexao.ExecutarComandoSql(command, false);
            }
        }

        private static Funcionario CarregaObjeto(IFormCollection collection)
        {
            Funcionario funcionario = new Funcionario()
            {
            primeiroNome = collection["primeiroNome"].ToString(),
                ultimoNome = collection["ultimoNome"].ToString(),
                nomeSocial = collection["nomeSocial"].ToString(),
                rg = collection["rg"].ToString(),
                cpf = collection["cpf"].ToString(),
                endereco = Convert.ToInt32(collection["endereco"]),
                telefone = collection["telefone"].ToString(),
                email = collection["email"].ToString(),
                etnia = collection["etnia"].ToString(),
                certificacoes = collection["certificacoes"].ToString(),
                escolaridade = collection["escolaridade"].ToString(),
                cargoId = Convert.ToInt32(collection["cargoId"]),
                // promocoes = float.Parse(collection["promocoes"])
            };

            funcionario.dataNasc = ConverteData(collection["dataNasc"].ToString());
            funcionario.dataAdmissao = ConverteData(collection["dataAdmissao"].ToString());
            funcionario.dataDemissao = ConverteData(collection["dataDemissao"].ToString());

            if (collection["pcd"].Count > 1)
            {
                funcionario.pcd = true;
            }

            return funcionario;
        }

        private static Funcionario CarregaObjeto(DataRow dataRow)
        {
            Funcionario funcionario = new Funcionario()
            {
                funcionarioId = Convert.ToInt32(dataRow["funcionarioId"]),
                primeiroNome = dataRow["primeiroNome"]?.ToString(),
                ultimoNome = dataRow["ultimoNome"]?.ToString(),
                nomeSocial = dataRow["nomeSocial"]?.ToString(),
                rg = dataRow["rg"]?.ToString(),
                cpf = dataRow["cpf"]?.ToString(),
                endereco = Convert.ToInt32(dataRow["endereco"]),
                telefone = dataRow["telefone"]?.ToString(),
                email = dataRow["email"]?.ToString(),
                etnia = dataRow["etnia"]?.ToString(),
                certificacoes = dataRow["certificacoes"]?.ToString(),
                escolaridade = dataRow["escolaridade"]?.ToString(),
                //cargoId = Convert.ToInt32(dataRow["cargoId"]),
            };

            funcionario.dataNasc = ConverteData(dataRow, "dataNasc");
            funcionario.dataAdmissao = ConverteData(dataRow, "dataAdmissao");
            funcionario.dataDemissao = ConverteData(dataRow, "dataDemissao");

           /* if (dataRow["dataNasc"] != DBNull.Value)
            {
                funcionario.dataNasc = Convert.ToDateTime(dataRow["dataNasc"]);
            }

            if (dataRow["dataAdmissao"] != DBNull.Value)
            {
                funcionario.dataAdmissao = Convert.ToDateTime(dataRow["dataAdmissao"]);
            }

            if (dataRow["dataDemissao"] != DBNull.Value)
            {
                funcionario.dataDemissao = Convert.ToDateTime(dataRow["dataDemissao"]);
            }*/

            if (dataRow["pcd"] != DBNull.Value)
            {
                funcionario.pcd = Convert.ToBoolean(dataRow["pcd"]);
            }

            return funcionario;
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

        private static bool ConverteBool(IFormCollection collection, string nomeCampo)
        {
            if (collection.TryGetValue(nomeCampo, out var pcdValue))
            {
                if (bool.TryParse(pcdValue, out var valorBool))
                {
                    if (valorBool)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

       /* public static List<AtestadoMedico> GetAtestadosMedicos(int idFuncionario)
        {
            Funcionario funcionario = new Funcionario();
            funcionario.listaAtestadoMedico = new List<AtestadoMedico>();//ClasseConexaoGET(ComandoSQL GetAtestadoMedico_idFuncionario);
            return funcionario.listaAtestadoMedico;
        }

        public static bool SetAtestadoMedico(int idFuncionario, List<AtestadoMedico> listaAtestadoMedico, out string mensagemRetorno)
        {
            Funcionario funcionario = new Funcionario();

            //validacao da lista
            if (funcionario.InsertListaAtestadoMedico(listaAtestadoMedico, out mensagemRetorno))
            {
                //retorno positivo OK - Mensagem sucesso
                return true;
            }

            else
            {
                // retorno negativo Erro - Mensagem erro
                return false;
            }
        }

        public static bool SetAtestadoMedico(int idFuncionario, AtestadoMedico AtestadoMedico, out string mensagemRetorno)
        {

            Funcionario funcionario = new Funcionario();
            List<AtestadoMedico> listaAtestadoMedico = new List<AtestadoMedico>();
            listaAtestadoMedico.Add(AtestadoMedico);

            //validacao da lista
            if (funcionario.InsertListaAtestadoMedico(listaAtestadoMedico, out mensagemRetorno))
            {
                //retorno positivo OK - Mensagem sucesso
                return true;
            }

            else
            {
                // retorno negativo Erro - Mensagem erro
                return false;
            }
        }

        private bool InsertListaAtestadoMedico(List<AtestadoMedico> listaAtestadoMedico, out string mensagemRetorno)
        {
            try
            {
                for (int i = 0; i < listaAtestadoMedico.Count; i++)
                {
                    //insert sql idFuncionario => funcionario.listaAtestadoMedico[i];
                }

                mensagemRetorno = "Registros OK";

                return true;
            }

            catch (Exception ex)
            {
                mensagemRetorno = $"Erro ao registrar tal: {ex.Message}";

                return false;
            }
        }

        public static void GetCertificacoes(int idFuncionario)
        {

        }

        public static void SetCertificacao(int idFuncionario, Certificacoes certificacao)
        {

        }

        public void PensaoDevida(bool value, int dependentes, float valor)
        {

        }*/
    }
}