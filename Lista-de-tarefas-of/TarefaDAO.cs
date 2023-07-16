using System.Data.SqlClient;
using System.Text;

namespace Lista_de_tarefas_of
{
    public class TarefaDAO
    {
        private SqlConnection Connection { get; }
        public TarefaDAO(SqlConnection connection)
        {
            Connection = connection;
        }
        public void Salvar(TarefaModel tarefa)
        {
            using (SqlCommand command = Connection.CreateCommand())
            {
                SqlTransaction t = Connection.BeginTransaction();
                try
                {
                    //Excluir(codAutor, t);

                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine($"INSERT INTO TarefasOficial(tarefa, situacao) VALUES(@tarefa, @situacao)");
                    command.CommandText = sql.ToString();
                    command.Parameters.Add(new SqlParameter("@tarefa", tarefa.titulo));
                    command.Parameters.Add(new SqlParameter("@situacao", tarefa.situacao));
                    command.Transaction = t;
                    command.ExecuteNonQuery();
                    t.Commit();
                }

                catch (Exception ex)
                {
                    t.Rollback();
                    throw ex;
                }
            }
        }
        public void Alterar(TarefaModel tarefa)
        {
            using (SqlCommand command = Connection.CreateCommand())
            {
                SqlTransaction t = Connection.BeginTransaction();
                try
                {
                    //Excluir(codAutor, t);

                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine($"UPDATE TarefasOficial SET tarefa = @tarefa, situacao = @situacao WHERE codTarefa = @codTarefa");
                    command.CommandText = sql.ToString();
                    command.Parameters.AddWithValue("@codTarefa", tarefa.codTarefa);
                    command.Parameters.Add(new SqlParameter("@tarefa", tarefa.titulo));
                    command.Parameters.Add(new SqlParameter("@situacao", tarefa.situacao));
                    command.Transaction = t;
                    command.ExecuteNonQuery();
                    t.Commit();
                }

                catch (Exception ex)
                {
                    t.Rollback();
                    throw ex;
                }
            }
        }
        public List<TarefaModel> getId(TarefaModel tarefa)
        {
            List<TarefaModel> tarefas = new List<TarefaModel>();
            SqlCommand command = Connection.CreateCommand();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT codTarefa, tarefa, situacao FROM TarefasOficial WHERE codTarefa = @codTarefa");
            command.CommandText = sql.ToString();
            command.Parameters.Add(new SqlParameter("@codTarefa", tarefa.codTarefa));
            command.ExecuteNonQuery();
            using (SqlDataReader dr = command.ExecuteReader())
            {
                while (dr.Read())
                {
                    tarefas.Add(PopulateDr(dr));
                }
            }
            return tarefas;
        }
        public void Excluir(TarefaModel tarefa, SqlTransaction t = null)
        {
            using (SqlCommand command = Connection.CreateCommand())
            {
                if (t != null)
                {
                    command.Transaction = t;
                }
                StringBuilder sql = new StringBuilder();
                sql.AppendLine($"DELETE FROM TarefasOficial WHERE codTarefa=@codTarefa");
                command.CommandText = sql.ToString();
                command.Parameters.Add(new SqlParameter("@codTarefa", tarefa.codTarefa));
                command.ExecuteNonQuery();
            }
        }
        public List<TarefaModel> GetTarefas()
        {
            List<TarefaModel> tarefas = new List<TarefaModel>();
            SqlCommand command = Connection.CreateCommand();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT TAR.codTarefa, TAR.tarefa, TAR.situacao FROM TarefasOficial TAR ORDER BY TAR.codTarefa DESC");
            command.CommandText = sql.ToString();
            using (SqlDataReader dr = command.ExecuteReader())
            {
                while (dr.Read())
                {
                    tarefas.Add(PopulateDr(dr));
                }
            }
            return tarefas;
        }
        private TarefaModel PopulateDr(SqlDataReader dr)
        {
            string codTarefa = "";
            string tarefa = "";
            string situacao = "";
            if (DBNull.Value != dr["codTarefa"])
            {
                codTarefa = dr["codTarefa"] + "";
            }
            if (DBNull.Value != dr["tarefa"])
            {
                tarefa = dr["tarefa"] + "";
            }
            if (DBNull.Value != dr["situacao"])
            {
                situacao = dr["situacao"] + "";
            }
            return new TarefaModel()
            {
                codTarefa = Convert.ToInt32(codTarefa),
                titulo = tarefa,
                situacao = Convert.ToBoolean(situacao)
            };
        }
    }
}
