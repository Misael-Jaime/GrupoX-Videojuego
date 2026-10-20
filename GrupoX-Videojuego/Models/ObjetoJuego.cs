using System.Drawing;

namespace GrupoX_Videojuego.Models
{
    // Clase abstracta base para todos los objetos que se renderizan en el juego
    public abstract class ObjetoJuego
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Color Color { get; set; }

        public ObjetoJuego(float x, float y, float width, float height, Color color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
        }

        // ABSTRACCION - cada hijo define como se actualiza
        public abstract void Update();
        // ABSTRACCION - cada hijo define como se dibuja
        public abstract void Draw(Graphics g);

        // Devuelve un rectangulo con la posicion y tamaño del objeto
        public RectangleF GetBounds()
        {
            return new RectangleF(X, Y, Width, Height);
        }

        // Detecta colision entre dos objetos usando interseccion de rectangulos
        public bool IntersectsWith(ObjetoJuego other)
        {
            return this.GetBounds().IntersectsWith(other.GetBounds());
        }
    }
}
