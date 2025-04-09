using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;

// Gets number of times a player has played a specific game
namespace ApiGame.Controllers;
[ApiController]
[Route("[controller]")]
public class manageCurrency : ControllerBase
{
    [HttpGet("GetDaysPlayed")]
    // Regresa días jugados basado en id_usuario del jugador y el id_juego que está jugando
    public int GetDiasJugado(int id_logged, int id_jugando)
    {
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();

        // Comando
        MySqlCommand cmd = new MySqlCommand(@"SELECT uh.id_usuario, COUNT(h.id_historial) AS veces_jugado
            FROM usuario_historial uh
            LEFT JOIN historial h 
            ON uh.id_historial = h.id_historial AND h.id_juego = @id_jugando
            WHERE uh.id_usuario = @id_logged
            GROUP BY uh.id_usuario;", conexion);
        
        // Inyecta parametros 
        cmd.Parameters.AddWithValue("@id_jugando", id_jugando);
        cmd.Parameters.AddWithValue("@id_logged", id_logged);
        
        using var reader = cmd.ExecuteReader();
        int vecesJugado = 0; // Temporary sets to 0

        // Retrieves veces_jugado
        if (reader.Read())
        {
            vecesJugado = reader.GetInt32("veces_jugado");
        }

        return vecesJugado; 
    }

    [HttpPost("AgregarDatosJuego")]
    // Agrega datos del juego usando el usuario activo
    public void AgregarDatosJuego([FromForm] datosJuego datos)
    {
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();

       
            // Insert historial
            var cmdHistorial = new MySqlCommand(@"
                INSERT INTO historial (id_juego, fecha, monedas, exp)
                VALUES (@id_juego, NOW(), @monedas, @exp);", conexion);

            cmdHistorial.Parameters.AddWithValue("@id_juego", datos.id_juego);
            cmdHistorial.Parameters.AddWithValue("@monedas", datos.monedas);
            cmdHistorial.Parameters.AddWithValue("@exp", datos.exp);
            cmdHistorial.ExecuteNonQuery();

            // Get last id_historial
            var cmdLastId = new MySqlCommand("SELECT LAST_INSERT_ID();", conexion);
            int id_historialCALC = Convert.ToInt32(cmdLastId.ExecuteScalar());

            // Usar id_historial para insert into usuario_historial
            var cmdUH = new MySqlCommand(@"
                INSERT INTO usuario_historial(id_usuario, id_historial)
                VALUES (@id_usuario, @id_historial);", conexion);

            cmdUH.Parameters.AddWithValue("@id_usuario", datos.id_usuario);
            cmdUH.Parameters.AddWithValue("@id_historial", id_historialCALC);
            cmdUH.ExecuteNonQuery();
        
        conexion.Close();
    }

    [HttpGet("GetElotesTotal")]
    // Regresa elotes totales de usuario, y 0 si no ha jugado antes
    public int GetElotesTotal(int id_logged)
    {
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();

        // Comando
        MySqlCommand cmd = new MySqlCommand(@"
            SELECT u.id_usuario, IFNULL(SUM(h.monedas), 0) AS elotes_totales
            FROM usuario u
            LEFT JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
            LEFT JOIN historial h ON uh.id_historial = h.id_historial
            WHERE u.id_usuario = @id_logged
            GROUP BY u.id_usuario;", conexion);
        
        // Inyecta parametro de usuario para buscarlo
        cmd.Parameters.AddWithValue("@id_logged", id_logged);
        
        using var reader = cmd.ExecuteReader();
        int elotesTotales = 0; // Temporary sets to 0

        // Retrieves veces_jugado
        if (reader.Read())
        {
            elotesTotales = reader.GetInt32("elotes_totales");
        }

        return elotesTotales; 
    }

    [HttpGet("GetExpTotal")]
    // Regresa exp de usuario y 0 si no ha jugado antes
    public int GetExpTotal(int id_logged)
    {
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();

        // Comando
        MySqlCommand cmd = new MySqlCommand(@"
            SELECT u.id_usuario, IFNULL(SUM(h.exp), 0) AS exp_total
            FROM usuario u
            LEFT JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
            LEFT JOIN historial h ON uh.id_historial = h.id_historial
            WHERE u.id_usuario = @id_logged
            GROUP BY u.id_usuario;", conexion);
        
        // Inyecta parametro de usuario para buscarlo
        cmd.Parameters.AddWithValue("@id_logged", id_logged);
        
        using var reader = cmd.ExecuteReader();
        int expTotal = 0; // Temporary sets to 0

        // Retrieves veces_jugado
        if (reader.Read())
        {
            expTotal = reader.GetInt32("exp_total");
        }

        return expTotal; 
    }

    [HttpGet("GetRacha")]
    // Regresa racha consecutiva
    public int GetRacha(int id_logged)
    {
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();

        // Comando
        MySqlCommand cmd = new MySqlCommand(@"
            SELECT DISTINCT DATE(h.fecha) AS fecha
            FROM usuario_historial uh
            JOIN historial h ON uh.id_historial = h.id_historial
            WHERE uh.id_usuario = @id_logged
            ORDER BY fecha DESC;", conexion);
        
        // Inyecta parametro de usuario para buscarlo
        cmd.Parameters.AddWithValue("@id_logged", id_logged);

        // Lista de las fechas 
        List<DateTime> fechas = new List<DateTime>();
        using var reader = cmd.ExecuteReader();

        // Leer datos
        while (reader.Read())
        {
            fechas.Add(Convert.ToDateTime(reader["fecha"]));
        }

        // Calcular la racha
        DateTime fechaHoy = DateTime.Today; // DateTime actual
        int racha = 0;

        // Checa fechas consecutivas
        foreach (var fecha in fechas)
        {
            // Checa si fecha temporal y la actual tienen diferencia 
            if ((fechaHoy - fecha).TotalDays == 0 || (fechaHoy - fecha).TotalDays == 1)
            {
                racha++; //Incrementa racha
                fechaHoy = fecha; // Actualizar la fecha para verificar la siguiente en la racha
            }
            else
            {
                break; // Si no es consecutiva, detener el cálculo
            }
        }

        return racha; // Regresa racha
    }

    [HttpPost("LocalAgregarDatosJuego")]
    // Agrega datos del juego usando el usuario activo
    public void LocalAgregarDatosJuego([FromBody] datosJuego datos)
    {
        string conection = "Server=127.0.0.1;Port=3306;Database=oxxo_base_e2_1;Uid=root;password=80mB*%7aEf;";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();

       
            // Insert historial
            var cmdHistorial = new MySqlCommand(@"
                INSERT INTO historial (id_juego, fecha, monedas, exp)
                VALUES (@id_juego, NOW(), @monedas, @exp);", conexion);

            cmdHistorial.Parameters.AddWithValue("@id_juego", datos.id_juego);
            cmdHistorial.Parameters.AddWithValue("@monedas", datos.monedas);
            cmdHistorial.Parameters.AddWithValue("@exp", datos.exp);
            cmdHistorial.ExecuteNonQuery();

            // Get last id_historial
            var cmdLastId = new MySqlCommand("SELECT LAST_INSERT_ID();", conexion);
            int id_historialCALC = Convert.ToInt32(cmdLastId.ExecuteScalar());

            // Usar id_historial para insert into usuario_historial
            var cmdUH = new MySqlCommand(@"
                INSERT INTO usuario_historial(id_usuario, id_historial)
                VALUES (@id_usuario, @id_historial);", conexion);

            cmdUH.Parameters.AddWithValue("@id_usuario", datos.id_usuario);
            cmdUH.Parameters.AddWithValue("@id_historial", id_historialCALC);
            cmdUH.ExecuteNonQuery();
        
        conexion.Close();
    }
}