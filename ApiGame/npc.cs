namespace ApiGame;

public class npc
{
	public int id_npc {get;set;}
	public byte es_bueno {get;set;}
	public string textoparamaladecision {get;set;}
	public string expediente {get;set;}
	public string local {get;set;}
	public int id_pregunta{get;set;}

	public npc(){}
    	public npc(int id_npc_,byte es_bueno_,string textoparamaladecision_,string expediente_,string local_, int id_pregunta_)
	{
		this.id_npc = id_npc_;
		this.es_bueno = es_bueno_;
		this.textoparamaladecision = textoparamaladecision_;
		this.expediente = expediente_;
		this.local = local_;
		this.id_pregunta=id_pregunta_;
	}
}
