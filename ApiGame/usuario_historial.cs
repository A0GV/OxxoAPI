namespace ApiGame;

public class usuario_historial
{
    public int id_usuario { get; set; }
    public int id_historial { get; set; }

    public usuario_historial() {}

    public usuario_historial(int id_usuario, int id_historial)
    {
        this.id_usuario = id_usuario;
        this.id_historial = id_historial;
    }
}