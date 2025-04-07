namespace ApiGame
{
    public class Enemigos
    {
        public int id_preguntaenemigo { get; set; } // ID único de la pregunta
        public string textoPregunta { get; set; } // Texto de la pregunta
        public string[] opciones { get; set; } // Opciones de respuesta
        public int respuestaCorrecta { get; set; } // Índice de la respuesta correcta (0-3)

        public Enemigos() {}

        public Enemigos(int id_preguntaenemigo, string textoPregunta, string[] opciones, int respuestaCorrecta)
        {
            this.id_preguntaenemigo = id_preguntaenemigo;
            this.textoPregunta = textoPregunta;
            this.opciones = opciones;
            this.respuestaCorrecta = respuestaCorrecta;
        }
    }
}