using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using GrupoX_Videojuego.Models;

namespace GrupoX_Videojuego.Core
{
    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Estados del juego
    public enum EstadoJuego { Menu, Playing, GameOver, LevelComplete, Paused }

    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Motor principal del juego - maneja la logica, fisica, colisiones y dibujado
    public class MotorJuego
    {
        // ENCAPSULAMIENTO - propiedades publicas con get controlado
        public Jugador Jugador { get; private set; }
        public EstadoJuego CurrentState { get; set; } = EstadoJuego.Menu;
        public int CurrentPercent => _totalTiles == 0 ? 0 : Math.Min(100, (int)((_cameraX / _levelEndX) * 100));
        public float InternalWidth => _formWidth;
        public float InternalHeight => _formHeight;

        // Listas de objetos del nivel actual
        public List<Obstaculo> Obstaculos { get; private set; } = new();
        public List<Portal> Portals { get; private set; } = new();
        public List<Moneda> Monedas { get; private set; } = new();
        public List<Plataforma> Plataformas { get; private set; } = new();

        // Datos del nivel en curso
        private DatosNivel? _currentLevel;
        private float _levelEndX = 9999;
        private float _spawnX, _spawnY;
        private int _coinsMask = 0;
        private int _totalTiles = 1;
        private int _coinsCollectedThisRun = 0;

        // Camara y efectos
        private float _cameraX = 0;
        private float _shakeIntensity = 0;
        private const float SHAKE_DECAY = 0.82f;

        // Dimensiones del formulario
        private float _formWidth;
        private float _formHeight;
        private float _groundY;

        private float _gameSpeed = 8f;

        // Generador procedural de obstaculos y portales
        private int _obstacleTimer = 0;
        private int _portalTimer = 200;
        private Random _rng = new();
        private bool _proceduralMode = false;

        // Particulas y fondo decorativo
        private List<Particula> _particles = new();
        private List<EstrellaFondo> _stars = new();
        private float _bgOffset = 0;
        private float _menuAngle = 0;

        // Tiempo y logros
        private DateTime _levelStartTime;
        private long _savedPlayTimeAtStart;
        private int _deathsThisRun = 0;
        private double _runTimeSeconds = 0;
        private Logro? _pendingLogro = null;
        private float _achNotifyTimer = 0;

        public MotorJuego(float w, float h)
        {
            _formWidth = w; _formHeight = h;
            _groundY = h - 80;
            SistemaGuardado.Load();
            Jugador = MakeJugador();
            GenerateStars();
        }

        // Crea el jugador con la skin guardada
        private Jugador MakeJugador() => new Jugador(120, _groundY - 40, 40, 40,
            SkinColors[Math.Min(SistemaGuardado.Data.SelectedColorIndex, SkinColors.Length - 1)], _groundY,
            SistemaGuardado.Data.SelectedCube, SistemaGuardado.Data.SelectedShip);

        // Colores disponibles para el jugador
        public static readonly Color[] SkinColors =
        {
            Color.LimeGreen, Color.DeepSkyBlue, Color.OrangeRed, Color.MediumOrchid,
            Color.Gold, Color.HotPink, Color.Cyan, Color.White, Color.Coral,
            Color.SpringGreen, Color.DodgerBlue, Color.Tomato, Color.Violet,
            Color.YellowGreen, Color.Aquamarine, Color.Salmon, Color.SteelBlue,
            Color.Khaki
        };

        // Inicia un nivel (o modo procedural si level es null)
        public void StartLevel(DatosNivel? level)
        {
            _currentLevel = level;
            _proceduralMode = (level == null);
            _gameSpeed = 8f;
            _cameraX = 0;
            _shakeIntensity = 0;
            _bgOffset = 0;
            _obstacleTimer = 0;
            _portalTimer = 200;
            _deathsThisRun = 0;
            _runTimeSeconds = 0;
            _coinsCollectedThisRun = 0;
            _coinsMask = 0;
            _particles.Clear();
            Jugador = MakeJugador();
            Obstaculos.Clear(); Portals.Clear(); Monedas.Clear(); Plataformas.Clear();

            // Carga los objetos del nivel desde el archivo de datos
            if (level != null)
            {
                var (obs, por, coins, plat, sx, sy, ex) =
                    CargadorNiveles.Load(level, _groundY);
                Obstaculos = obs; Portals = por; Monedas = coins; Plataformas = plat;
                _spawnX = sx; _spawnY = sy; _levelEndX = ex;
                _totalTiles = level.Rows.Length > 0 ? level.Rows[0].Length : 1;
                Jugador.X = sx; Jugador.Y = sy;
            }
            else
            {
                // Modo libre: sin final definido
                _levelEndX = 99999;
                _spawnX = 120; _spawnY = _groundY - 40;
            }
            _levelStartTime = DateTime.Now;
            _savedPlayTimeAtStart = SistemaGuardado.Data.TotalPlayTimeSeconds;
            CurrentState = EstadoJuego.Playing;
        }

