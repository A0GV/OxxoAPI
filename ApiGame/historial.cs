namespace ApiGame;

public class historial
{
    public int id_historial { get; set; }
    public int id_juego { get; set; }
    public DateTime fecha { get; set; }
    public int monedas { get; set; }
    public int exp { get; set; }

    public historial() {}

    public historial(int id_historial, int id_juego, DateTime fecha, int monedas, int exp)
    {
        this.id_historial = id_historial;
        this.id_juego = id_juego;
        this.fecha = fecha;
        this.monedas = monedas;
        this.exp = exp;
    }
}
