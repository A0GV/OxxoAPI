namespace ApiGame;

// Unecessary
public class diasJugados
{
	public int id_usuario {get; set;}
    public int veces_jugado {get; set;}

	public diasJugados(){}
    
    public diasJugados(int id_usuario, int veces_jugado)
	{
		this.id_usuario = id_usuario;
		this.veces_jugado = veces_jugado;
	}
}