        // Bucle principal de actualizacion del juego
        public void Update(bool actionPressed, float deltaMs = 16f)
        {
            float dt = deltaMs / 16f;
            _menuAngle += 0.02f * dt;
            foreach (var s in _stars) s.Update(dt);
            if (_achNotifyTimer > 0) _achNotifyTimer -= dt;

            if (CurrentState == EstadoJuego.Menu) return;
            if (CurrentState != EstadoJuego.Playing) return;

            // Mueve la camara hacia la derecha
            _cameraX += _gameSpeed;
            _bgOffset += _gameSpeed * 0.2f;

            // Ignora input al inicio del nivel para evitar muerte instantanea
            if ((DateTime.Now - _levelStartTime).TotalSeconds < 0.1)
                actionPressed = false;

            bool wasGrounded = Jugador.IsGrounded;
            bool wasInCubeMode = Jugador.Mode == ModoJugador.Cube;

            // Actualiza la fisica del jugador
            bool isDead = Jugador.Update(actionPressed, Plataformas);
            if (!isDead && wasGrounded && !Jugador.IsGrounded && wasInCubeMode)
                SistemaGuardado.Data.TotalJumps++;
            if (isDead)
            {
                OnDeath();
                return;
            }

            // Actualiza y elimina particulas muertas
            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                _particles[i].Update(dt);
                if (_particles[i].IsDead) _particles.RemoveAt(i);
            }

            _shakeIntensity *= SHAKE_DECAY;

            // Verifica colision con monedas
            for (int idx = 0; idx < Monedas.Count; idx++)
            {
                var coin = Monedas[idx];
                if (!coin.Collected && Jugador.IntersectsWith(coin))
                {
                    coin.Collected = true;
                    _coinsCollectedThisRun++;
                    _coinsMask |= (1 << idx);
                    SistemaGuardado.Data.Monedas++;
                    SistemaGuardado.Data.TotalMonedasCollected++;
                }
                if (!coin.Collected) coin.Update();
            }

            // GENERADOR PROCEDURAL - crea obstaculos y portales aleatoriamente
            if (_proceduralMode)
            {
                _obstacleTimer++;
                if (_obstacleTimer > 55 + _rng.Next(0, 30))
                {
                    float h = 35 + _rng.Next(0, 35);
                    float yy = _rng.Next(0, 3) == 0 ? _groundY - h - _rng.Next(60, 120) : _groundY - h;
                    float ox = _cameraX + _formWidth + 80;
                    float hitW = 20, hitH = h * 0.35f;
                    float hitX2 = ox + 4, hitY2 = yy + h * 0.2f;
                    Obstaculos.Add(new Obstaculo(hitX2, hitY2, hitW, hitH, Color.OrangeRed, 0, ox, yy, 28, h));
                    _obstacleTimer = 0;
                }
                _portalTimer--;
                if (_portalTimer <= 0)
                {
                    bool toShip = Jugador.Mode == ModoJugador.Cube;
                    var c = toShip ? Color.FromArgb(100, 200, 255) : Color.FromArgb(100, 255, 120);
                    var mode = toShip ? ModoJugador.Ship : ModoJugador.Cube;
                    Portals.Add(new Portal(_cameraX + _formWidth + 80, _groundY - 120, 20, 120, c, mode, 0));
                    _portalTimer = 200 + _rng.Next(0, 120);
                }
            }

