using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;


namespace ApiGame.Controllers;
[ApiController]
[Route("controller")]
public class npcController : ControllerBase
{
    [HttpGet("GetAllnpc")]
    public IEnumerable<npc> Get()
    {
        var allDataNpc = new List<npc>();
        string conection = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";
        using var conexion = new MySqlConnection(conection);
        conexion.Open();
        MySqlCommand cmd = new MySqlCommand(
            "SELECT * from npc;", conexion);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var list = new npc(
                reader.GetInt32("id_npc"),
                reader.GetByte("es_bueno"),
                reader.GetString("textoparamaladecision"),
                reader.GetString("expediente"),
                reader.GetString("local")
            );
            allDataNpc.Add(list);
        }
        return allDataNpc;

    }
}