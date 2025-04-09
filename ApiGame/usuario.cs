namespace ApiGame;
    public class Usuario
    {
        public string nombre { get; set; }
        public string apellido_pat { get; set; }
        public string apellido_mat { get; set; }
        public string correo { get; set; }

        public Usuario(string nombre_, string apellido_pat_, string apellido_mat_, string correo_)
        {
            this.nombre = nombre_;
            this.apellido_pat = apellido_pat_;
            this.apellido_mat = apellido_mat_;
            this.correo = correo_;
        }

        public Usuario() {}
    }
