    namespace ApiGame;

    public class Contrasena
    {
        public int IdContrasena { get; set; }
        public string ValorContrasena { get; set; }

        public Contrasena(int idContrasena, string valorContrasena)
        {
            this.IdContrasena = idContrasena;
            this.ValorContrasena = valorContrasena;
        }
    }