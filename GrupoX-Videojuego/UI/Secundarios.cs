using System;
using System.Linq;
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
    public class Tienda : Base
    {
        private int _tab = 0;
        private int _hovered = -1;

        public Tienda()
        {
            ClientSize = new Size(1100, 640); StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; BackColor = Color.FromArgb(8,5,25);
            FormBorderStyle = FormBorderStyle.None; MaximizeBox = false;
            MouseClick += OnClick; MouseMove += OnHover;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) GoBack(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var bg = new LinearGradientBrush(new PointF(0,0), new PointF(0,H),
                Color.FromArgb(8,5,25), Color.FromArgb(25,10,60)))
                g.FillRectangle(bg, 0, 0, W, H);

            DrawText(g, T.ShopTitle, W/2f, 35, 32, Color.White);

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            string cur = $"🪙 {SistemaGuardado.Data.Monedas} {T.Coins_}    💰 {SistemaGuardado.Data.Diamonds} {T.Diamonds_}";
            using (var ef = new Font("Segoe UI Emoji", 12)) using (var eb = new SolidBrush(Color.Gold))
            { var es = g.MeasureString(cur, ef); g.DrawString(cur, ef, eb, W/2f - es.Width/2, 58); }

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            string[] tabs = { T.TabCubes, T.TabShips, T.TabColors };
            for (int t = 0; t < tabs.Length; t++)
            {
                float tx = 80 + t * 160f, ty = 95;
                bool sel = t == _tab;
                Color tc = sel ? Color.FromArgb(100, 100, 200, 255) : Color.FromArgb(40, 80, 120, 180);
                using (var fill = new SolidBrush(tc)) g.FillRectangle(fill, tx, ty, 140, 36);
                using (var pen = new Pen(sel ? Color.DeepSkyBlue : Color.FromArgb(80, 100, 140), 2)) g.DrawRectangle(pen, tx, ty, 140, 36);
                DrawText(g, tabs[t], tx + 70, ty + 18, 14, sel ? Color.White : Color.LightGray);
            }

            var colors = MotorJuego.SkinColors;
            string[] names = { "Verde Neon","Azul Cielo","Rojo Solar","Violeta","Dorado","Rosa Neon","Cian","Blanco","Coral","Verde Menta","Azul Mar","Tomate","Viola","Lima","Agua","Salmon","Acero","Kaki" };
            int cols = 6;
            float iW = 120, iH = 116, padX = 15, padY = 15;
            float startX = (W - cols * iW - (cols - 1) * padX) / 2f, startY = 150;

            var d = SistemaGuardado.Data;
            int maxItems = _tab == 2 ? colors.Length : 5;
            for (int i = 0; i < Math.Min(maxItems, names.Length); i++)
            {
                int col = i % cols, row = i / cols;
                float ix = startX + col * (iW + padX), iy = startY + row * (iH + padY);

                var unlockedList = _tab == 0 ? d.UnlockedCubes : (_tab == 1 ? d.UnlockedShips : d.UnlockedColors);
                int selectedIdx = _tab == 0 ? d.SelectedCube : (_tab == 1 ? d.SelectedShip : d.SelectedColorIndex);

                bool unlocked = unlockedList.Contains(i);
                bool selected = selectedIdx == i;
                bool hov = _hovered == i;

                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                Color c = colors[_tab == 2 ? i : d.SelectedColorIndex];
                using (var fill = new SolidBrush(Color.FromArgb(60, c.R, c.G, c.B))) g.FillRectangle(fill, ix, iy, iW, iH);
                Color bord = selected ? Color.White : (hov ? Color.FromArgb(200, c) : Color.FromArgb(100, c));
                using (var pen = new Pen(bord, selected ? 3 : 1.5f)) g.DrawRectangle(pen, ix, iy, iW, iH);

                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                float px = ix + iW/2 - 16, py = iy + 12;
                using (var cb = new SolidBrush(c))
                {
                    if (_tab == 1)
                    {
                        if (i == 0) g.FillPolygon(cb, new PointF[] { new PointF(px, py), new PointF(px + 32, py + 16), new PointF(px, py + 32) });
                        else if (i == 1) g.FillPolygon(cb, new PointF[] { new PointF(px, py + 4), new PointF(px + 28, py + 12), new PointF(px + 28, py + 20), new PointF(px, py + 28) });
                        else if (i == 2) g.FillRectangle(cb, px, py + 8, 32, 16);
                        else if (i == 3) g.FillPolygon(cb, new PointF[] { new PointF(px, py), new PointF(px + 32, py + 16), new PointF(px, py + 32), new PointF(px + 10, py + 16) });
                        else if (i == 4) g.FillPolygon(cb, new PointF[] { new PointF(px, py + 16), new PointF(px + 16, py), new PointF(px + 32, py + 16), new PointF(px + 16, py + 32) });
                    }
                    else g.FillRectangle(cb, px, py, 32, 32);
                }
                using (var cp = new Pen(Color.White, 1.5f))
                {
                    if (_tab == 1)
                    {
                        if (i == 0) g.DrawPolygon(cp, new PointF[] { new PointF(px, py), new PointF(px + 32, py + 16), new PointF(px, py + 32) });
                        else if (i == 1) g.DrawPolygon(cp, new PointF[] { new PointF(px, py + 4), new PointF(px + 28, py + 12), new PointF(px + 28, py + 20), new PointF(px, py + 28) });
                        else if (i == 2) g.DrawRectangle(cp, px, py + 8, 32, 16);
                        else if (i == 3) g.DrawPolygon(cp, new PointF[] { new PointF(px, py), new PointF(px + 32, py + 16), new PointF(px, py + 32), new PointF(px + 10, py + 16) });
                        else if (i == 4) g.DrawPolygon(cp, new PointF[] { new PointF(px, py + 16), new PointF(px + 16, py), new PointF(px + 32, py + 16), new PointF(px + 16, py + 32) });
                    }
                    else g.DrawRectangle(cp, px, py, 32, 32);
                }

                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                if (_tab == 0)
                {
                    using (var ci = new Pen(Color.FromArgb(100, 255,255,255), 1))
                    {
                        if (i == 0) g.DrawRectangle(ci, px + 5, py + 5, 22, 22);
                        else if (i == 1) { g.DrawLine(ci, px + 16, py + 4, px + 16, py + 28); g.DrawLine(ci, px + 4, py + 16, px + 32 - 4, py + 16); }
                        else if (i == 2) g.DrawEllipse(ci, px + 4, py + 4, 24, 24);
                        else if (i == 3) { using(var bb = new SolidBrush(Color.FromArgb(180,255,255,255))) { g.FillRectangle(bb, px+8, py+8, 6, 6); g.FillRectangle(bb, px+18, py+8, 6, 6); } }
                        else if (i == 4) g.DrawPolygon(ci, new PointF[] { new PointF(px+16, py+4), new PointF(px+28, py+16), new PointF(px+16, py+28), new PointF(px+4, py+16) });
                    }
                }

                string itemName = _tab == 2 ? names[i] : T.Model(i);
                DrawText(g, itemName, ix + iW/2, iy + 58, 9, Color.LightGray);

                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                if (!unlocked)
                {
                    DrawText(g, T.Locked, ix + iW/2, iy + 70, 9, Color.Gold);
                    if (i >= 3)
                    {
                        /// <summary>
                        /// Elaborated by Godot Engine
                        /// </summary>
                        int diamondPrice = i == 3 ? 5 : 10;
                        int coinPrice = i == 3 ? 150 : 300;
                        DrawText(g, T.PriceDiamond(diamondPrice), ix + iW/2, iy + 84, 8, Color.Cyan);
                        DrawText(g, T.Price(coinPrice), ix + iW/2, iy + 96, 8, Color.Gold);
                    }
                    else
                    {
                        int coinPrice = 50 + (i * 25);
                        DrawText(g, T.Price(coinPrice), ix + iW/2, iy + 84, 8, Color.Gold);
                    }
                    if (hov) using (var ov = new SolidBrush(Color.FromArgb(40, 0,0,0))) g.FillRectangle(ov, ix, iy, iW, iH);
                }
                else
                {
                    DrawText(g, selected ? T.Active : T.Equip, ix + iW/2, iy + 76, 9, selected ? Color.LimeGreen : Color.LightGray);
                }
            }

            DrawText(g, T.Back, 80, H - 30, 14, Color.LightGray);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void OnHover(object? s, MouseEventArgs e)
        {
            int cols = 6; float iW = 120, iH = 116, padX = 15, padY = 15;
            float startX = (ClientSize.Width - cols * iW - (cols - 1) * padX) / 2f, startY = 150;
            _hovered = -1;
            int maxItems = _tab == 2 ? MotorJuego.SkinColors.Length : 5;
            for (int i = 0; i < maxItems; i++)
            {
                float ix = startX + (i % cols) * (iW + padX), iy = startY + (i / cols) * (iH + padY);
                if (e.X >= ix && e.X <= ix + iW && e.Y >= iy && e.Y <= iy + iH) { _hovered = i; break; }
            }
            Cursor = _hovered >= 0 ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void OnClick(object? s, MouseEventArgs e)
        {
            for (int t = 0; t < 3; t++) { float tx = 80 + t * 160f; if (e.X >= tx && e.X <= tx + 140 && e.Y >= 95 && e.Y <= 131) { _tab = t; Invalidate(); return; } }

            if (_hovered < 0) return;
            var d = SistemaGuardado.Data;
            var unlockedList = _tab == 0 ? d.UnlockedCubes : (_tab == 1 ? d.UnlockedShips : d.UnlockedColors);

            if (unlockedList.Contains(_hovered))
            {
                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                if (_tab == 0) d.SelectedCube = _hovered;
                else if (_tab == 1) d.SelectedShip = _hovered;
                else d.SelectedColorIndex = _hovered;
                SistemaGuardado.Save(); Invalidate();
            }
            else
            {
                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                bool bought = false;

                if (_hovered >= 3)
                {
                    int diamondPrice = _hovered == 3 ? 5 : 10;
                    int coinPrice = _hovered == 3 ? 150 : 300;
                    bool hasDiamonds = SistemaGuardado.Data.Diamonds >= diamondPrice;
                    bool hasCoins = SistemaGuardado.Data.Monedas >= coinPrice;
                    if (hasDiamonds && hasCoins)
                    {
                        SistemaGuardado.SpendDiamonds(diamondPrice);
                        SistemaGuardado.SpendCoins(coinPrice);
                        bought = true;
                    }
                    else
                    {
                        Dialogo.Show(T.NoFundsBoth(coinPrice, diamondPrice), T.NoFundsTitle);
                    }
                }
                else
                {
                    int coinPrice = 50 + (_hovered * 25);
                    bought = SistemaGuardado.SpendCoins(coinPrice);
                    if (!bought) Dialogo.Show(T.NoFunds(coinPrice), T.NoFundsTitle);
                }

                if (bought)
                {
                    unlockedList.Add(_hovered);
                    if (_tab == 0) d.SelectedCube = _hovered;
                    else if (_tab == 1) d.SelectedShip = _hovered;
                    else d.SelectedColorIndex = _hovered;
                    SistemaGuardado.Save(); Invalidate();
                }
            }
        }

        private void GoBack() { new Menu().Show(); Close(); }

        private void DrawText(Graphics g, string t, float x, float y, float size, Color c)
        {
            using (var f = new Font("Impact", Math.Max(1, size))) using (var b = new SolidBrush(c))
            { var s = g.MeasureString(t, f); g.DrawString(t, f, b, x - s.Width/2, y - s.Height/2); }
        }
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    public class Ajustes : Base
    {
        private Rectangle[] _btnRects = new Rectangle[10];

        public Ajustes()
        {
            ClientSize = new Size(1100, 640); StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; BackColor = Color.FromArgb(12, 8, 30);
            FormBorderStyle = FormBorderStyle.None; MaximizeBox = false;
            KeyPreview = true;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { new Menu().Show(); Close(); } };
            MouseClick += OnMouseClick;
            Load += (s, e) => Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var bg = new SolidBrush(Color.FromArgb(12, 8, 30))) g.FillRectangle(bg, 0, 0, W, H);

            DrawText(g, T.SettingsTitle, W/2f, 40, 28, Color.White);

            var d = SistemaGuardado.Data;
            int y = 120;
            int cx = W / 2;

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            void DrawOpt(string label, string val, int yPos, int btnIdxL, int btnIdxR)
            {
                DrawText(g, label, cx - 150, yPos, 14, Color.LightGray);
                _btnRects[btnIdxL] = new Rectangle(cx + 20, yPos - 15, 30, 30);
                _btnRects[btnIdxR] = new Rectangle(cx + 170, yPos - 15, 30, 30);
                DrawBtn(g, "<", _btnRects[btnIdxL]);
                DrawBtn(g, ">", _btnRects[btnIdxR]);
                DrawText(g, val, cx + 110, yPos, 14, Color.White);
            }

            DrawOpt(T.MusicVolume, $"{(int)(d.MusicVolume * 100)}%", y, 2, 3); y += 60;

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            DrawText(g, T.Particles, cx - 150, y, 14, Color.LightGray);
            _btnRects[4] = new Rectangle(cx + 20, y - 15, 180, 30);
            DrawBtn(g, d.LowParticulas ? T.ParticlesLow : T.ParticlesNormal, _btnRects[4], d.LowParticulas ? Color.Gold : Color.LimeGreen);
            y += 60;

            DrawOpt(T.Language_, d.Language.ToUpper(), y, 5, 6); y += 60;

            DrawText(g, T.Back, 110, H - 30, 14, Color.LightGray);
        }

        private void DrawBtn(Graphics g, string text, Rectangle r, Color? customC = null)
        {
            using (var b = new SolidBrush(Color.FromArgb(30, 30, 50))) g.FillRectangle(b, r);
            using (var p = new Pen(customC ?? Color.FromArgb(100, 150, 255), 1.5f)) g.DrawRectangle(p, r);
            DrawText(g, text, r.X + r.Width/2f, r.Y + r.Height/2f, 11, Color.White);
        }

        private void DrawText(Graphics g, string t, float x, float y, float size, Color c)
        {
            using (var f = new Font("Impact", Math.Max(1, size))) using (var b = new SolidBrush(c))
            { var s = g.MeasureString(t, f); g.DrawString(t, f, b, x - s.Width/2, y - s.Height/2 + 2); }
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void OnMouseClick(object? sender, MouseEventArgs e)
        {
            var d = SistemaGuardado.Data;
            bool changed = false;

            if (_btnRects[2].Contains(e.Location)) { d.MusicVolume = Math.Max(0f, d.MusicVolume - 0.1f); changed = true; }
            if (_btnRects[3].Contains(e.Location)) { d.MusicVolume = Math.Min(1f, d.MusicVolume + 0.1f); changed = true; }

            if (_btnRects[4].Contains(e.Location)) { d.LowParticulas = !d.LowParticulas; changed = true; }

            if (_btnRects[5].Contains(e.Location) || _btnRects[6].Contains(e.Location))
            {
                d.Language = d.Language == "es" ? "en" : "es"; changed = true;
            }

            if (changed) { SistemaGuardado.Save(); GestorAudio.UpdateVolumes(); Invalidate(); }
        }
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    public class Estadisticas : Base
    {
        public Estadisticas()
        {
            ClientSize = new Size(1100, 640); StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; BackColor = Color.FromArgb(8, 5, 25);
            FormBorderStyle = FormBorderStyle.None; MaximizeBox = false;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { new Menu().Show(); Close(); } };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            var d = SistemaGuardado.Data;
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var bg = new LinearGradientBrush(new PointF(0,0), new PointF(0,H), Color.FromArgb(8,5,25), Color.FromArgb(20,10,50)))
                g.FillRectangle(bg, 0, 0, W, H);

            DrawText(g, T.StatsTitle, W/2f, 40, 28, Color.White);

            int realCompleted = d.LevelCompleted.Count(kv => kv.Value);
            int totalLevels   = BaseNiveles.All.Count;

            var stats = new (string label, string value)[]
            {
                (T.StatLevels,       $"{realCompleted} / {totalLevels}"),
                (T.StatDeaths,       d.TotalDeaths.ToString()),
                (T.StatJumps,        d.TotalJumps.ToString()),
                (T.StatCoins,        d.TotalMonedasCollected.ToString()),
                (T.StatDiamonds,     d.Diamonds.ToString()),
                (T.StatPlayTime,     TimeSpan.FromSeconds(d.TotalPlayTimeSeconds).ToString(@"hh\:mm\:ss")),
                (T.StatHighScore,    d.HighScore.ToString()),
                (T.StatAchievements, $"{d.UnlockedLogros.Count} / {SistemaLogros.All.Count}"),
                (T.StatHighestWorld, $"{d.HighestWorldUnlocked} / 5"),
            };

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            for (int i = 0; i < stats.Length; i++)
            {
                float sy = 100 + i * 46;
                using (var row = new SolidBrush(Color.FromArgb(i % 2 == 0 ? 18 : 25, 255, 255, 255)))
                    g.FillRectangle(row, 40, sy - 8, W - 80, 40);
                DrawText(g, stats[i].label, 200, sy + 12, 13, Color.LightGray);
                DrawText(g, stats[i].value, W - 120, sy + 12, 15, Color.White);
            }

            DrawText(g, T.Back, 110, H - 30, 14, Color.LightGray);
        }

        private void DrawText(Graphics g, string t, float x, float y, float size, Color c)
        {
            using (var f = new Font("Impact", Math.Max(1, size))) using (var b = new SolidBrush(c))
            { var s = g.MeasureString(t, f); g.DrawString(t, f, b, x - s.Width/2, y - s.Height/2); }
        }
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    public class Logros : Base
    {
        public Logros()
        {
            ClientSize = new Size(1100, 640); StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; BackColor = Color.FromArgb(8, 5, 25);
            FormBorderStyle = FormBorderStyle.None; MaximizeBox = false;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { new Menu().Show(); Close(); } };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var bg = new SolidBrush(Color.FromArgb(8,5,25))) g.FillRectangle(bg, 0, 0, W, H);
            DrawText(g, T.AchTitle, W/2f, 35, 28, Color.White);

            var d = SistemaGuardado.Data;
            int unlocked = d.UnlockedLogros.Count;
            DrawText(g, T.AchUnlocked(unlocked, SistemaLogros.All.Count), W/2f, 70, 14, Color.Gold);

            int cols = 2, y0 = 100, row = 0, col = 0;
            float aW = (W - 60) / 2f - 10, aH = 62;

            foreach (var ach in SistemaLogros.All)
            {
                bool done = ach.IsUnlocked(d);
                float ax = 30 + col * (aW + 20), ay = y0 + row * (aH + 8);
                if (ay > H - 80) break;

                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                using (var fill = new SolidBrush(Color.FromArgb(done ? 40 : 20, done ? 80 : 50, done ? 40 : 50, done ? 10 : 50)))
                    g.FillRectangle(fill, ax, ay, aW, aH);
                using (var pen = new Pen(done ? Color.FromArgb(100, 200, 100) : Color.FromArgb(60, 100, 100), 1.5f))
                    g.DrawRectangle(pen, ax, ay, aW, aH);

                DrawText(g, (done ? "✔ " : "") + ach.DisplayName, ax + aW/2, ay + 18, 12, done ? Color.LimeGreen : Color.Gray);
                if (!done) DrawText(g, T.AchBlocked, ax + aW/2, ay + 33, 8, Color.FromArgb(160, 200, 100, 100));
                DrawText(g, ach.DisplayDesc, ax + aW/2, ay + 46, 9, Color.LightGray);

                col++;
                if (col >= cols) { col = 0; row++; }
            }

            DrawText(g, T.Back, 100, H - 30, 14, Color.LightGray);
        }

        private void DrawText(Graphics g, string t, float x, float y, float size, Color c)
        {
            using (var f = new Font("Impact", Math.Max(1, size))) using (var b = new SolidBrush(c))
            { var s = g.MeasureString(t, f); g.DrawString(t, f, b, x - s.Width/2, y - s.Height/2); }
        }
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    public class Personaje : Base
    {
        public Personaje()
        {
            ClientSize = new Size(1100, 640); StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; BackColor = Color.FromArgb(8, 5, 25);
            FormBorderStyle = FormBorderStyle.None; MaximizeBox = false;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { new Menu().Show(); Close(); } };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var bg = new SolidBrush(Color.FromArgb(8,5,25))) g.FillRectangle(bg, 0, 0, W, H);
            DrawText(g, T.CharTitle, W/2f, 35, 28, Color.White);

            var d = SistemaGuardado.Data;
            Color c = MotorJuego.SkinColors[Math.Min(d.SelectedColorIndex, MotorJuego.SkinColors.Length-1)];

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            float cx = W/2f - 30, cy = H/2f - 80;
            using (var cb = new SolidBrush(c)) g.FillRectangle(cb, cx, cy, 60, 60);
            using (var cp = new Pen(Color.White, 2.5f)) g.DrawRectangle(cp, cx, cy, 60, 60);
            using (var ci = new Pen(Color.FromArgb(100, 255,255,255), 1.5f))
            {
                if (d.SelectedCube == 0) g.DrawRectangle(ci, cx + 10, cy + 10, 40, 40);
                else if (d.SelectedCube == 1) { g.DrawLine(ci, cx + 30, cy + 6, cx + 30, cy + 54); g.DrawLine(ci, cx + 6, cy + 30, cx + 54, cy + 30); }
                else if (d.SelectedCube == 2) g.DrawEllipse(ci, cx + 6, cy + 6, 48, 48);
                else if (d.SelectedCube == 3) { using(var bb = new SolidBrush(Color.FromArgb(180,255,255,255))) { g.FillRectangle(bb, cx+15, cy+15, 11, 11); g.FillRectangle(bb, cx+34, cy+15, 11, 11); } }
                else if (d.SelectedCube == 4) g.DrawPolygon(ci, new PointF[] { new PointF(cx+30, cy+8), new PointF(cx+52, cy+30), new PointF(cx+30, cy+52), new PointF(cx+8, cy+30) });
            }

            DrawText(g, T.CharCurrent, W/2f, cy + 80, 14, Color.LightGray);
            DrawText(g, $"Cube #{d.SelectedCube+1} | Ship #{d.SelectedShip+1} | Color #{d.SelectedColorIndex+1}", W/2f, cy + 104, 12, c);
            DrawText(g, T.CharVisitShop, W/2f, cy + 135, 11, Color.Gray);
            DrawText(g, T.Back, 110, H - 30, 14, Color.LightGray);
        }

        private void DrawText(Graphics g, string t, float x, float y, float size, Color c)
        {
            using (var f = new Font("Impact", Math.Max(1, size))) using (var b = new SolidBrush(c))
            { var s = g.MeasureString(t, f); g.DrawString(t, f, b, x - s.Width/2, y - s.Height/2); }
        }
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    public class Perfil : Base
    {
        public Perfil()
        {
            ClientSize = new Size(1100, 640); StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; BackColor = Color.FromArgb(8, 5, 25);
            FormBorderStyle = FormBorderStyle.None; MaximizeBox = false;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { new Menu().Show(); Close(); } };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var bg = new SolidBrush(Color.FromArgb(8,5,25))) g.FillRectangle(bg, 0, 0, W, H);
            DrawText(g, T.ProfileTitle, W/2f, 35, 28, Color.White);
            var d = SistemaGuardado.Data;
            DrawText(g, T.ProfilePlayer, W/2f, 100, 20, Color.White);
            DrawText(g, T.ProfileLevel(Math.Max(1, d.TotalLevelsCompleted / 3 + 1)), W/2f, 140, 16, Color.Gold);
            DrawText(g, T.ProfileXP(d.TotalLevelsCompleted * 100 + d.TotalMonedasCollected), W/2f, 170, 14, Color.LightGray);
            DrawText(g, T.ProfileAchs(d.UnlockedLogros.Count, SistemaLogros.All.Count), W/2f, 200, 14, Color.LightGray);
            DrawText(g, T.ProfileOnline, W/2f, 260, 11, Color.Gray);
            DrawText(g, T.Back, 110, H - 30, 14, Color.LightGray);
        }

        private void DrawText(Graphics g, string t, float x, float y, float size, Color c)
        {
            using (var f = new Font("Impact", Math.Max(1, size))) using (var b = new SolidBrush(c))
            { var s = g.MeasureString(t, f); g.DrawString(t, f, b, x - s.Width/2, y - s.Height/2); }
        }
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    public class Creditos : Base
    {
        public Creditos()
        {
            ClientSize = new Size(1100, 640); StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; BackColor = Color.FromArgb(8, 5, 25);
            FormBorderStyle = FormBorderStyle.None; MaximizeBox = false;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { new Menu().Show(); Close(); } };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var bg = new LinearGradientBrush(new PointF(0,0), new PointF(0,H), Color.FromArgb(8,5,25), Color.FromArgb(20,10,50)))
                g.FillRectangle(bg, 0, 0, W, H);

            DrawText(g, T.CreditsTitle, W/2f, 40, 28, Color.White);
            DrawText(g, "GeoStorm v1.0", W/2f, 100, 20, Color.FromArgb(100,200,255));
            DrawText(g, T.CreditsEngine, W/2f, 140, 13, Color.LightGray);
            DrawText(g, T.CreditsProject, W/2f, 175, 13, Color.LightGray);
            DrawText(g, T.CreditsFacilitator, W/2f, 215, 13, Color.LightGray);

            DrawText(g, T.Back, 110, H - 30, 14, Color.LightGray);
        }

        private void DrawText(Graphics g, string t, float x, float y, float size, Color c)
        {
            using (var f = new Font("Impact", Math.Max(1, size))) using (var b = new SolidBrush(c))
            { var s = g.MeasureString(t, f); g.DrawString(t, f, b, x - s.Width/2, y - s.Height/2); }
        }
    }
}