            // Verifica colision con obstaculos (muerte)
            for (int i = Obstaculos.Count - 1; i >= 0; i--)
            {
                var obs = Obstaculos[i];
                if (_proceduralMode)
                {
                    obs.Speed = _gameSpeed;
                    obs.Update();
                    if (obs.X + obs.Width < _cameraX - 80) { Obstaculos.RemoveAt(i); continue; }
                }
                if (Jugador.IntersectsWith(obs)) { OnDeath(); return; }
            }

            // Verifica colision con portales (cambio de modo)
            for (int i = Portals.Count - 1; i >= 0; i--)
            {
                var portal = Portals[i];
                if (_proceduralMode)
                {
                    portal.Speed = _gameSpeed;
                    portal.Update();
                    if (portal.X + portal.Width < _cameraX - 80) { Portals.RemoveAt(i); continue; }
                }
                if (Jugador.IntersectsWith(portal))
                {
                    Jugador.Mode = portal.TargetMode;
                    if (portal.TargetMode == ModoJugador.Ship)
                    {
                        SistemaGuardado.Data.HasEnteredShipMode = true;
                    }
                    Portals.RemoveAt(i);
                    continue;
                }
            }

            // Verifica si el jugador llego al final del nivel
            if (!_proceduralMode && Jugador.X >= _levelEndX)
            {
                OnLevelComplete();
                return;
            }

            // Actualiza el tiempo total jugado
            SistemaGuardado.Data.TotalPlayTimeSeconds = _savedPlayTimeAtStart + (long)(DateTime.Now - _levelStartTime).TotalSeconds;
        }

        // Se ejecuta cuando el jugador muere
        private void OnDeath()
        {
            _deathsThisRun++;
            _runTimeSeconds = (DateTime.Now - _levelStartTime).TotalSeconds;
            SistemaGuardado.RecordDeath();
            _shakeIntensity = 14;
            SpawnExplosion(Jugador.X + Jugador.Width / 2, Jugador.Y + Jugador.Height / 2);
            GestorAudio.PlayDeathSound();

            // Guarda el progreso del nivel
            int pct = CurrentPercent;
            if (_currentLevel != null)
                SistemaGuardado.UpdateBestPercent(_currentLevel.Id, pct);
            else if (_proceduralMode)
            {
                int score = CalcScore();
                if (score > SistemaGuardado.Data.HighScore)
                    SistemaGuardado.Data.HighScore = score;
            }

            // Verifica si se desbloqueo algun logro
            var newAchs = SistemaLogros.CheckAndUnlock(SistemaGuardado.Data);
            if (newAchs.Count > 0) { _pendingLogro = newAchs[0]; _achNotifyTimer = 180; SistemaGuardado.Save(); }
            else SistemaGuardado.Save();

            CurrentState = EstadoJuego.GameOver;
        }

