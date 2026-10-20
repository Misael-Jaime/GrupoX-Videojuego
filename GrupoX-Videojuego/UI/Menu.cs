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
    public class Menu : Base
    {
        private System.Windows.Forms.Timer _animTimer;
        private float _angle = 0;
        private float _bgOffset = 0;
        private Random _rng = new();
        private List<EstrellaMenu> _stars = new();

        private readonly (string label, string icon, Action onClick)[] _buttons;
        private int _hoverIndex = -1;
        private float[] _btnScale;
        private PointF[] _btnPositions;

        public Menu()
        {
            this.ClientSize = new Size(1100, 640);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.MaximizeBox = false;
            this.BackColor = Color.Black;

            SistemaGuardado.Load();
            GestorAudio.PlayMenuMusic();

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            _buttons = new (string, string, Action)[]
            {
                (T.Play,        "▶",  OpenWorldMap),
                (T.Shop,        "🛒", OpenShop),
                (T.Character,   "👤", OpenCharacter),
                (T.Achievements,"🏆", OpenAchievements),
                (T.Settings,    "⚙",  OpenSettings),
                (T.Profile,     "👑", OpenProfile),
                (T.Statistics,  "📊", OpenStats),
                (T.Credits,     "ℹ",  OpenCredits),
                (T.Quit,        "🚪", () => Application.Exit()),
            };
            _btnScale = new float[_buttons.Length];
            for (int i = 0; i < _btnScale.Length; i++) _btnScale[i] = 1f;
            _btnPositions = BuildButtonPositions();

            for (int i = 0; i < 120; i++) _stars.Add(new EstrellaMenu(_rng, ClientSize.Width, ClientSize.Height));

            int fps = 60;
            _animTimer = new System.Windows.Forms.Timer { Interval = 1000 / fps };
            _animTimer.Tick += OnTick;
            _animTimer.Start();

            this.MouseMove  += OnMouseMove;
            this.MouseClick += OnMouseClick;
            this.KeyDown    += OnKeyDown;
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private PointF[] BuildButtonPositions()
        {
            int cols = 3, rows = 3;
            float startX = (ClientSize.Width - cols * 160 - (cols - 1) * 20) / 2f;
            float startY = ClientSize.Height - rows * 72 - (rows - 1) * 14 - 30;
            var pts = new PointF[_buttons.Length];
            for (int i = 0; i < _buttons.Length; i++)
            {
                int col = i % cols, row = i / cols;
                pts[i] = new PointF(startX + col * 180, startY + row * 86);
            }
            return pts;
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void OnTick(object? s, EventArgs e)
        {
            _angle += 0.015f;
            _bgOffset += 0.5f;
            foreach (var star in _stars) star.Update();

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            for (int i = 0; i < _btnScale.Length; i++)
            {
                float target = (i == _hoverIndex) ? 1.08f : 1f;
                _btnScale[i] += (target - _btnScale[i]) * 0.15f;
            }
            Invalidate();
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            using (var bg = new LinearGradientBrush(new PointF(0, 0), new PointF(0, H),
                Color.FromArgb(8, 5, 25), Color.FromArgb(20, 10, 50)))
                g.FillRectangle(bg, 0, 0, W, H);

            foreach (var star in _stars) star.Draw(g);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            float off = _bgOffset % 80;
            using (var lp = new Pen(Color.FromArgb(12, 255, 255, 255), 1))
            {
                for (float x = -off; x < W + 80; x += 80) g.DrawLine(lp, x, 0, x, H);
                for (float y = -off; y < H + 80; y += 80) g.DrawLine(lp, 0, y, W, y);
            }

            DrawDemoCube(g, W / 2f, H / 2f - 120, _angle);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            float titlePulse = 54f + (float)(Math.Sin(_angle * 1.5) * 4);
            DrawGlowText(g, "GeoStorm", W / 2f, H / 2f - 220, titlePulse);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            using (var sf = new SolidBrush(Color.FromArgb(160, 180, 200, 255)))
            using (var font = new Font("Segoe UI", 12))
            {
                string sub = SistemaGuardado.Data.Language == "en" ? "Now available on PC!" : "Ahora en lanzamiento para PC!";
                var sz = g.MeasureString(sub, font);
                g.DrawString(sub, font, sf, W / 2f - sz.Width / 2, H / 2f - 175);
            }

            DrawCurrencyBar(g, W, 14);

            for (int i = 0; i < _buttons.Length; i++)
                DrawMenuButton(g, i);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            using (var gb = new LinearGradientBrush(new PointF(0, H - 30), new PointF(0, H),
                Color.FromArgb(60, 100, 220, 255), Color.FromArgb(20, 50, 80, 140)))
                g.FillRectangle(gb, 0, H - 30, W, 30);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawDemoCube(Graphics g, float cx, float cy, float angle)
        {
            const float size = 45;
            var save = g.Transform;
            g.TranslateTransform(cx, cy);
            g.RotateTransform((float)(angle * 180 / Math.PI));

            var d = SistemaGuardado.Data;
            Color skinColor = MotorJuego.SkinColors[Math.Min(d.SelectedColorIndex, MotorJuego.SkinColors.Length - 1)];
            int cubeType = d.SelectedCube;

            using (var b = new SolidBrush(skinColor))
                g.FillRectangle(b, -size/2, -size/2, size, size);
            using (var pen = new Pen(Color.White, 2))
                g.DrawRectangle(pen, -size/2, -size/2, size, size);
            using (var pi = new Pen(Color.FromArgb(100, 255, 255, 255), 1))
            {
                if (cubeType == 0)
                    g.DrawRectangle(pi, -size/2 + 5, -size/2 + 5, size - 10, size - 10);
                else if (cubeType == 1)
                {
                    g.DrawLine(pi, 0, -size/2 + 4, 0, size/2 - 4);
                    g.DrawLine(pi, -size/2 + 4, 0, size/2 - 4, 0);
                }
                else if (cubeType == 2)
                    g.DrawEllipse(pi, -size/2 + 4, -size/2 + 4, size - 8, size - 8);
                else if (cubeType == 3)
                {
                    using (var bb = new SolidBrush(Color.FromArgb(180, 255, 255, 255)))
                    {
                        g.FillRectangle(bb, -8, -8, 7, 7);
                        g.FillRectangle(bb, 1, -8, 7, 7);
                    }
                }
                else
                    g.DrawPolygon(pi, new PointF[] {
                        new PointF(0, -size/2 + 4),
                        new PointF(size/2 - 4, 0),
                        new PointF(0, size/2 - 4),
                        new PointF(-size/2 + 4, 0)
                    });
            }

            g.Transform = save;
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawGlowText(Graphics g, string text, float x, float y, float size)
        {
            using (var glowFont = new Font("Impact", size, FontStyle.Regular))
            {
                var sz = g.MeasureString(text, glowFont);
                float tx = x - sz.Width / 2, ty = y - sz.Height / 2;
                using (var glow = new SolidBrush(Color.FromArgb(60, 80, 160, 255)))
                    for (int d = 6; d >= 1; d--)
                        g.DrawString(text, glowFont, glow, tx - d, ty - d);
                using (var shadow = new SolidBrush(Color.FromArgb(100, 20, 80, 180)))
                    g.DrawString(text, glowFont, shadow, tx + 4, ty + 4);
                using (var main = new LinearGradientBrush(
                    new PointF(tx, ty), new PointF(tx, ty + sz.Height),
                    Color.White, Color.FromArgb(160, 200, 255)))
                    g.DrawString(text, glowFont, main, tx, ty);
            }
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawMenuButton(Graphics g, int index)
        {
            var pos = _btnPositions[index];
            float sc = _btnScale[index];
            const float BW = 160, BH = 72;
            float x = pos.X + BW * (1 - sc) / 2;
            float y = pos.Y + BH * (1 - sc) / 2;
            float w = BW * sc, h = BH * sc;

            bool hov = (index == _hoverIndex);
            Color c1 = hov ? Color.FromArgb(60, 120, 220, 255) : Color.FromArgb(40, 60, 100, 180);
            Color c2 = hov ? Color.FromArgb(80, 50, 160, 220) : Color.FromArgb(30, 30, 60, 120);

            using (var fill = new LinearGradientBrush(new PointF(x, y), new PointF(x, y + h), c1, c2))
                g.FillRectangle(fill, x, y, w, h);

            Color borderColor = hov ? Color.FromArgb(200, 100, 200, 255) : Color.FromArgb(120, 80, 140, 200);
            using (var border = new Pen(borderColor, hov ? 2.5f : 1.5f))
                g.DrawRectangle(border, x, y, w, h);

            var (label, icon, _) = _buttons[index];
            using (var iconFont = new Font("Segoe UI Emoji", 18 * sc, FontStyle.Regular))
            using (var iconBrush = new SolidBrush(Color.White))
            {
                var isz = g.MeasureString(icon, iconFont);
                g.DrawString(icon, iconFont, iconBrush, x + w / 2 - isz.Width / 2, y + h * 0.05f);
            }
            using (var lblFont = new Font("Impact", 10 * sc, FontStyle.Regular))
            using (var lblBrush = new SolidBrush(hov ? Color.White : Color.FromArgb(200, 220, 240, 255)))
            {
                var lsz = g.MeasureString(label, lblFont);
                g.DrawString(label, lblFont, lblBrush, x + w / 2 - lsz.Width / 2, y + h - lsz.Height - 6);
            }
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawCurrencyBar(Graphics g, int W, int y)
        {
            var d = SistemaGuardado.Data;
            string[] items = {
                $"\ud83e\ude99 {d.Monedas} {T.Coins_}",
                $"\ud83d\udcb0 {d.Diamonds} {T.Diamonds_}",
            };
            Color[] colors = { Color.Gold, Color.Cyan };

            using (var bg = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
                g.FillRectangle(bg, 0, 0, W, 36);

            using (var font = new Font("Segoe UI Emoji", 12, FontStyle.Regular))
            {
                float totalWidth = 0;
                float[] widths = new float[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    widths[i] = g.MeasureString(items[i], font).Width;
                    totalWidth += widths[i];
                }
                float spacing = 40;
                totalWidth += spacing * (items.Length - 1);

                float cx = W / 2f - totalWidth / 2f;
                for (int i = 0; i < items.Length; i++)
                {
                    using (var brush = new SolidBrush(colors[i]))
                        g.DrawString(items[i], font, brush, cx, y);
                    cx += widths[i] + spacing;
                }
            }
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void OnMouseMove(object? s, MouseEventArgs e)
        {
            _hoverIndex = -1;
            for (int i = 0; i < _buttons.Length; i++)
            {
                var pos = _btnPositions[i];
                var rect = new RectangleF(pos.X, pos.Y, 160, 72);
                if (rect.Contains(e.Location)) { _hoverIndex = i; break; }
            }
            this.Cursor = _hoverIndex >= 0 ? Cursors.Hand : Cursors.Default;
        }

        private void OnMouseClick(object? s, MouseEventArgs e)
        {
            if (_hoverIndex >= 0) _buttons[_hoverIndex].onClick();
        }

        private void OnKeyDown(object? s, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space) OpenWorldMap();
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void OpenWorldMap()    { _animTimer.Stop(); new Mundos().Show(); this.Hide(); }
        private void OpenShop() { _animTimer.Stop(); new Tienda().Show(); this.Hide(); }
        private void OpenCharacter()   { _animTimer.Stop(); new Personaje().Show(); this.Hide(); }
        private void OpenAchievements(){ _animTimer.Stop(); new Logros().Show(); this.Hide(); }
        private void OpenSettings()    { _animTimer.Stop(); new Ajustes().Show(); this.Hide(); }
        private void OpenProfile()     { _animTimer.Stop(); new Perfil().Show(); this.Hide(); }
        private void OpenStats()       { _animTimer.Stop(); new Estadisticas().Show(); this.Hide(); }
        private void OpenCredits()     { _animTimer.Stop(); new Creditos().Show(); this.Hide(); }
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    internal class EstrellaMenu
    {
        private float _x, _y, _speed, _size, _fw, _fh, _life;
        private Random _rng;
        public EstrellaMenu(Random rng, float w, float h) { _rng = rng; _fw = w; _fh = h; Reset(true); }
        private void Reset(bool rx = false)
        {
            _x = rx ? (float)_rng.NextDouble() * _fw : _fw + 4;
            _y = (float)_rng.NextDouble() * _fh;
            _speed = 0.4f + (float)_rng.NextDouble() * 1.5f;
            _size  = 1f + (float)_rng.NextDouble() * 3f;
            _life  = 0.4f + (float)_rng.NextDouble() * 0.6f;
        }
        public void Update() { _x -= _speed; if (_x < -4) Reset(); }
        public void Draw(Graphics g)
        {
            int a = (int)(_life * 200);
            using (var b = new SolidBrush(Color.FromArgb(a, 200, 210, 255)))
                g.FillEllipse(b, _x, _y, _size, _size);
        }
    }
}