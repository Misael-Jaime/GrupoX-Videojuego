using System;
using System.IO;
using System.Text.Json;

namespace GrupoX_Videojuego.Core
{
    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Sistema de guardado - maneja persistencia en JSON en la carpeta del juego
    public static class SistemaGuardado
    {
        private static readonly string SavePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "save.json");

        public static DatosGuardado Data { get; private set; } = new DatosGuardado();

        // Carga los datos desde el archivo, o crea nuevos si no existe
        public static void Load()
        {
            try
            {
                string dir = Path.GetDirectoryName(SavePath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                if (File.Exists(SavePath))
                {
                    string json = File.ReadAllText(SavePath);
                    Data = JsonSerializer.Deserialize<DatosGuardado>(json) ?? new DatosGuardado();
                }
            }
            catch { Data = new DatosGuardado(); }
            SistemaLogros.CleanObsoleteLogros(Data);
        }

        // Guarda los datos actuales al archivo
        public static void Save()
        {
            try
            {
                string dir = Path.GetDirectoryName(SavePath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var opts = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(SavePath, JsonSerializer.Serialize(Data, opts));
            }
            catch { }
        }

        public static void AddCoins(int amount)    { Data.Monedas += amount;    Save(); }
        public static void AddDiamonds(int amount) { Data.Diamonds += amount; Save(); }

        // Intenta gastar monedas, retorna false si no tiene suficiente
        public static bool SpendCoins(int amount)
        {
            if (Data.Monedas < amount) return false;
            Data.Monedas -= amount; Save(); return true;
        }

        // Intenta gastar diamantes, retorna false si no tiene suficiente
        public static bool SpendDiamonds(int amount)
        {
            if (Data.Diamonds < amount) return false;
            Data.Diamonds -= amount; Save(); return true;
        }

        public static void RecordDeath()    { Data.TotalDeaths++; Save(); }

        // Registra un nivel completado y otorga diamante si es la primera vez
        public static void RecordLevelComplete(int levelId, int coinsMask)
        {
            bool alreadyDone = Data.LevelCompleted.ContainsKey(levelId) && Data.LevelCompleted[levelId];
            Data.LevelCompleted[levelId] = true;
            Data.LevelMonedasCollected[levelId] = coinsMask;
            if (!alreadyDone)
            {
                Data.TotalLevelsCompleted++;
                AddDiamonds(1);
            }
            Save();
        }

        // Actualiza el mejor porcentaje si es mayor al anterior
        public static void UpdateBestPercent(int levelId, int percent)
        {
            if (!Data.LevelBestPercent.ContainsKey(levelId) || Data.LevelBestPercent[levelId] < percent)
            {
                Data.LevelBestPercent[levelId] = percent;
                Save();
            }
        }
    }
}