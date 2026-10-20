using System.Collections.Generic;

namespace GrupoX_Videojuego.Core
{
    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Categorias para clasificar los logros en la interfaz
    public enum CategoriaLogro { General, Gameplay, Collection }

    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Un logro individual con nombre en español e ingles
    public class Logro
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public int DiamondReward { get; set; }
        public CategoriaLogro Category { get; set; }

        // Verifica si el logro ya esta desbloqueado
        public bool IsUnlocked(DatosGuardado d) => d.UnlockedLogros.Contains(Id);

        // Retorna el nombre y descripcion segun el idioma actual
        public string DisplayName => SistemaGuardado.Data.Language == "en" ? NameEn : Name;
        public string DisplayDesc => SistemaGuardado.Data.Language == "en" ? DescriptionEn : Description;

        public Logro(string id, string nameEs, string nameEn, string descEs, string descEn, int reward, CategoriaLogro cat = CategoriaLogro.General)
        {
            Id = id;
            Name = nameEs;
            NameEn = nameEn;
            Description = descEs;
            DescriptionEn = descEn;
            DiamondReward = reward;
            Category = cat;
        }
    }

    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Sistema que define y verifica todos los logros del juego
    public static class SistemaLogros
    {
        public static readonly List<Logro> All = new()
        {
            new("first_play", "Primer Vuelo", "First Flight", "Inicia el juego por primera vez", "Start the game for the first time", 1),
            new("die_once", "Primer Choque", "First Crash", "Muere por primera vez", "Die for the first time", 1),
            new("die_10", "Aprendiendo", "Learning", "Muere 10 veces", "Die 10 times", 1),
            new("die_100", "Dedicacion Total", "Total Dedication", "Muere 100 veces", "Die 100 times", 2),
            new("die_1000", "Masoquista", "Masochist", "Muere 1000 veces", "Die 1000 times", 5),
            new("jump_100", "Saltarin", "Jumper", "Salta 100 veces", "Jump 100 times", 1),
            new("jump_1000", "Saltamontes", "Grasshopper", "Salta 1000 veces", "Jump 1000 times", 2),
            new("jump_10000", "Rebote Infinito", "Infinite Bounce", "Salta 10,000 veces", "Jump 10,000 times", 5),
            new("play_1h", "Una Hora", "One Hour", "Juega 1 hora en total", "Play for 1 hour total", 2),
            new("play_10h", "Maratonista", "Marathoner", "Juega 10 horas en total", "Play for 10 hours total", 5),
            new("play_50h", "Adicto", "Addicted", "Juega 50 horas en total", "Play for 50 hours total", 10),
            new("level1_done", "Nivel 1", "Level 1", "Completa el Nivel 1", "Complete Level 1", 2, CategoriaLogro.Gameplay),
            new("level2_done", "Nivel 2", "Level 2", "Completa el Nivel 2", "Complete Level 2", 2, CategoriaLogro.Gameplay),
            new("level12_done", "Nivel Final", "Final Level", "Completa el Nivel Final", "Complete the Final Level", 5, CategoriaLogro.Gameplay),

        };

        // Elimina logros que ya no existen en la base actual
        public static void CleanObsoleteLogros(DatosGuardado data)
        {
            data.UnlockedLogros.RemoveAll(id => !All.Exists(a => a.Id == id));
        }

        // Verifica todas las condiciones y retorna los logros recien desbloqueados
        public static List<Logro> CheckAndUnlock(DatosGuardado data)
        {
            var newlyUnlocked = new List<Logro>();
            foreach (var ach in All)
            {
                if (ach.IsUnlocked(data)) continue;

                // Evalua si se cumple la condicion del logro
                bool shouldUnlock = ach.Id switch
                {
                    "first_play" => true,
                    "die_once" => data.TotalDeaths >= 1,
                    "die_10" => data.TotalDeaths >= 10,
                    "die_100" => data.TotalDeaths >= 100,
                    "die_1000" => data.TotalDeaths >= 1000,
                    "jump_100" => data.TotalJumps >= 100,
                    "jump_1000" => data.TotalJumps >= 1000,
                    "jump_10000" => data.TotalJumps >= 10000,
                    "play_1h" => data.TotalPlayTimeSeconds >= 3600,
                    "play_10h" => data.TotalPlayTimeSeconds >= 36000,
                    "play_50h" => data.TotalPlayTimeSeconds >= 180000,
                    "level1_done" => data.LevelCompleted.ContainsKey(1) && data.LevelCompleted[1],
                    "level2_done" => data.LevelCompleted.ContainsKey(2) && data.LevelCompleted[2],
                    "level12_done" => data.LevelCompleted.ContainsKey(12) && data.LevelCompleted[12],
                    _ => false
                };
                if (shouldUnlock)
                {
                    data.UnlockedLogros.Add(ach.Id);
                    data.Diamonds += ach.DiamondReward;
                    newlyUnlocked.Add(ach);
                }
            }
            return newlyUnlocked;
        }
    }
}