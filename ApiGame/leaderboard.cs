namespace ApiGame;

public class Leadearboard
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public int exp {get;set;}

    public Leadearboard(string nombre,string apellido, int expp)
    {
        this.Nombre = nombre;
        this.Apellido = apellido;
        this.exp=expp;
    }
}