        // Se ejecuta cuando el jugador completa el nivel
        private void OnLevelComplete()
        {
            if (_currentLevel != null)
            {
                if (_deathsThisRun == 0)
                {
                    SistemaGuardado.Data.CompletedLevelWithoutDying = true;
                }
                SistemaGuardado.RecordLevelComplete(_currentLevel.Id, _coinsMask);
                int score = (int)(_cameraX / 10);
                if (score > SistemaGuardado.Data.HighScore)
                    SistemaGuardado.Data.HighScore = score;
                // Desbloquea el siguiente mundo si es necesario
                int nextWorld = Math.Min(_currentLevel.WorldId + 1, 5);
                if (nextWorld > SistemaGuardado.Data.HighestWorldUnlocked)
                    SistemaGuardado.Data.HighestWorldUnlocked = nextWorld;
                SistemaGuardado.Save();
            }
            var newAchs = SistemaLogros.CheckAndUnlock(SistemaGuardado.Data);
            if (newAchs.Count > 0) { _pendingLogro = newAchs[0]; _achNotifyTimer = 180; }
            CurrentState = EstadoJuego.LevelComplete;
        }
        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void SpawnExplosion(float cx, float cy)
        {
            int count = SistemaGuardado.Data.LowParticulas ? 15 : 35;
            for (int i = 0; i < count; i++) _particles.Add(new Particula(cx, cy, _rng));
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        public void Draw(Graphics g, float w, float h)
        {
            var baseTransform = g.Transform;

            float sx = _shakeIntensity > 0.5f ? ((float)_rng.NextDouble() * 2 - 1) * _shakeIntensity : 0;
            float sy = _shakeIntensity > 0.5f ? ((float)_rng.NextDouble() * 2 - 1) * _shakeIntensity : 0;
            g.TranslateTransform(sx, sy);

            DrawBackground(g, w, h);

            if (CurrentState == EstadoJuego.Menu) { DrawMenu(g, w, h); g.Transform = baseTransform; return; }

            DrawGround(g, w, h);

            float cam = _cameraX;
            g.TranslateTransform(-cam + sx, 0);

            foreach (var plat in Plataformas) plat.Draw(g);
            foreach (var obs in Obstaculos) obs.Draw(g);
            foreach (var por in Portals) por.Draw(g);
            foreach (var coin in Monedas) coin.Draw(g);
            Jugador.Draw(g);

            g.Transform = baseTransform;
            g.TranslateTransform(sx, sy);

            foreach (var p in _particles) p.Draw(g);

            g.Transform = baseTransform;

            DrawHUD(g, w, h);

            if (CurrentState == EstadoJuego.GameOver) DrawGameOver(g, w, h);
            if (CurrentState == EstadoJuego.LevelComplete) DrawLevelComplete(g, w, h);
            if (CurrentState == EstadoJuego.Paused) DrawPauseMenu(g, w, h);
            if (_achNotifyTimer > 0 && _pendingLogro != null)
                DrawLogroNotify(g, w);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawBackground(Graphics g, float w, float h)
        {
            Color top = _currentLevel?.BackgroundColor ?? Color.FromArgb(15, 10, 40);
            Color bot = Color.FromArgb(Math.Max(0, top.R - 10), Math.Max(0, top.G - 5), Math.Max(0, top.B + 20));
            using (var brush = new LinearGradientBrush(new PointF(0, 0), new PointF(0, h), top, bot))
                g.FillRectangle(brush, 0, 0, w, h);
            foreach (var star in _stars) star.Draw(g);
            if (CurrentState == EstadoJuego.Playing)
            {
                float off = _bgOffset % 100;
                using (var lp = new Pen(Color.FromArgb(15, 255, 255, 255), 1))
                    for (float x = -off; x < w + 100; x += 100) g.DrawLine(lp, x, 0, x, h);
            }
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawGround(Graphics g, float w, float h)
        {
            Color gc = _currentLevel?.GroundColor ?? Color.FromArgb(50, 60, 80);
            using (var brush = new LinearGradientBrush(new PointF(0, _groundY), new PointF(0, h), gc, Color.FromArgb(20, 25, 35)))
                g.FillRectangle(brush, 0, _groundY, w, h - _groundY);
            using (var pen = new Pen(Color.FromArgb(200, 100, 220, 255), 3))
                g.DrawLine(pen, 0, _groundY, w, _groundY);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawMenu(Graphics g, float w, float h)
        {
            float demoX = w / 2 - 20;
            float demoY = h / 2 - 20 + (float)(Math.Sin(_menuAngle * 2) * 18);
            using (var b = new SolidBrush(Color.LimeGreen)) g.FillRectangle(b, demoX, demoY, 40, 40);
            using (var p = new Pen(Color.White, 2)) g.DrawRectangle(p, demoX, demoY, 40, 40);
            DrawGround(g, w, h);
            float pulse = 44f + (float)(Math.Sin(_menuAngle) * 3);
            DrawShadowText(g, "GeoStorm", w / 2, h / 2 - 130, pulse, Color.FromArgb(100, 190, 255), Color.White);
            DrawText(g, T.PressToPlay, w / 2, h / 2 + 70, 15, Color.LightGray);
            DrawText(g, $"Mejor: {SistemaGuardado.Data.HighScore}", w / 2, h / 2 + 100, 13, Color.Gold);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawHUD(Graphics g, float w, float h)
        {
            using (var bg = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                g.FillRectangle(bg, 0, 0, w, 46);

            if (!_proceduralMode && _currentLevel != null)
            {
                int pct = CurrentPercent;
                float barW = w - 40;
                using (var bg2 = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
                    g.FillRectangle(bg2, 20, 38, barW, 4);
                using (var fg = new SolidBrush(Color.LimeGreen))
                    g.FillRectangle(fg, 20, 38, barW * pct / 100f, 4);
                DrawText(g, $"{pct}%", w / 2, 22, 16, Color.White);
                DrawText(g, _currentLevel.Name, w / 2 - 100, 22, 13, Color.LightGray);
            }
            else
            {
                DrawText(g, $"Puntos: {CalcScore()}", w / 2, 22, 18, Color.White);
            }

            DrawText(g, $"💰 {SistemaGuardado.Data.Diamonds}", w - 60, 22, 11, Color.Cyan, "Segoe UI Emoji");
            DrawText(g, $"🪙 {SistemaGuardado.Data.Monedas}", w - 130, 22, 11, Color.Gold, "Segoe UI Emoji");

            string mode = Jugador.Mode == ModoJugador.Ship ? T.HudShip : T.HudCube;
            Color mc = Jugador.Mode == ModoJugador.Ship ? Color.FromArgb(100, 200, 255) : Color.LimeGreen;
            DrawText(g, mode, 80, 22, 13, mc);
        }

        private int CalcScore() => (int)(_cameraX / 100);

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawGameOver(Graphics g, float w, float h)
        {
            bool isEn = SistemaGuardado.Data.Language == "en";
            using (var o = new SolidBrush(Color.FromArgb(170, 0, 0, 0))) g.FillRectangle(o, 0, 0, w, h);
            DrawShadowText(g, "GAME OVER", w / 2, h / 2 - 55, 48, Color.DarkRed, Color.Red);
            DrawText(g, isEn ? $"You reached {CurrentPercent}%" : $"Llegaste al {CurrentPercent}%", w / 2, h / 2 + 8, 22, Color.White);
            int mins = (int)_runTimeSeconds / 60;
            int secs = (int)_runTimeSeconds % 60;
            string timeStr = $"{mins}:{secs:D2}";
            DrawText(g, isEn ? $"Time survived: {timeStr}" : $"Tiempo sobrevivido: {timeStr}", w / 2, h / 2 + 38, 16, Color.OrangeRed);
            DrawText(g, isEn ? $"Monedas collected: {_coinsCollectedThisRun}" : $"Monedas recogidas: {_coinsCollectedThisRun}", w / 2, h / 2 + 62, 15, Color.Gold);
            DrawText(g, isEn ? "SPACE to retry   |   ESC to quit" : "ESPACIO para reintentar   |   ESC para salir", w / 2, h / 2 + 96, 14, Color.LightGray);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawPauseMenu(Graphics g, float w, float h)
        {
            bool isEn = SistemaGuardado.Data.Language == "en";
            using (var o = new SolidBrush(Color.FromArgb(170, 0, 0, 0))) g.FillRectangle(o, 0, 0, w, h);
            DrawShadowText(g, isEn ? "GAME PAUSED" : "JUEGO PAUSADO", w / 2, h / 2 - 30, 48, Color.DarkBlue, Color.Cyan);
            DrawText(g, isEn ? "SPACE or ESC to resume   |   Q to quit" : "ESPACIO o ESC para continuar   |   Q para salir", w / 2, h / 2 + 40, 14, Color.LightGray);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawLevelComplete(Graphics g, float w, float h)
        {
            bool isEn = SistemaGuardado.Data.Language == "en";
            using (var o = new SolidBrush(Color.FromArgb(180, 10, 40, 10))) g.FillRectangle(o, 0, 0, w, h);
            DrawShadowText(g, isEn ? "LEVEL COMPLETE!" : "NIVEL COMPLETADO!", w / 2, h / 2 - 60, 40, Color.DarkGreen, Color.LimeGreen);
            
            int totalCoins = Monedas.Count;
            if (totalCoins > 0)
            {
                DrawText(g, isEn ? $"Coins: {_coinsCollectedThisRun} / {totalCoins}" : $"Monedas: {_coinsCollectedThisRun} / {totalCoins}", w / 2, h / 2 + 10, 22, Color.Gold);
            }
            
            DrawText(g, isEn ? "+1 Diamond" : "+1 Diamante", w / 2, h / 2 + 42, 18, Color.Cyan);
            DrawText(g, isEn ? "SPACE to continue   |   ESC to quit" : "ESPACIO para continuar   |   ESC para salir", w / 2, h / 2 + 80, 14, Color.LightGray);
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawLogroNotify(Graphics g, float w)
        {
            if (_pendingLogro == null) return;
            float alpha = Math.Min(1f, _achNotifyTimer / 20f) * 255;
            using (var bg = new SolidBrush(Color.FromArgb((int)alpha, 30, 80, 30)))
                g.FillRectangle(bg, w - 310, 50, 300, 60);
            using (var brd = new Pen(Color.FromArgb((int)alpha, 100, 220, 100), 2))
                g.DrawRectangle(brd, w - 310, 50, 300, 60);
            string notifTitle = SistemaGuardado.Data.Language == "en" ? "🏆 Logro Unlocked" : "🏆 Logro Desbloqueado";
            DrawText(g, notifTitle, w - 160, 65, 13, Color.FromArgb((int)alpha, 150, 255, 150), "Segoe UI Emoji");
            DrawText(g, _pendingLogro.DisplayName, w - 160, 88, 14, Color.FromArgb((int)alpha, 255, 255, 255), "Segoe UI");
        }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        private void DrawText(Graphics g, string t, float x, float y, float size, Color c, string fontName = "Impact")
        {
            using (var font = new Font(fontName, size, FontStyle.Regular))
            using (var brush = new SolidBrush(c))
            {
                var s = g.MeasureString(t, font);
                g.DrawString(t, font, brush, x - s.Width / 2, y - s.Height / 2);
            }
        }

        private void DrawShadowText(Graphics g, string t, float x, float y, float sz, Color shadow, Color main)
        {
            DrawText(g, t, x + 3, y + 3, sz, shadow);
            DrawText(g, t, x, y, sz, main);
        }

        private void GenerateStars()
        {
            for (int i = 0; i < 90; i++) _stars.Add(new EstrellaFondo(_rng, _formWidth, _formHeight));
        }
    }
    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    internal class Particula
    {
        private float _x, _y, _vx, _vy, _life, _size;
        private Color _color;
        public bool IsDead => _life <= 0;

        public Particula(float x, float y, Random rng)
        {
            _x = x; _y = y;
            double a = rng.NextDouble() * Math.PI * 2;
            float sp = 2f + (float)rng.NextDouble() * 7f;
            _vx = (float)Math.Cos(a) * sp;
            _vy = (float)Math.Sin(a) * sp;
            _life = 1f; _size = 3 + (float)rng.NextDouble() * 6;
            _color = Color.FromArgb(rng.Next(200, 255), rng.Next(100, 220), rng.Next(0, 100));
        }

        public void Update(float dt = 1f) { _x += _vx * dt; _y += _vy * dt; _vy += 0.35f * dt; _life -= 0.04f * dt; }

        public void Draw(Graphics g)
        {
            int a = (int)(_life * 255); if (a <= 0) return;
            using (var b = new SolidBrush(Color.FromArgb(a, _color)))
                g.FillEllipse(b, _x - _size / 2, _y - _size / 2, _size, _size);
        }
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    internal class EstrellaFondo
    {
        private float _x, _y, _speed, _size, _fw, _fh;
        private int _alpha; private Random _rng;

        public EstrellaFondo(Random rng, float w, float h) { _rng = rng; _fw = w; _fh = h; Reset(true); }

        private void Reset(bool rx = false)
        {
            _x = rx ? (float)_rng.NextDouble() * _fw : _fw + 4;
            _y = (float)_rng.NextDouble() * _fh;
            _speed = 0.3f + (float)_rng.NextDouble() * 1.4f;
            _size = 1f + (float)_rng.NextDouble() * 2.5f;
            _alpha = 70 + _rng.Next(0, 180);
        }

        public void Update(float dt = 1f) { _x -= _speed * dt; if (_x < -4) Reset(); }

        public void Draw(Graphics g)
        {
            using (var b = new SolidBrush(Color.FromArgb(_alpha, 220, 220, 255)))
                g.FillEllipse(b, _x, _y, _size, _size);
        }
    }
}