using MercadoCampesinoo.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MercadoCampesinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public EventoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSql");
        }
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Evento> lista = new List<Evento>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))

                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LISTAR_EVENTO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Evento
                            {
                                idEvento = Convert.ToInt32(rd["ID_EVENTO"]),
                                nombre = rd["NOMBRE"].ToString(),
                                descripcion = rd["DESCRIPCION"].ToString(),
                                tipo = rd["TIPO"].ToString(),
                            });
                        }
                    }

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception error)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message }));
            }
        }
        [HttpGet]
        [Route("obtener/{idPersona:int}")]
        public IActionResult Obtener(int idPersona)
        {
            List<Persona> lista = new List<Persona>();
            Persona persona = new Persona();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listarPersona", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Persona
                            {
                                idPersona = Convert.ToInt32(rd["idPersona"]),
                                nombre = rd["nombre"].ToString(),
                                apellido = rd["apellido"].ToString(),
                                edad = Convert.ToInt32(rd["edad"]),
                                ubicacion = rd["ubicacion"].ToString(),
                                correo = rd["correo"].ToString(),
                                fechaNacimiento = Convert.ToDateTime(rd["fechaNacimiento"])
                            });
                        }
                    }
                }
                persona = lista.Where(item => item.idPersona == idPersona).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = persona });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = persona });
            }
        }
        [HttpPost]
        [Route("Registrar")]
        public IActionResult Registrar([FromBody] Persona objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ingresarPersona", conexion);
                    cmd.Parameters.AddWithValue("idPersona", objeto.idPersona);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("apellido", objeto.apellido);
                    cmd.Parameters.AddWithValue("edad", objeto.edad);
                    cmd.Parameters.AddWithValue("ubicacion", objeto.ubicacion);
                    cmd.Parameters.AddWithValue("correo", objeto.correo);
                    cmd.Parameters.AddWithValue("fechaNacimiento", objeto.fechaNacimiento);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "registrado" });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]

        public IActionResult Editar([FromBody] Evento objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_EDITAR_EVENTO", conexion);
                    cmd.Parameters.AddWithValue("ID_EVENTO", objeto.idEvento == 0 ? DBNull.Value : objeto.idEvento);
                    cmd.Parameters.AddWithValue("NOMBRE", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("DESCRIPCION", objeto.descripcion is null ? DBNull.Value : objeto.descripcion);
                    cmd.Parameters.AddWithValue("TIPO", objeto.tipo is null ? DBNull.Value : objeto.tipo);
                }
                return StatusCode(StatusCodes.Status200OK, new { mensage = "editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpDelete]
        [Route("Eliminar/{ID_EVENTO:int}")]
        public IActionResult Eliminar(int idEvento)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_ELIMINAR_CLIENTE", conexion);
                    cmd.Parameters.AddWithValue("ID_EVENTO", idEvento);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
