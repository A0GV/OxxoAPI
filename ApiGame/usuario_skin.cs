
namespace ApiGame;

public class usuario_skin
{
    public int id_usuario { get; set; }
    public int id_skin { get; set; }
    public int isActive { get; set; }

    public usuario_skin(int id_usuario_, int id_skin_, int isActive_)
    {
        this.id_usuario = id_usuario_;
        this.id_skin = id_skin_;
        this.isActive = isActive_;
    }
    public usuario_skin(int id_skin_,int isActive_)
    {
        this.id_skin=id_skin_;
        this.isActive = isActive_;
    }
}