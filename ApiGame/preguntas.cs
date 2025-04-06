namespace ApiGame;

public class preguntas
{
	public int id_pregunta {get;set;}
	public string pregunta {get;set;}
	public int id_npc {get;set;}

	public preguntas(int id_pregunta_,string pregunta_,int id_npc_)
	{
		this.id_pregunta = id_pregunta_;
		this.pregunta = pregunta_;
		this.id_npc = id_npc_;
	}

    public preguntas(){}
}