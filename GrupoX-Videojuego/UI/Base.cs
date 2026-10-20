using System.Drawing;
using System.Windows.Forms;

namespace GrupoX_Videojuego.UI
{
    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Base : Form
    {
        private Point _dragStart;
        private bool _dragging;

        public Base()
        {
            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { _dragging = true; _dragStart = e.Location; } };
            MouseMove += (s, e) => { if (_dragging) Location = new Point(Left + e.X - _dragStart.X, Top + e.Y - _dragStart.Y); };
            MouseUp += (s, e) => { _dragging = false; };
            DoubleBuffered = true;
        }
    }
}