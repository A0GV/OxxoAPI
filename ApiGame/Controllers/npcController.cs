using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;


namespace ApiGame.Controllers;
[ApiController]
[Route("controller")]
public class npcController : ControllerBase
{
    [HttpGet("GetNpcBasicData")]
    public npc Get(int id)
    {
        npc datosBasicos = null;
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();
        MySqlCommand cmd = new MySqlCommand(
            @"SELECT * from npc Where id_npc =@id", conexion);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            datosBasicos = new npc(
                reader.GetInt32("id_npc"),
                reader.GetByte("es_bueno"),
                reader.GetString("textoparamaladecision"),
                reader.GetString("expediente"),
                reader.GetString("local")
            );

        }
        return datosBasicos;

    }

    [HttpGet("GetPreguntas")]
    public IEnumerable<preguntas> GetPreguntas(int id)
    {

        var Lpreguntas = new List<preguntas>();
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();
        MySqlCommand cmd = new MySqlCommand(
            @"SELECT * from preguntas WHERE id_npc=@id", conexion);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var preguntas = new preguntas(
                reader.GetInt32("id_pregunta"),
                reader.GetString("pregunta"),
                reader.GetInt32("id_npc")
            );
            Lpreguntas.Add(preguntas);
        }
        return Lpreguntas;
    }

    [HttpGet("GetRespuestas")]
    public IEnumerable<dialogo> GetRespuestas(int id)
    {

        var LRespuestas = new List<dialogo>();
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();
        MySqlCommand cmd = new MySqlCommand(
            @"SELECT * from dialogo WHERE id_npc=@id", conexion);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var respuestas = new dialogo(
                reader.GetInt32("id_dialogo"),
                reader.GetInt32("id_npc"),
                reader.GetString("respuesta")
            );
            LRespuestas.Add(respuestas);
        }
        return LRespuestas;
    }

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
    public void AgregarDatosJuego([FromBody] datosJuego datos)
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