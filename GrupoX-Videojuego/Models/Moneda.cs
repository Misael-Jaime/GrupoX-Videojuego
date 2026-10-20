using System.Drawing;

namespace GrupoX_Videojuego.Models
{
    // HERENCIA - hereda de ObjetoJuego
    // Moneda coleccionable que se puede recoger en los niveles
    public class Moneda : ObjetoJuego
    {
        public bool Collected { get; set; } = false;
        private float _bobAngle = 0;

        public Moneda(float x, float y, float width, float height)
            : base(x, y, width, height, Color.Gold) { }

        public override void Update() { _bobAngle += 0.08f; }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        public override void Draw(Graphics g)
        {
            if (Collected) return;
            float bob = (float)System.Math.Sin(_bobAngle) * 3;
            using (Brush b = new SolidBrush(Color.Gold))
                g.FillEllipse(b, X, Y + bob, Width, Height);
            using (Brush inner = new SolidBrush(Color.FromArgb(180, 255, 220, 80)))
                g.FillEllipse(inner, X + 4, Y + 4 + bob, Width - 8, Height - 8);
            using (Pen pen = new Pen(Color.DarkGoldenrod, 1.5f))
                g.DrawEllipse(pen, X, Y + bob, Width, Height);
        }
    }
}