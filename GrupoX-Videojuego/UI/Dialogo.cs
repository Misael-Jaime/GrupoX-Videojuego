using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GrupoX_Videojuego.UI
{
    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Dialogo : Base
    {
        private string _message;
        private string _titleText;

        public Dialogo(string message, string title)
        {
            _message = message;
            _titleText = title;
            ClientSize = new Size(400, 200);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;

            DoubleBuffered = true;
            BackColor = Color.FromArgb(8, 5, 25);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            var btn = new Button
            {
                Text = "ACEPTAR",
                Font = new Font("Impact", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 20, 80),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 40),
                Location = new Point((ClientSize.Width - 120) / 2, ClientSize.Height - 60)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => Close();
            Controls.Add(btn);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            using (var pen = new Pen(Color.FromArgb(100, 150, 255), 2))
                g.DrawRectangle(pen, 1, 1, W - 2, H - 2);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            using (var fTitle = new Font("Impact", 16))
            using (var bTitle = new SolidBrush(Color.Gold))
            {
                var sTitle = g.MeasureString(_titleText, fTitle);
                g.DrawString(_titleText, fTitle, bTitle, W / 2f - sTitle.Width / 2f, 15);
            }

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            using (var fMsg = new Font("Segoe UI", 12, FontStyle.Bold))
            using (var bMsg = new SolidBrush(Color.White))
            {
                var sMsg = g.MeasureString(_message, fMsg, W - 40);
                var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(_message, fMsg, bMsg, new RectangleF(20, 50, W - 40, H - 120), stringFormat);
            }
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        public static void Show(string message, string title)
        {
            using (var msgBox = new Dialogo(message, title))
            {
                msgBox.ShowDialog();
            }
        }
    }
}