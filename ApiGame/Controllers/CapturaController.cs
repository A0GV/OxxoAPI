using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ApiGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CapturaController : ControllerBase
    {
        private readonly string connectionString = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=OxxoWeb/ca.pem";

        // GET: /Captura/tiendas_usuario/{idUsuario}
        [HttpGet("tiendas_usuario/{idUsuario}")]
        public List<Tienda> GetTiendasPorUsuario(int idUsuario)
        {
            List<Tienda> tiendas = new List<Tienda>();

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();
                string query = @"
                    SELECT t.id_tienda, CONCAT(t.nombre_calle, ' #', t.num_calle) AS nombre
                    FROM tienda t
                    JOIN usuario_tienda ut ON ut.id_tienda = t.id_tienda
                    WHERE ut.id_usuario = @idUsuario;";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tienda tienda = new Tienda
                            {
                                IdTienda = reader.GetInt32("id_tienda"),
                                NombreCalle = reader.GetString("nombre")
                            };
                            tiendas.Add(tienda);
                        }
                    }
                }
            }

            return tiendas;
        }

        // GET: /Captura/irregularidades
        [HttpGet("irregularidades")]
        public List<Irregularidad> GetIrregularidades()
        {
            List<Irregularidad> irregularidades = new List<Irregularidad>();

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();
                string query = "SELECT id_irregularidad, nombre FROM irregularidad WHERE activo = 1;";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Irregularidad irregularidad = new Irregularidad
                        {
                            IdIrregularidad = reader.GetInt32("id_irregularidad"),
                            Nombre = reader.GetString("nombre")
                        };
                        irregularidades.Add(irregularidad);
                    }
                }
            }

            return irregularidades;
        }

        // POST: /Captura/reporte
        [HttpPost("reporte")]
        public string GuardarReporte([FromBody] CapturaInput input)
        {
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();
                string insertQuery = @"
                    INSERT INTO reporte_irregularidad (id_tienda, id_irregularidad, descripcion, fecha)
                    VALUES (@idTienda, @idIrregularidad, @descripcion, NOW());";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conexion))
                {
                    cmd.Parameters.AddWithValue("@idTienda", input.IdTienda);
                    cmd.Parameters.AddWithValue("@idIrregularidad", input.IdIrregularidad);
                    cmd.Parameters.AddWithValue("@descripcion", input.Descripcion);
                    cmd.ExecuteNonQuery();
                }
            }

            return "Reporte guardado con éxito";
        }
    }
}
