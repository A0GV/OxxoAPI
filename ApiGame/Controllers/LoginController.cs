using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ApiGame.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly string _connectionString = "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ApiGame/ca.pem";

    [HttpGet(Name = "VerifyPassAndUser")]
    public ActionResult<int?> Get(string user, string contrasena)
    {
        int? userId = CheckCredentials(user, contrasena);
        if (userId.HasValue)
        {
            return Ok(userId);
        }
        return Unauthorized("Invalid credentials");
    }

    private int? CheckCredentials(string user, string contrasena)
    {
        int? userId = null;
        using (MySqlConnection conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT u.id_usuario FROM usuario u INNER JOIN contrasena c ON u.id_contrasena = c.id_contrasena WHERE u.correo = @user AND c.contrasena = @contrasena;", conexion);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@contrasena", contrasena);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    userId = reader.GetInt32("id_usuario");
                }
            }
        }
        return userId;
    }

    [HttpGet("datosSkin")]
    public usuario_skin GetSkinInfo(int userId)
    {
        usuario_skin data = null;

        using (MySqlConnection conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT id_skin, isActive FROM usuario_skin WHERE id_usuario = @userId and isActive =1;", conexion);
            cmd.Parameters.AddWithValue("@userId", userId);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    data = new(
                    reader.GetInt32("id_skin"),
                    reader.GetInt32("isActive")
                    );
                }
            }
        }
        return data;
    }

    [HttpPost("primeraCompra")]
    public void postNewCompra(int userId, int id_skin)
    {
        using (MySqlConnection conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();
            var postCompra = new MySqlCommand(
                @"Insert into usuario_skin(id_usuario, id_skin, isActive) values(@userId, @id_skin, 0);", conexion);

            postCompra.Parameters.AddWithValue("@userId", userId);
            postCompra.Parameters.AddWithValue("@id_skin", id_skin);

            postCompra.ExecuteNonQuery();

        }
    }
    [HttpPut("ACtualizar activo")]
    public void activoActual(int userId, int id_skin)
    {
        using (MySqlConnection conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();
            var cmd = new MySqlCommand(@"Update usuario_skin SET isActive = 1 WHERE id_usuario = @userId AND id_skin = @id_skin;", conexion);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@id_skin", id_skin);
            cmd.ExecuteNonQuery();

        }

        using (MySqlConnection conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();
            var cmd = new MySqlCommand(@"Update usuario_skin SET isActive = 0 WHERE id_usuario = @userId AND id_skin = !@id_skin;", conexion);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@id_skin", id_skin);
            cmd.ExecuteNonQuery();

        }


    }
}