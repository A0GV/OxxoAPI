using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;


namespace ApiGame.Controllers;
[ApiController]
[Route("controller")]
public class npcController : ControllerBase
{
    [HttpGet("GetAllnpc")]
    public IEnumerable<npc> Get(int id)
    {
        var allDataNpc = new List<npc>();
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();
        MySqlCommand cmd = new MySqlCommand(
            @"SELECT 
        npc.id_npc,
        npc.es_bueno,
        npc.textoparamaladecision,
        npc.expediente,
        npc.local,
        preguntas.id_pregunta,
        preguntas.pregunta,
        GROUP_CONCAT(dialogo.respuesta SEPARATOR '; ') AS respuestas
    FROM 
        npc
    LEFT JOIN 
        preguntas ON npc.id_npc = preguntas.id_npc
    LEFT JOIN 
        dialogo ON npc.id_npc = dialogo.id_npc
    WHERE 
        npc.id_npc = @npcId -- Cambia el 1 por el id_npc específico
    GROUP BY 
        preguntas.id_pregunta;", conexion);
        cmd.Parameters.AddWithValue("@npcId", id);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var list = new npc(
                reader.GetInt32("id_npc"),
                reader.GetByte("es_bueno"),
                reader.GetString("textoparamaladecision"),
                reader.GetString("expediente"),
                reader.GetString("local"),
                reader.GetInt32("id_pregunta")
            );
            allDataNpc.Add(list);
        }
        return allDataNpc;

    }
}