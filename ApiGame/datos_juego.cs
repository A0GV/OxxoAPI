namespace ApiGame;

// Datos de jugada
public class datosJuego
{
    public int id_usuario { get; set; }
    public int id_juego { get; set; }
    public int monedas { get; set; }
    public int exp { get; set; }

    public datosJuego() {}

    public datosJuego(int id_usuario, int id_juego, int monedas, int exp)
    {
        this.id_usuario = id_usuario;
        this.id_juego = id_juego;
        this.monedas = monedas;
        this.exp = exp;
    }
}
