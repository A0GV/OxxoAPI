namespace ApiGame;

public class dialogo
{
	public int id_dialogo { get; set; }
	public int id_npc { get; set; }
	public string respuesta { get; set; }

	public dialogo(int id_dialogo_, int id_npc_, string respuesta_)
	{
		this.id_dialogo = id_dialogo_;
		this.id_npc = id_npc_;
		this.respuesta = respuesta_;
	}

	public dialogo(){}
}