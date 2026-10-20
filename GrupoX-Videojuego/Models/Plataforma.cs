using System.Drawing;
using System.Drawing.Drawing2D;

namespace GrupoX_Videojuego.Models
{
    // HERENCIA - hereda de ObjetoJuego
    // Plataforma estatica sobre la que camina el jugador
    public class Plataforma : ObjetoJuego
    {
        public Plataforma(float x, float y, float width, float height, Color color)
            : base(x, y, width, height, color) { }

        public override void Update() { }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        public override void Draw(Graphics g)
        {
            using (var brush = new LinearGradientBrush(
                new System.Drawing.PointF(X, Y), new System.Drawing.PointF(X, Y + Height),
                Color, Color.FromArgb(Color.A, Color.R / 2, Color.G / 2, Color.B / 2)))
            {
                g.FillRectangle(brush, X, Y, Width, Height);
            }
            using (Pen pen = new Pen(Color.FromArgb(180, 255, 255, 255), 1))
                g.DrawRectangle(pen, X, Y, Width, Height);
        }
    }
}