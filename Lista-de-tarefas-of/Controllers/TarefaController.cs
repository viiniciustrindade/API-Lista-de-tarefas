using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Runtime.Versioning;

namespace Lista_de_tarefas_of.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {

        [HttpGet]
        [Route("/tarefas")]
        public ActionResult GetAutores()
        {
            using (SqlConnection connection = DaoConnection.GetConexao())
            {
                try
                {
                    TarefaDAO dao = new TarefaDAO(connection);
                    List<TarefaModel> tarefas = dao.GetTarefas();
                    return Ok(tarefas);

                }
                catch (Exception e)
                {
                    BadRequest("Erro ao obter tarefas!: " + e.Message);
                    throw;
                }
            }
        }
        [HttpGet]
        [Route("/tarefas/{codTarefa}")]
        public ActionResult GetId(int codTarefa)
        {
            using (SqlConnection connection = DaoConnection.GetConexao())
            {
                try
                {
                    TarefaDAO dao = new TarefaDAO(connection);
                    TarefaModel tarefa = new TarefaModel { codTarefa = codTarefa };
                    List<TarefaModel> tarefas = dao.getId(tarefa);

                    if (tarefas.Count > 0)
                    {
                        return Ok(tarefas[0]);
                    }
                    else
                    {
                        return NotFound(); // Retorna 404 se a tarefa não for encontrada
                    }
                }
                catch (Exception e)
                {
                    return BadRequest("Erro ao obter tarefa: " + e.Message);
                }
            }
        }
        [HttpPost]
        [Route("/tarefas")]
        public ActionResult CreateTeste(string titulo, bool situacao)
        {
            using (SqlConnection connection = DaoConnection.GetConexao())
            {
                TarefaDAO dao = new TarefaDAO(connection);
                TarefaModel taf = new TarefaModel
                {
                    titulo = titulo,
                    situacao = situacao
                };
                dao.Salvar(taf);
                return new StatusCodeResult(201);
            }


        }
        [HttpPut]
        [Route("/tarefas/{codTarefa}/update")]
        public ActionResult UpdateTarefa([FromRoute] int codTarefa, [FromBody] TarefaUpdateModel tarefa)
        {
            using (SqlConnection connection = DaoConnection.GetConexao())
            {
                TarefaDAO dao = new TarefaDAO(connection);
                TarefaModel taf = new TarefaModel
                {
                    codTarefa = codTarefa,
                    titulo = tarefa.titulo,
                    situacao = tarefa.situacao
                };
                dao.Alterar(taf);

                return Ok(taf);
            }
        }
        [HttpDelete]
        [Route("/tarefas/{codTarefa}/delete")]
        public ActionResult DeleteTarefa([FromRoute] int codTarefa)
        {

            using (SqlConnection connection = DaoConnection.GetConexao())
            {
                TarefaDAO dao = new TarefaDAO(connection);
                dao.Excluir(new TarefaModel { codTarefa = codTarefa });


                return NoContent();
            }
        }
    }
}