using Microsoft.Data.SqlClient;
using System.Data;

namespace FolhaPagamentoJoinha6.Models
{
    public class Conexao
    {
        public static string server = "localhost";
        public static string user = "root";
        public static string password = "root";
        public static string database = "db_joinha";

        private string connectionString = $"Server={server};Database={database};User Id={user};Password={password};Trusted_Connection=True;Encrypt=False;";
        //private string connectionString = $"Server={server};Database={database};Trusted_Connection=True;";

        public SqlConnection sqlConnection = new SqlConnection();

        public Conexao()
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        public void ExecutarComandoSql(SqlCommand command)
        {
            command.Connection = sqlConnection;
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void ExecutarComandoSql(SqlCommand command, out int newID)
        {
            command.Connection = sqlConnection;
            newID = (int)command.ExecuteScalar();
            sqlConnection.Close();
        }

        public DataTable RetornaDataTable(string sql)
        {
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            sqlConnection.Close();

            return dt;
        }
    }
}
