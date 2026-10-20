using System;
using System.Drawing;
using System.Collections.Generic;
using GrupoX_Videojuego.Core;

namespace GrupoX_Videojuego.Models
{
    // Modos de juego del jugador
    public enum ModoJugador { Cube, Ship }

    // HERENCIA - hereda de ObjetoJuego
    // Representa al jugador, maneja fisica, colisiones y dibujado
    public class Jugador : ObjetoJuego
    {
        // ENCAPSULAMIENTO - propiedades con get/set
        public ModoJugador Mode { get; set; } = ModoJugador.Cube;
        public float VelocityY { get; set; }
        public float ForwardSpeed { get; set; } = 8f;
        public float Gravity { get; set; } = 1.6f;
        public float JumpStrength { get; set; } = -20.0f;
        public float MaxFallSpeed { get; set; } = 15f;

        // Fisicas del modo nave
        public float ShipFlyStrength { get; set; } = -1.5f;
        public float ShipGravity { get; set; } = 0.8f;
        public float MaxVelocityY { get; set; } = 12f;

        public bool IsGrounded { get; set; }
        private float _groundY;

        // Historial de posiciones para dibujar la estela
        public List<PointF> Trail { get; private set; } = new();
        private int TrailLength => SistemaGuardado.Data.LowParticulas ? 5 : 10;
        public int CubeType { get; set; } = 0;
        public int ShipType { get; set; } = 0;

        public Jugador(float x, float y, float width, float height, Color color, float groundY, int cubeType = 0, int shipType = 0)
            : base(x, y, width, height, color)
        {
            _groundY = groundY;
            IsGrounded = true;
            CubeType = cubeType;
            ShipType = shipType;
        }

        // Logica principal del jugador: fisica, colisiones y estado
        // Devuelve true si el jugador murio
        public bool Update(bool isActionPressed, List<Plataforma> platforms)
        {
            // Mueve el jugador hacia la derecha automaticamente
            X += ForwardSpeed;

            // Verifica colision con plataformas despues de moverse
            RectangleF playerBounds = GetBounds();
            foreach (var plat in platforms)
            {
                if (playerBounds.IntersectsWith(plat.GetBounds()))
                {
                    return true; // muerte por colision
                }
            }

            // Actualiza la estela visual
            Trail.Insert(0, new PointF(X, Y));
            if (Trail.Count > TrailLength) Trail.RemoveAt(Trail.Count - 1);

            // FISICAS MODO CUBO - gravedad y salto
            if (Mode == ModoJugador.Cube)
            {
                VelocityY += Gravity;
                if (VelocityY > MaxFallSpeed) VelocityY = MaxFallSpeed;

                // Salto: solo si esta en el suelo y presiona
                if (isActionPressed && IsGrounded)
                {
                    VelocityY = JumpStrength;
                    IsGrounded = false;
                }
            }
            // FISICAS MODO NAVE - vuela con accion, gravedad sin accion
            else if (Mode == ModoJugador.Ship)
            {
                VelocityY += isActionPressed ? ShipFlyStrength : ShipGravity;
                VelocityY = Math.Clamp(VelocityY, -MaxVelocityY, MaxVelocityY);
            }

            // Aplica la velocidad vertical
            Y += VelocityY;
            IsGrounded = false;

            // Resuelve colision con plataformas (suelo y techo)
            playerBounds = GetBounds();
            foreach (var plat in platforms)
            {
                var pb = plat.GetBounds();
                if (playerBounds.IntersectsWith(pb))
                {
                    // Aterrizando sobre la plataforma
                    if (VelocityY > 0)
                    {
                        Y = pb.Top - Height;
                        VelocityY = 0;
                        IsGrounded = true;
                    }
                    // Golpeando el techo
                    else if (VelocityY < 0)
                    {
                        Y = pb.Bottom;
                        VelocityY = 0;
                    }
                    playerBounds = GetBounds();
                }
            }

            // Limita al jugador dentro de la pantalla (parte superior)
            if (Y <= 46)
            {
                Y = 46;
                if (VelocityY < 0) VelocityY = 0;
            }

            // Muerte si se cayo del mapa
            if (Y > _groundY + 120)
            {
                return true;
            }

            return false;
        }

        // Metodo de la clase base (no se usa directamente)
        public override void Update() { }

