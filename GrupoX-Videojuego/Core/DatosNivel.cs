using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GrupoX_Videojuego.Models;

namespace GrupoX_Videojuego.Core
{
    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Dificultad de cada nivel (usada para clasificar visualmente)
    public enum DificultadNivel { Easy, Normal, Hard, Harder, Insane, Demon, ExtremeDemon }

    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Representa un tile individual del mapa de nivel
    public class TileNivel
    {
        public char Type;
        public float X, Y;
    }

    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Datos de un nivel: filas de tiles, colores, metadata
    public class DatosNivel
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public string Name { get; set; } = "";
        public string Author { get; set; } = "GeoStorm";
        public DificultadNivel Difficulty { get; set; }
        public Color BackgroundColor { get; set; }
        public Color GroundColor { get; set; }
        public Color ObjectColor { get; set; }
        public string[] Rows { get; set; } = System.Array.Empty<string>();
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    public static class BaseNiveles
    {
        public static readonly List<DatosNivel> All = new()
        {
            new DatosNivel
            {
                Id = 1, WorldId = 1, Name = "Stereo Madness", Difficulty = DificultadNivel.Easy,
                BackgroundColor = Color.FromArgb(15, 10, 40),
                GroundColor = Color.FromArgb(50, 60, 80),
                ObjectColor = Color.OrangeRed,
                Rows = new[]
                {
                    "                                                                                               ",
                    "                                             C       C       C                                 ",
                    "                   ##  ##                                                                      ",
                    "                                                 #^      #^      #                              E   ",
                    " #################    ################################################################################################################################################",
                }
            },
            new DatosNivel
            {
                Id = 2, WorldId = 1, Name = "Back On Track", Difficulty = DificultadNivel.Easy,
                BackgroundColor = Color.FromArgb(10, 30, 50),
                GroundColor = Color.FromArgb(20, 80, 100),
                ObjectColor = Color.CornflowerBlue,
                Rows = new[]
                {
                    "                                                                                               ",
                    "                                                                             C                 ",
                    "                                 C                                                             ",
                    "                         ##      ^         P                                              Q                  E",
                    " #############################################################################################################################################################",
                }
            },
            new DatosNivel
            {
                Id = 3, WorldId = 2, Name = "Polargeist", Difficulty = DificultadNivel.Normal,
                BackgroundColor = Color.FromArgb(5, 15, 35),
                GroundColor = Color.FromArgb(30, 50, 90),
                ObjectColor = Color.MediumPurple,
                Rows = new[]
                {
                    "                                                               ######                                              ",
                    "                                                   ############        ############                                ",
                    "                                                                                                   C               ",
                    "                          ##      ^       ##    P                              ####                     ##   Q                 E ",
                    "###############################################################################################################################################################"
                }
            },
            new DatosNivel
            {
                Id = 4, WorldId = 2, Name = "Dry Out", Difficulty = DificultadNivel.Normal,
                BackgroundColor = Color.FromArgb(40, 20, 5),
                GroundColor = Color.FromArgb(80, 50, 10),
                ObjectColor = Color.SandyBrown,
                Rows = new[]
                {
                    "                                                                                                                        ",
                    "                                                    ###                               C                                  ",
                    "                                              ###   ###           ###     ##          ####                               ",
                    "                          ^       ##     ^    ###           ##    ###                             E   ",
                    "########################################################################################################################################################################"
                }
            },
            new DatosNivel
            {
                Id = 5, WorldId = 3, Name = "Base After Base", Difficulty = DificultadNivel.Hard,
                BackgroundColor = Color.FromArgb(10, 30, 20),
                GroundColor     = Color.FromArgb(20, 70, 40),
                ObjectColor     = Color.LimeGreen,
                Rows = new[]
                {
                    "                                                                                                         ",
                    "                              C                                       C                   C             ",
                    "                             ##                                     ##                  ##             ",
                    "                             ##   ^     ##    P                                      Q    ^     ##    ^             ##         E   ",
                    "###########################################################################################################################################################"
                }
            },
            new DatosNivel
            {
                Id = 6, WorldId = 3, Name = "Can't Let Go", Difficulty = DificultadNivel.Hard,
                BackgroundColor = Color.FromArgb(25, 10, 40),
                GroundColor     = Color.FromArgb(60, 30, 80),
                ObjectColor     = Color.Orchid,
                Rows = new[]
                {
                    "                                                                                                              ",
                    "                               C                      C                         C             C             ",
                    "                              ###                                              ###           ###             ",
                    "                             ###   ^     ##   P                                   Q    ##   ^         ###   ^       ###           E    ",
                    "#######################################################################################################################################################"
                }
            },
            new DatosNivel
            {
                Id = 7, WorldId = 3, Name = "Jumper", Difficulty = DificultadNivel.Harder,
                BackgroundColor = Color.FromArgb(15, 40, 55),
                GroundColor     = Color.FromArgb(10, 90, 120),
                ObjectColor     = Color.Cyan,
                Rows = new[]
                {
                    "                                                                                                                   ",
                    "                        C                   C                    C                   C              C             ",
                    "                       ##        ##                              ##        ##         ##            ##             ",
                    "                       ##   ^    ##  P                           Q  ##   ^      ##   ^    ##   ^     ##  ^                 E   ",
                    "#####################################################################################################################################################"
                }
            },
            new DatosNivel
            {
                Id = 8, WorldId = 4, Name = "Time Machine", Difficulty = DificultadNivel.Harder,
                BackgroundColor = Color.FromArgb(40, 10, 10),
                GroundColor     = Color.FromArgb(100, 20, 20),
                ObjectColor     = Color.Tomato,
                Rows = new[]
                {
                    "                                                                                                                      ",
                    "                         C                   C                       C                   C              C            ",
                    "                        ##       ##                                  ##       ##          ##            ##            ",
                    "                        ##  ^    ##   P                           Q  ##  ^          ##  ^    ##   ^      ##  ^                 E  ",
                    "###########################################################################################################################################################"
                }
            },
            new DatosNivel
            {
                Id = 9, WorldId = 4, Name = "Cycles", Difficulty = DificultadNivel.Insane,
                BackgroundColor = Color.FromArgb(5, 5, 50),
                GroundColor     = Color.FromArgb(15, 15, 100),
                ObjectColor     = Color.DeepSkyBlue,
                Rows = new[]
                {
                    "                                                                                                                           ",
                    "                         C                   C                    C                   C                C                  ",
                    "                        ###       ##                              ###       ##        ###             ###       ##         ",
                    "                        ###  ^    ##  P                           Q  ##  ^      ###  ^    ##   ^    ###  ^          ###  ^          E",
                    "################################################################################################################################################################"
                }
            },
            new DatosNivel
            {
                Id = 10, WorldId = 5, Name = "xStep", Difficulty = DificultadNivel.Insane,
                BackgroundColor = Color.FromArgb(30, 30, 10),
                GroundColor     = Color.FromArgb(70, 70, 20),
                ObjectColor     = Color.Yellow,
                Rows = new[]
                {
                    "                                                                                                                                ",
                    "                          C                  C                   C                  C                C              C          ",
                    "                         ###      ##                             ###      ##        ###             ###            ###           ",
                    "                         ###  ^   ##  P                              Q   ##  ^     ###  ^   ##   ^    ###  ^          ###  ^                  E  ",
                    "###############################################################################################################################################################################"
                }
            },
            new DatosNivel
            {
                Id = 11, WorldId = 5, Name = "Clutterfunk", Difficulty = DificultadNivel.Insane,
                BackgroundColor = Color.FromArgb(20, 10, 30),
                GroundColor     = Color.FromArgb(50, 20, 70),
                ObjectColor     = Color.Fuchsia,
                Rows = new[]
                {
                    "                                                                                                                                      ",
                    "                           C                  C                    C                   C                  C              C            ",
                    "                          ###      ##                              ###       ##        ###      ##        ###            ###             ",
                    "                          ###  ^   ##  P                             ^   ##  Q  ^   ###  ^    ##  ^     ###  ^   ##  ^     ###  ^                    E   ",
                    "#####################################################################################################################################################################################"
                }
            },
            new DatosNivel
            {
                Id = 12, WorldId = 5, Name = "Theory of Everything", Difficulty = DificultadNivel.Demon,
                BackgroundColor = Color.FromArgb(20, 0, 0),
                GroundColor     = Color.FromArgb(60, 0, 0),
                ObjectColor     = Color.Red,
                Rows = new[]
                {
                    "                                                                                                                                            ",
                    "                           C                 C                   C                   C                 C                  C                 ",
                    "                          ###      ##                            ###       ##        ###      ##        ###      ##        ###                ",
                    "                          ###   ^  ##  P                            Q  ##  ^      ###   ^   ##  ^     ###   ^  ##  ^     ###   ^  ##  ^                        E",
                    "###############################################################################################################################################################################################"
                }
            },
        };

        public static DatosNivel? GetById(int id) => All.Find(l => l.Id == id);

        public static List<DatosNivel> GetByWorld(int worldId) => All.FindAll(l => l.WorldId == worldId);
    }

