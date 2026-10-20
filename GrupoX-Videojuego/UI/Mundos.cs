using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GrupoX_Videojuego.Core;

namespace GrupoX_Videojuego.UI
{
    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Mundos : Base
    {
        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private (string nameEs, string nameEn, int worldId, string icon, Color color)[] _worlds =
        {
            ("Mundo 1 - Inicio",      "World 1 - Start",       1, "⭐", Color.LimeGreen),
            ("Mundo 2 - Velocidad",   "World 2 - Speed",       2, "⚡", Color.DeepSkyBlue),
            ("Mundo 3 - Saltos",      "World 3 - Jumps",       3, "🌙", Color.MediumOrchid),
            ("Mundo 4 - Peligro",     "World 4 - Danger",      4, "🔥", Color.OrangeRed),
            ("Mundo 5 - Nivel Final", "World 5 - Final Level", 5, "💜", Color.Violet),
        };

        private int _hovered = -1;
        private System.Windows.Forms.Timer _t;

        public Mundos()
        {
            ClientSize = new Size(1100, 640); StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; BackColor = Color.FromArgb(8, 5, 25);
            FormBorderStyle = FormBorderStyle.None; MaximizeBox = false;
            _t = new System.Windows.Forms.Timer { Interval = 16 };
            _t.Tick += (s, e) => Invalidate();
            _t.Start();
            MouseMove  += (s, e) => { DetectHover(e.X, e.Y); Invalidate(); };
            MouseClick += (s, e) => SelectWorld(e);
            KeyDown    += (s, e) => { if (e.KeyCode == Keys.Escape) GoBack(); };
            FormClosed += (s, e) => _t.Stop();
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void GetLayoutMetrics(out int cols, out float bW, out float bH, out float padX, out float padY, out float startX, out float startY)
        {
            int total = _worlds.Length;
            cols = Math.Min(5, total);
            int rows = (int)Math.Ceiling((float)total / cols);

            bW = 190; bH = 110; padX = 25; padY = 25;
            float gridWidth = cols * bW + (cols - 1) * padX;
            float gridHeight = rows * bH + (rows - 1) * padY;

            startX = (ClientSize.Width - gridWidth) / 2f;
            startY = 110f + (ClientSize.Height - 110f - 80f - gridHeight) / 2f;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            using (var bg = new LinearGradientBrush(new PointF(0,0), new PointF(0,H),
                Color.FromArgb(8,5,25), Color.FromArgb(15,10,40)))
                g.FillRectangle(bg, 0, 0, W, H);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            using (var f = new Font("Impact", 28)) using (var b = new SolidBrush(Color.White))
            {
                string title = T.WorldMapTitle;
                var s = g.MeasureString(title, f);
                g.DrawString(title, f, b, W/2f - s.Width/2, 20);
            }
            using (var f = new Font("Segoe UI", 11)) using (var b = new SolidBrush(Color.FromArgb(150, 200, 220, 255)))
            {
                string sub = T.WorldUnlocked(SistemaGuardado.Data.HighestWorldUnlocked);
                var s = g.MeasureString(sub, f); g.DrawString(sub, f, b, W/2f - s.Width/2, 68);
            }

            GetLayoutMetrics(out int cols, out float bW, out float bH, out float padX, out float padY, out float startX, out float startY);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            for (int i = 0; i < _worlds.Length; i++)
            {
                int col = i % cols, row = i / cols;
                float x = startX + col * (bW + padX);
                float y = startY + row * (bH + padY);

                bool unlocked = _worlds[i].worldId <= SistemaGuardado.Data.HighestWorldUnlocked;
                bool hover = (i == _hovered && unlocked);

                Color c = unlocked ? _worlds[i].color : Color.FromArgb(60, 80, 80);
                Color c2 = Color.FromArgb(c.A, c.R / 2, c.G / 2, c.B / 2);

                if (hover) { x -= 3; y -= 3; bW += 6; bH += 6; }

                using (var fill = new LinearGradientBrush(new PointF(x, y), new PointF(x, y + bH), Color.FromArgb(80, c), Color.FromArgb(40, c2)))
                    g.FillRectangle(fill, x, y, bW, bH);

                Color borderC = hover ? c : Color.FromArgb(120, c);
                using (var pen = new Pen(borderC, hover ? 2.5f : 1.5f))
                    g.DrawRectangle(pen, x, y, bW, bH);

                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                using (var ef = new Font("Segoe UI Emoji", 26)) using (var eb = new SolidBrush(Color.White))
                {
                    var es = g.MeasureString(_worlds[i].icon, ef);
                    g.DrawString(_worlds[i].icon, ef, eb, x + bW/2 - es.Width/2, y + 10);
                }
                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                using (var nf = new Font("Impact", 11)) using (var nb = new SolidBrush(unlocked ? Color.White : Color.Gray))
                {
                    string worldName = SistemaGuardado.Data.Language == "en" ? _worlds[i].nameEn : _worlds[i].nameEs;
                    var parts = worldName.Split('-');
                    string n = (parts.Length > 1 ? parts[1] : parts[0]).Trim();
                    var ns = g.MeasureString(n, nf); g.DrawString(n, nf, nb, x + bW/2 - ns.Width/2, y + bH - 35);
                }
                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                if (!unlocked)
                    using (var lf = new Font("Segoe UI Emoji", 14)) using (var lb = new SolidBrush(Color.FromArgb(200, 255,255,255)))
                    {
                        var ls = g.MeasureString("🔒", lf); g.DrawString("🔒", lf, lb, x + bW/2 - ls.Width/2, y + bH - 55);
                    }

                if (hover) { bW -= 6; bH -= 6; }
            }

            using (var f = new Font("Impact", 14)) using (var b = new SolidBrush(Color.LightGray))
                g.DrawString(T.Back, f, b, 20, H - 40);
        }

        private void DetectHover(int mx, int my)
        {
            GetLayoutMetrics(out int cols, out float bW, out float bH, out float padX, out float padY, out float startX, out float startY);
            _hovered = -1;
            for (int i = 0; i < _worlds.Length; i++)
            {
                float x = startX + (i % cols) * (bW + padX), y = startY + (i / cols) * (bH + padY);
                if (mx >= x && mx <= x + bW && my >= y && my <= y + bH) { _hovered = i; break; }
            }
            Cursor = _hovered >= 0 ? Cursors.Hand : Cursors.Default;
        }

        private void SelectWorld(MouseEventArgs e)
        {
            if (_hovered < 0) return;
            var w = _worlds[_hovered];
            if (w.worldId > SistemaGuardado.Data.HighestWorldUnlocked) { Dialogo.Show(T.WorldLocked, T.WorldLockedTitle); return; }
            _t.Stop();
            new SeleccionNivel(w.worldId).Show();
            Hide();
        }

        private void GoBack() { _t.Stop(); new Menu().Show(); Close(); }
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    public class SeleccionNivel : Base
    {
        private readonly int _worldId;
        private System.Windows.Forms.Timer _t;
        private int _hovered = -1;

        public SeleccionNivel(int worldId)
        {
            _worldId = worldId;
            ClientSize = new Size(1100, 640); StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; BackColor = Color.FromArgb(8, 5, 25);
            FormBorderStyle = FormBorderStyle.None; MaximizeBox = false;
            _t = new System.Windows.Forms.Timer { Interval = 16 };
            _t.Tick += (s, e) => Invalidate(); _t.Start();
            MouseMove  += (s, e) => { DetectHover(e.X, e.Y); Invalidate(); };
            MouseClick += (s, e) => SelectLevel(e);
            KeyDown    += (s, e) => { if (e.KeyCode == Keys.Escape) GoBack(); };
            FormClosed += (s, e) => _t.Stop();
        }

        private List<DatosNivel> Levels => BaseNiveles.GetByWorld(_worldId);

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var bg = new LinearGradientBrush(new PointF(0,0), new PointF(0,H), Color.FromArgb(8,5,25), Color.FromArgb(20,10,50)))
                g.FillRectangle(bg, 0, 0, W, H);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            using (var f = new Font("Impact", 28)) using (var b = new SolidBrush(Color.White))
            {
                string worldTitle = T.WorldNum(_worldId);
                var s = g.MeasureString(worldTitle, f); g.DrawString(worldTitle, f, b, W/2f - s.Width/2, 20);
            }

            var levels = Levels;
            float bW = 200, bH = 100, padX = 30;
            float totalW = levels.Count * bW + (levels.Count - 1) * padX;
            float startX = W/2f - totalW/2f, startY = 140;

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            for (int i = 0; i < levels.Count; i++)
            {
                var lv = levels[i];
                float x = startX + i * (bW + padX);
                bool hov = i == _hovered;
                bool done = SistemaGuardado.Data.LevelCompleted.ContainsKey(lv.Id) && SistemaGuardado.Data.LevelCompleted[lv.Id];
                int best = SistemaGuardado.Data.LevelBestPercent.ContainsKey(lv.Id) ? SistemaGuardado.Data.LevelBestPercent[lv.Id] : 0;

                Color c = lv.ObjectColor;
                if (hov) { x -= 4; startY -= 4; bW += 8; bH += 8; }
                using (var fill = new LinearGradientBrush(new PointF(x, startY), new PointF(x, startY + bH), Color.FromArgb(70, c), Color.FromArgb(30, c)))
                    g.FillRectangle(fill, x, startY, bW, bH);
                using (var pen = new Pen(hov ? c : Color.FromArgb(120, c), hov ? 2.5f : 1.5f))
                    g.DrawRectangle(pen, x, startY, bW, bH);

                using (var f = new Font("Impact", 13)) using (var b = new SolidBrush(Color.White))
                {
                    var s = g.MeasureString(lv.Name, f); g.DrawString(lv.Name, f, b, x + bW/2 - s.Width/2, startY + 12);
                }
                using (var f2 = new Font("Segoe UI", 10)) using (var b2 = new SolidBrush(Color.LightGray))
                {
                    string diff = lv.Difficulty.ToString();
                    var s = g.MeasureString(diff, f2); g.DrawString(diff, f2, b2, x + bW/2 - s.Width/2, startY + 38);
                }
                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                string pctText = done ? T.LevelCompleted : (best > 0 ? $"{best}%" : T.LevelNoAttempts);
                Color pctColor = done ? Color.LimeGreen : (best > 0 ? Color.Yellow : Color.Gray);
                using (var f3 = new Font("Segoe UI", 10, FontStyle.Bold)) using (var b3 = new SolidBrush(pctColor))
                {
                    var s = g.MeasureString(pctText, f3); g.DrawString(pctText, f3, b3, x + bW/2 - s.Width/2, startY + 62);
                }
                if (hov) { bW -= 8; bH -= 8; startY += 4; }
            }

            using (var f = new Font("Impact", 14)) using (var b = new SolidBrush(Color.LightGray))
                g.DrawString(T.Back, f, b, 20, H - 40);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DetectHover(int mx, int my)
        {
            var levels = Levels;
            float bW = 200, bH = 100, padX = 30;
            float totalW = levels.Count * bW + (levels.Count - 1) * padX;
            float startX = ClientSize.Width/2f - totalW/2f, startY = 140;
            _hovered = -1;
            for (int i = 0; i < levels.Count; i++)
            {
                float x = startX + i * (bW + padX);
                if (mx >= x && mx <= x + bW && my >= startY && my <= startY + bH) { _hovered = i; break; }
            }
            Cursor = _hovered >= 0 ? Cursors.Hand : Cursors.Default;
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void SelectLevel(MouseEventArgs e)
        {
            if (_hovered < 0) return;
            _t.Stop();
            var lv = Levels[_hovered];
            var gf = new Partida(lv);
            gf.Show(); Hide();
        }

        private void GoBack() { _t.Stop(); new Mundos().Show(); Close(); }
    }
}