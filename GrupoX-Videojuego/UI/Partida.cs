using System;
using System.Drawing;
using System.Windows.Forms;
using GrupoX_Videojuego.Core;
using GrupoX_Videojuego.Models;

namespace GrupoX_Videojuego.UI
{
    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Partida : Form
    {
        private MotorJuego _engine;
        private System.Windows.Forms.Timer _timer;
        private bool _actionPressed;
        private bool _actionPressedThisFrame;
        private double _timeAccumulator;
        private DatosNivel? _levelData;
        private DateTime _lastFrame = DateTime.Now;

        public Partida(DatosNivel? level = null)
        {
            _levelData = level;
            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            var screen = Screen.PrimaryScreen?.Bounds ?? new Rectangle(0, 0, 1100, 620);
            ClientSize = new Size(screen.Width, screen.Height);
            Location = new Point(screen.Left, screen.Top);
            StartPosition = FormStartPosition.Manual;
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            TopMost = true;
            BackColor = Color.Black;

            _engine = new MotorJuego(1100, 620);
            _engine.StartLevel(level);
            GestorAudio.PlayGameMusic();

            int fps = 60;
            _timer = new System.Windows.Forms.Timer { Interval = 1000 / fps };
            _timer.Tick += OnTick;
            _timer.Start();

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            FormClosed += (s, e) => { _timer.Stop(); GestorAudio.StopGameMusic(); SistemaGuardado.Save(); };
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void OnTick(object? s, EventArgs e)
        {
            double ms = (DateTime.Now - _lastFrame).TotalMilliseconds;
            _lastFrame = DateTime.Now;

            if (_engine.CurrentState == EstadoJuego.Playing)
            {
                _timeAccumulator += ms;
                if (_timeAccumulator > 100) _timeAccumulator = 100;

                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                while (_timeAccumulator >= 16.6667)
                {
                    bool input = _actionPressed || _actionPressedThisFrame;
                    _engine.Update(input, 16.6667f);
                    _actionPressedThisFrame = false;
                    _timeAccumulator -= 16.6667;
                }
            }
            else
            {
                _engine.Update(_actionPressed, (float)ms);
            }
            Invalidate();
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;

            g.FillRectangle(Brushes.Black, 0, 0, ClientSize.Width, ClientSize.Height);

            float scaleX = (float)ClientSize.Width / _engine.InternalWidth;
            float scaleY = (float)ClientSize.Height / _engine.InternalHeight;

            g.ScaleTransform(scaleX, scaleY);
            _engine.Draw(g, _engine.InternalWidth, _engine.InternalHeight);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void OnKeyDown(object? s, KeyEventArgs e)
        {
            if (e.KeyCode is Keys.Space or Keys.Up or Keys.W or Keys.Enter)
            {
                if (_engine.CurrentState == EstadoJuego.Paused) { _engine.CurrentState = EstadoJuego.Playing; return; }
                _actionPressed = true;
                _actionPressedThisFrame = true;
                HandleAction();
            }
            if (e.KeyCode == Keys.Escape)
            {
                if (_engine.CurrentState == EstadoJuego.Playing) _engine.CurrentState = EstadoJuego.Paused;
                else if (_engine.CurrentState == EstadoJuego.Paused) _engine.CurrentState = EstadoJuego.Playing;
                else GoBack();
            }
            if (e.KeyCode == Keys.Q && _engine.CurrentState == EstadoJuego.Paused) GoBack();
            if (e.KeyCode == Keys.R && _engine.CurrentState != EstadoJuego.Playing && _engine.CurrentState != EstadoJuego.Paused) Restart();
        }

        private void OnKeyUp(object? s, KeyEventArgs e)
        {
            if (e.KeyCode is Keys.Space or Keys.Up or Keys.W or Keys.Enter) _actionPressed = false;
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void OnMouseDown(object? s, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { _actionPressed = true; _actionPressedThisFrame = true; HandleAction(); }
        }

        private void OnMouseUp(object? s, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _actionPressed = false;
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void HandleAction()
        {
            if (_engine.CurrentState == EstadoJuego.GameOver) Restart();
            if (_engine.CurrentState == EstadoJuego.LevelComplete) GoBack();
            if (_engine.CurrentState == EstadoJuego.Menu) _engine.StartLevel(_levelData);
        }

        private void Restart() { _engine.StartLevel(_levelData); GestorAudio.PlayGameMusic(); }
        private void GoBack() { _timer.Stop(); SistemaGuardado.Save(); GestorAudio.PlayMenuMusic(); new Mundos().Show(); Close(); }
    }
}