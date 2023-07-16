using System.Data.SqlClient;

namespace Lista_de_tarefas_of
{
    public class DaoConnection
    {
        public static SqlConnection GetConexao()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=localhost;Initial Catalog=Treinamento;Integrated Security=True;MultipleActiveResultSets=true;");
            connection.Open();
            return connection;
        }
    }
}
