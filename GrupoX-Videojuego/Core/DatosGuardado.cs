using System.Drawing;
using System.Collections.Generic;

namespace GrupoX_Videojuego.Core
{
    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Datos guardados del jugador 
    public class DatosGuardado
    {
        public int Monedas { get; set; } = 0;
        public int Diamonds { get; set; } = 0;

        // Listas de objetos desbloqueados (indices de skins)
        public List<int> UnlockedCubes { get; set; } = new() { 0 };
        public List<int> UnlockedShips { get; set; } = new() { 0 };
        public List<int> UnlockedColors { get; set; } = new() { 0 };
        public int SelectedCube { get; set; } = 0;
        public int SelectedShip { get; set; } = 0;
        public int SelectedColorIndex { get; set; } = 0;

        // Progreso por nivel (mejor porcentaje, completado, monedas recolectadas)
        public Dictionary<int, int> LevelBestPercent { get; set; } = new();
        public Dictionary<int, bool> LevelCompleted { get; set; } = new();
        public Dictionary<int, int> LevelMonedasCollected { get; set; } = new();

        public int HighestWorldUnlocked { get; set; } = 1;

        // Estadisticas globales del jugador
        public int TotalDeaths { get; set; } = 0;
        public int TotalJumps { get; set; } = 0;
        public int TotalLevelsCompleted { get; set; } = 0;
        public int TotalMonedasCollected { get; set; } = 0;
        public long TotalPlayTimeSeconds { get; set; } = 0;
        public int HighScore { get; set; } = 0;

        public List<string> UnlockedLogros { get; set; } = new();

        // Configuracion de audio y ajustes
        public float MusicVolume { get; set; } = 0.8f;
        public bool Fullscreen { get; set; } = false;
        public string Language { get; set; } = "es";
        public bool LowParticulas { get; set; } = false;

        // Flags temporales para verificacion de logros
        public bool CompletedLevelWithoutDying { get; set; } = false;
        public bool HasEnteredShipMode { get; set; } = false;
    }
}