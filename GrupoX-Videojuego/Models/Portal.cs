using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GrupoX_Videojuego.Models
{
    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
 
    // HERENCIA - hereda de ObjetoJuego
    // Portal que cambia al jugador entre modo cubo y modo nave
    public class Portal : ObjetoJuego
    {
        public ModoJugador TargetMode { get; set; }
        public float Speed { get; set; }
        private float _animOffset = 0f;

        public Portal(float x, float y, float width, float height, Color color, ModoJugador targetMode, float speed)
            : base(x, y, width, height, color)
        {
            TargetMode = targetMode;
            Speed = speed;
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        public override void Update()
        {
            X -= Speed;
            _animOffset += 0.08f;
        }
 
        public override void Draw(Graphics g)
        {
            float arcW = Width * 2f;
            float arcH = Height;
            float arcX = X - Width * 0.5f;
            float arcY = Y;
          
            for (int i = 4; i >= 1; i--)
            {
                int alpha = 18 * i;
                using (var glowBrush = new SolidBrush(Color.FromArgb(alpha, Color)))
                using (var glowPath = BuildArchPath(arcX - i * 3, arcY + i, arcW + i * 6, arcH - i))
                    g.FillPath(glowBrush, glowPath);
            }

            using (var fillPath = BuildArchPath(arcX, arcY + 2, arcW, arcH - 2))
            {
                using (var fillBrush = new LinearGradientBrush(
                    new PointF(arcX, arcY),
                    new PointF(arcX + arcW, arcY),
                    Color.FromArgb(60, Color),
                    Color.FromArgb(120, Color)))
                    g.FillPath(fillBrush, fillPath);
            }

            int rings = 5;
            for (int i = 0; i < rings; i++)
            {
                float phase = (_animOffset + i * (1f / rings)) % 1f;
                float shrink = phase * 0.45f;
                float rX = arcX + arcW * shrink * 0.5f;
                float rW = arcW * (1f - shrink);
                float rY = arcY + arcH * shrink * 0.35f;
                float rH = (arcH - 4) * (1f - shrink * 0.7f);
                int alpha = (int)(200 * (1f - phase));

                using (var ringPath = BuildArchPath(rX, rY + 2, rW, rH - 2))
                using (var ringPen = new Pen(Color.FromArgb(alpha, Color), 2f))
                    g.DrawPath(ringPen, ringPath);
            }

            using (var borderPath = BuildArchPath(arcX, arcY, arcW, arcH))
            using (var borderPen = new Pen(Color, 3f))
                g.DrawPath(borderPen, borderPath);

            using (var postPen = new Pen(Color, 4f))
            {
                g.DrawLine(postPen, arcX + 2, arcY + arcH * 0.35f, arcX + 2, arcY + arcH);
                g.DrawLine(postPen, arcX + arcW - 2, arcY + arcH * 0.35f, arcX + arcW - 2, arcY + arcH);
            }

            using (var shinePath = BuildArchPath(arcX + 2, arcY + 2, arcW - 4, arcH - 4))
            using (var shinePen = new Pen(Color.FromArgb(80, Color.White), 1.5f))
                g.DrawPath(shinePen, shinePath);

            string icon = TargetMode == ModoJugador.Ship ? "✈" : "■";
            float iconY = arcY + arcH * 0.25f;
            using (var iconFont = new Font("Segoe UI Emoji", 11, FontStyle.Regular))
            using (var iconBrush = new SolidBrush(Color.FromArgb(220, Color.White)))
            {
                var sz = g.MeasureString(icon, iconFont);
                g.DrawString(icon, iconFont, iconBrush, arcX + arcW / 2f - sz.Width / 2f, iconY);
            }
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private static GraphicsPath BuildArchPath(float x, float y, float w, float h)
        {
            float archH = w * 0.5f;
            archH = Math.Min(archH, h * 0.45f);

            var path = new GraphicsPath();
            path.AddArc(x, y, w, archH * 2f, 180, 180);
            path.AddLine(x + w, y + archH, x + w, y + h);
            path.AddLine(x + w, y + h, x, y + h);
            path.AddLine(x, y + h, x, y + archH);
            path.CloseFigure();
            return path;
        }
    }
}