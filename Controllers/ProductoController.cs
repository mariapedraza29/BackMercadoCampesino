using MercadoCampesinoo.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MercadoCampesinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public ClienteController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSql");
        }
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))

                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_LISTAR_PRODUCTO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto
                            {
                                idProducto = Convert.ToInt32(rd["ID_PRODUCTO"]),
                                nombre = rd["NOMBRE"].ToString(),
                                existencia = rd["EXISTENCIA"].ToString(),
                                precio = Convert.ToInt32(rd["PRECIO"])
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

        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_EDITAR_PRODUCTO", conexion);
                    cmd.Parameters.AddWithValue("ID_PRODUCTO", objeto.idProducto == 0 ? DBNull.Value : objeto.idProducto);
                    cmd.Parameters.AddWithValue("NOMBRE", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("EXISTENCIA", objeto.existencia is null ? DBNull.Value : objeto.existencia);
                    cmd.Parameters.AddWithValue("PRECIO", objeto.precio == 0 ? DBNull.Value : objeto.precio);
                }
                return StatusCode(StatusCodes.Status200OK, new { mensage = "editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpDelete]
        [Route("Eliminar/{ID_PRODUCTO:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_ELIMINAR_PRODUCTO", conexion);
                    cmd.Parameters.AddWithValue("ID_PRODUCTO", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Producto eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