        /// <summary>
        /// Elaborated by Godot Engine
        /// </summary>
        public override void Draw(Graphics g)
        {
            for (int i = 0; i < Trail.Count; i++)
            {
                int alpha = 255 - (i * (255 / TrailLength));
                float size = Width * (1 - ((float)i / TrailLength));
                float offset = (Width - size) / 2;
                using (Brush tb = new SolidBrush(Color.FromArgb(alpha, Color)))
                    g.FillRectangle(tb, Trail[i].X + offset, Trail[i].Y + offset, size, size);
            }

            if (Mode == ModoJugador.Cube)
            {
                using (Brush b = new SolidBrush(Color))
                    g.FillRectangle(b, X, Y, Width, Height);

                using (Pen p = new Pen(Color.White, 2))
                    g.DrawRectangle(p, X, Y, Width, Height);

                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
                using (Pen pi = new Pen(Color.FromArgb(100, 255, 255, 255), 1))
                {
                    if (CubeType == 0)
                        g.DrawRectangle(pi, X + 6, Y + 6, Width - 12, Height - 12);
                    else if (CubeType == 1)
                    {
                        g.DrawLine(pi, X + Width / 2, Y + 4, X + Width / 2, Y + Height - 4);
                        g.DrawLine(pi, X + 4, Y + Height / 2, X + Width - 4, Y + Height / 2);
                    }
                    else if (CubeType == 2)
                        g.DrawEllipse(pi, X + 4, Y + 4, Width - 8, Height - 8);
                    else if (CubeType == 3)
                    {
                        using (var bb = new SolidBrush(Color.FromArgb(180, 255, 255, 255)))
                        {
                            g.FillRectangle(bb, X + 10, Y + 10, 7, 7);
                            g.FillRectangle(bb, X + Width - 17, Y + 10, 7, 7);
                        }
                    }
                    else
                        g.DrawPolygon(pi, new PointF[] {
                            new PointF(X + Width / 2, Y + 4),
                            new PointF(X + Width - 4, Y + Height / 2),
                            new PointF(X + Width / 2, Y + Height - 4),
                            new PointF(X + 4, Y + Height / 2)
                        });
                }
            }
                /// <summary>
                /// Elaborated by Godot Engine
                /// </summary>
            else
            {
                using (Brush b = new SolidBrush(Color))
                {
                    if (ShipType == 0)
                        g.FillPolygon(b, new PointF[] { new PointF(X, Y), new PointF(X + Width, Y + Height / 2), new PointF(X, Y + Height) });
                    else if (ShipType == 1)
                        g.FillPolygon(b, new PointF[] { new PointF(X, Y + 4), new PointF(X + Width - 4, Y + 12), new PointF(X + Width - 4, Y + Height - 12), new PointF(X, Y + Height - 4) });
                    else if (ShipType == 2)
                        g.FillRectangle(b, X, Y + 8, Width, Height - 16);
                    else if (ShipType == 3)
                        g.FillPolygon(b, new PointF[] { new PointF(X, Y), new PointF(X + Width, Y + Height / 2), new PointF(X, Y + Height), new PointF(X + 10, Y + Height / 2) });
                    else
                        g.FillPolygon(b, new PointF[] { new PointF(X, Y + Height / 2), new PointF(X + Width / 2, Y), new PointF(X + Width, Y + Height / 2), new PointF(X + Width / 2, Y + Height) });
                }

                using (Pen p = new Pen(Color.White, 2))
                {
                    if (ShipType == 0)
                        g.DrawPolygon(p, new PointF[] { new PointF(X, Y), new PointF(X + Width, Y + Height / 2), new PointF(X, Y + Height) });
                    else if (ShipType == 1)
                        g.DrawPolygon(p, new PointF[] { new PointF(X, Y + 4), new PointF(X + Width - 4, Y + 12), new PointF(X + Width - 4, Y + Height - 12), new PointF(X, Y + Height - 4) });
                    else if (ShipType == 2)
                        g.DrawRectangle(p, X, Y + 8, Width, Height - 16);
                    else if (ShipType == 3)
                        g.DrawPolygon(p, new PointF[] { new PointF(X, Y), new PointF(X + Width, Y + Height / 2), new PointF(X, Y + Height), new PointF(X + 10, Y + Height / 2) });
                    else
                        g.DrawPolygon(p, new PointF[] { new PointF(X, Y + Height / 2), new PointF(X + Width / 2, Y), new PointF(X + Width, Y + Height / 2), new PointF(X + Width / 2, Y + Height) });
                }
            }
        }
    }
}