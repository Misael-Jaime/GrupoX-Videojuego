using System.Drawing;

namespace GrupoX_Videojuego.Models
{
    // HERENCIA - hereda de ObjetoJuego
    // Representa un pincho u obstaculo que mata al jugador al tocarlo
    public class Obstaculo : ObjetoJuego
    {
        public float Speed { get; set; }

        // Tamano visual separado del hitbox para mejor aspecto
        public float VisualX { get; set; }
        public float VisualY { get; set; }
        public float VisualWidth { get; set; }
        public float VisualHeight { get; set; }

        public Obstaculo(float x, float y, float width, float height, Color color, float speed, 
            float visualX = 0, float visualY = 0, float visualWidth = 0, float visualHeight = 0)
            : base(x, y, width, height, color)
        {
            Speed = speed;
            // Si no se define tamano visual, usa el mismo que el hitbox
            VisualX = visualX == 0 ? x : visualX;
            VisualY = visualY == 0 ? y : visualY;
            VisualWidth = visualWidth == 0 ? width : visualWidth;
            VisualHeight = visualHeight == 0 ? height : visualHeight;
        }

        // Se mueve hacia la izquierda en modo procedural
        public override void Update()
        {
            X -= Speed;
            VisualX -= Speed;
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        public override void Draw(Graphics g)
        {
            PointF[] points = new PointF[]
            {
                new PointF(VisualX + VisualWidth / 2, VisualY),
                new PointF(VisualX, VisualY + VisualHeight),
                new PointF(VisualX + VisualWidth, VisualY + VisualHeight)
            };

            PointF[] innerPoints = new PointF[]
            {
                new PointF(VisualX + VisualWidth / 2, VisualY + 6),
                new PointF(VisualX + 5, VisualY + VisualHeight - 4),
                new PointF(VisualX + VisualWidth - 5, VisualY + VisualHeight - 4)
            };

            using (Brush brush = new SolidBrush(Color))
                g.FillPolygon(brush, points);

            using (Brush highlight = new SolidBrush(System.Drawing.Color.FromArgb(80, 255, 255, 255)))
                g.FillPolygon(highlight, innerPoints);

            using (Pen pen = new Pen(System.Drawing.Color.FromArgb(200, 255, 80, 80), 2))
                g.DrawPolygon(pen, points);
        }
    }
}