    /// <summary>
    /// Elaborated by Godot Engine
    /// </summary>
    public static class CargadorNiveles
    {
        private const float TileSize = 40f;

        public static (List<Obstaculo> obstacles, List<Portal> portals, List<Moneda> coins,
                       List<Plataforma> platforms, float spawnX, float spawnY, float endX)
            Load(DatosNivel level, float groundY)
        {
            var obstacles = new List<Obstaculo>();
            var portals   = new List<Portal>();
            var coins     = new List<Moneda>();
            var platforms = new List<Plataforma>();

            float spawnX = TileSize, spawnY = groundY - TileSize;
            float endX = 800;
            int rows = level.Rows.Length;

            for (int row = 0; row < rows; row++)
            {
                string line = level.Rows[row];
                float tileY = groundY - (rows - row - 1) * TileSize;

                for (int col = 0; col < line.Length; col++)
                {
                    char ch = line[col];
                    float tileX = col * TileSize;

                    switch (ch)
                    {
                        case '#': 
                            platforms.Add(new Plataforma(tileX, tileY, TileSize, TileSize, level.GroundColor));
                            break;
                        case '^': 
                            float visX = tileX;
                            float visY = tileY + TileSize * 0.2f;
                            float visW = TileSize * 0.8f;
                            float visH = TileSize * 0.8f;

                            float hitX = tileX + TileSize * 0.25f;
                            float hitY = visY + visH * 0.3f;
                            float hitW = TileSize * 0.5f;
                            float hitH = visH * 0.6f;

                            obstacles.Add(new Obstaculo(hitX, hitY, hitW, hitH, level.ObjectColor, 0, visX, visY, visW, visH));
                            break;
                        case 'C': 
                            coins.Add(new Moneda(tileX + TileSize / 2 - 10, tileY, 20, 20));
                            break;
                        case 'P': 
                            portals.Add(new Portal(tileX, tileY - 10 * TileSize, TileSize / 2, TileSize * 11, Color.FromArgb(100, 200, 255), ModoJugador.Ship, 0));
                            break;
                        case 'Q': 
                            portals.Add(new Portal(tileX, tileY - 10 * TileSize, TileSize / 2, TileSize * 11, Color.FromArgb(100, 255, 120), ModoJugador.Cube, 0));
                            break;
                        case 'S': 
                            spawnX = tileX; spawnY = tileY;
                            break;
                        case 'E': 
                            endX = tileX;
                            break;
                    }
                }
            }

            /// <summary>
            /// Elaborated by Godot Engine
            /// </summary>
            var orderedPortals = portals.OrderBy(p => p.X).ToList();
            float currentShipStart = -1;
            var shipSegments = new List<(float start, float end)>();

            foreach (var p in orderedPortals)
            {
                if (p.TargetMode == ModoJugador.Ship && currentShipStart == -1)
                {
                    currentShipStart = p.X;
                }
                else if (p.TargetMode == ModoJugador.Cube && currentShipStart != -1)
                {
                    shipSegments.Add((currentShipStart, p.X));
                    currentShipStart = -1;
                }
            }
            if (currentShipStart != -1)
            {
                shipSegments.Add((currentShipStart, endX + 800f));
            }

            float ceilingY = groundY - 11 * TileSize;
            float midCeilingY = groundY - 8 * TileSize;
            var rng = new Random(level.Id * 7);
            foreach (var seg in shipSegments)
            {
                float segLen = seg.end - seg.start;
                int totalTiles = (int)(segLen / TileSize);
                int tileIdx = 0;

                while (tileIdx < totalTiles)
                {
                    int blockLen = 5 + rng.Next(0, 4);
                    for (int j = 0; j < blockLen && tileIdx < totalTiles; j++, tileIdx++)
                    {
                        float x = seg.start + tileIdx * TileSize;
                        platforms.Add(new Plataforma(x, ceilingY, TileSize, TileSize, level.GroundColor));
                    }
                    int gapLen = 2 + rng.Next(0, 2);
                    tileIdx += gapLen;
                }

                int midCount = Math.Max(2, totalTiles / 10);
                for (int m = 0; m < midCount; m++)
                {
                    float mx = seg.start + (m + 1) * segLen / (midCount + 1);
                    float my = m % 2 == 0 ? midCeilingY : midCeilingY + TileSize;
                    platforms.Add(new Plataforma(mx, my, TileSize, TileSize, level.GroundColor));
                }
            }

            return (obstacles, portals, coins, platforms, spawnX, spawnY, endX);
        }
    }
}