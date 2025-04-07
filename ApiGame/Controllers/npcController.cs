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
}