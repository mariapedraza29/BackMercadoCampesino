using MercadoCampesinoo.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MercadoCampesino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
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
            List<Cliente> lista = new List<Cliente>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))

                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_Cliente", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Cliente
                            {
                                idCliente = Convert.ToInt32(rd["id_cliente"]),
                                nombre = rd["nombre"].ToString(),
                                apellido = rd["apellido"].ToString(),
                                telefono = rd["Telefono"].ToString(),
                                correo = rd["correo"].ToString(),
                                direccion = rd["direccion"].ToString(),
                                contraseña = rd["CONTRASENA"].ToString() ?? "undefined"
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

        [HttpPost]
        [Route("/Registrar")]
        public IActionResult Registrar([FromBody] Cliente objeto)
        {

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_AGREGAR_CLIENTE", conexion);
                    cmd.Parameters.AddWithValue("id_cliente", objeto.idCliente);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("apellido", objeto.apellido);
                    cmd.Parameters.AddWithValue("telefono", objeto.telefono);
                    cmd.Parameters.AddWithValue("correo", objeto.correo);
                    cmd.Parameters.AddWithValue("contrasena", objeto.contraseña);
                    cmd.Parameters.AddWithValue("direccion", objeto.direccion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "el usuario se ha registrado correctamente" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensage = error.Message, mensaje ="Usuario ya existe" });
            }
        }
        [HttpPut]
        [Route("Editar")]

        public IActionResult Editar([FromBody] Cliente objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_Cliente", conexion);
                    cmd.Parameters.AddWithValue("id_Cliente", objeto.idCliente == 0 ? DBNull.Value : objeto.idCliente);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("apellido", objeto.apellido is null ? DBNull.Value : objeto.apellido);
                    cmd.Parameters.AddWithValue("telefono", objeto.telefono is null ? DBNull.Value : objeto.telefono);
                    cmd.Parameters.AddWithValue("correo", objeto.correo is null ? DBNull.Value : objeto.correo);
                    cmd.Parameters.AddWithValue("contraseña", objeto.contraseña is null ? DBNull.Value : objeto.contraseña);
                    cmd.Parameters.AddWithValue("direccion", objeto.direccion is null ? DBNull.Value : objeto.direccion);
                }
                return StatusCode(StatusCodes.Status200OK, new { mensage = "editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpDelete]
        [Route("Eliminar/{id_Cliente:int}")]
        public IActionResult Eliminar(int id_Cliente)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_Cliente", conexion);
                    cmd.Parameters.AddWithValue("id_Cliente", id_Cliente);
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

