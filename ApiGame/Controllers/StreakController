using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;

// Gets number of times a player has played a specific game
namespace ApiGame.Controllers;
[ApiController]
[Route("[controller]")]
public class streakController : ControllerBase
{
    [HttpGet(Name = "GetDaysPlayed")]

    // Regresa días jugados basado en id_usuario del jugador y el id_juego que está jugando
    public int GetDiasJugado(int id_logged, int id_jugando)
    {
        npc datosBasicos = null;
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
}