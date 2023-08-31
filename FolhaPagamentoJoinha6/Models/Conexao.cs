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
        private SqlTransaction transaction;

        public Conexao()
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        public void ExecutarComandoSql(SqlCommand command, bool isTransaction)
        {
            command.Connection = sqlConnection;
            command.ExecuteNonQuery();

            if (!isTransaction)
            {
                sqlConnection.Close();
            }
        }

        public void ExecutarComandoSql(SqlCommand command, out int newID, bool isTransaction)
        {
            command.Connection = sqlConnection;
            newID = Convert.ToInt32(command.ExecuteScalar());
            
            if (!isTransaction)
            {
                sqlConnection.Close();
            }
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

        public SqlTransaction ConexaoBegin()
        {
            //sqlConnection = new SqlConnection(connectionString);
            //sqlConnection.Open();

            return transaction = sqlConnection.BeginTransaction();
        }

        public void Commit(SqlTransaction transaction, SqlConnection sqlConnection)
        {
            transaction.Commit();
            sqlConnection.Close();
        }

        public void Rollback(SqlTransaction transaction, SqlConnection sqlConnection)
        {
            transaction.Rollback();
            sqlConnection.Close();
        }
    }
}
