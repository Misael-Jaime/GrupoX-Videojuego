namespace GrupoX_Videojuego.Core
{
    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Clase con todas las traducciones del juego (español / ingles)
    // Se usa T.Llave para acceder a cada string
    public static class T
    {
        private static bool IsEn => SistemaGuardado.Data.Language == "en";

        // Menu principal
        public static string Play => IsEn ? "PLAY" : "JUGAR";
        public static string Shop => IsEn ? "SHOP" : "TIENDA";
        public static string Character => IsEn ? "CHARACTER" : "PERSONAJE";
        public static string Achievements => IsEn ? "ACHIEVEMENTS" : "LOGROS";
        public static string Settings => IsEn ? "SETTINGS" : "AJUSTES";
        public static string Profile => IsEn ? "PROFILE" : "PERFIL";
        public static string Statistics => IsEn ? "STATISTICS" : "ESTADISTICAS";
        public static string Credits => IsEn ? "CREDITS" : "CREDITOS";
        public static string Quit => IsEn ? "QUIT GAME" : "SALIR DEL JUEGO";

        // Navegacion comun
        public static string Back => IsEn ? "← BACK (ESC)" : "← VOLVER (ESC)";
        public static string Locked => IsEn ? "Locked" : "Bloqueado";
        public static string Equip => IsEn ? "Equip" : "Equipar";
        public static string Active => IsEn ? "Active" : "Activo";
        public static string NoFunds(int p) => IsEn
            ? $"Not enough coins! ({p} coins needed)"
            : $"No tienes suficientes monedas! ({p} monedas)";
        public static string NoFundsDiamond(int p) => IsEn
            ? $"Not enough diamonds! ({p} diamonds needed)"
            : $"No tienes suficientes diamantes! ({p} diamantes)";
        public static string NoFundsTitle => IsEn ? "No Funds" : "Sin fondos";
        public static string NoFundsBoth(int c, int d) => IsEn
            ? $"Not enough funds! ({c} coins + {d} diamonds needed)"
            : $"No tienes suficientes fondos! ({c} monedas + {d} diamantes)";
        public static string Coins_ => IsEn ? "Coins" : "Monedas";
        public static string Diamonds_ => IsEn ? "Diamonds" : "Diamantes";

        // Tienda
        public static string ShopTitle => IsEn ? "SHOP" : "TIENDA";
        public static string TabCubes => IsEn ? "Cubes" : "Cubos";
        public static string TabShips => IsEn ? "Ships" : "Naves";
        public static string TabColors => IsEn ? "Colors" : "Colores";
        public static string Model(int i) => IsEn ? $"Model {i+1}" : $"Modelo {i+1}";
        public static string Price(int p) => IsEn ? $"{p} coins" : $"{p} monedas";
        public static string PriceDiamond(int p) => IsEn ? $"{p} diamonds" : $"{p} diamantes";

        // Ajustes
        public static string SettingsTitle => IsEn ? "SETTINGS" : "AJUSTES";
        public static string MusicVolume => IsEn ? "Music Volume" : "Volumen Musica";
        public static string Particles => IsEn ? "Particles" : "Particulas";
        public static string ParticlesNormal => IsEn ? "Normal" : "Normales";
        public static string ParticlesLow => IsEn ? "Low" : "Reducidas";
        public static string Language_ => IsEn ? "Language" : "Idioma";

        // Estadisticas
        public static string StatsTitle => IsEn ? "STATISTICS" : "ESTADISTICAS";
        public static string StatLevels => IsEn ? "Levels Completed" : "Niveles Completados";
        public static string StatDeaths => IsEn ? "Total Deaths" : "Muertes Totales";
        public static string StatJumps => IsEn ? "Total Jumps" : "Saltos Totales";
        public static string StatCoins => IsEn ? "Coins Collected" : "Monedas Recogidas";
        public static string StatDiamonds => IsEn ? "Diamonds" : "Diamantes";
        public static string StatPlayTime => IsEn ? "Play Time" : "Tiempo Jugado";
        public static string StatHighScore => IsEn ? "High Score" : "Mejor Puntuacion";
        public static string StatAchievements => IsEn ? "Achievements Unlocked" : "Logros Desbloqueados";
        public static string StatHighestWorld => IsEn ? "Highest World" : "Mundo Mas Alto";

        // Logros
        public static string AchTitle => IsEn ? "ACHIEVEMENTS" : "LOGROS";
        public static string AchUnlocked(int u, int t) => IsEn
            ? $"{u} / {t} unlocked"
            : $"{u} / {t} desbloqueados";
        public static string AchBlocked => IsEn ? "[Locked]" : "[Bloqueado]";

        // Personaje
        public static string CharTitle => IsEn ? "CHARACTER" : "PERSONAJE";
        public static string CharCurrent => IsEn ? "Your current character" : "Tu personaje actual";
        public static string CharVisitShop => IsEn ? "Visit SHOP to change skin" : "Visita la TIENDA para cambiar skin";

        // Perfil
        public static string ProfileTitle => IsEn ? "PROFILE" : "PERFIL";
        public static string ProfilePlayer => IsEn ? "GeoStorm Player" : "GeoStorm Player";
        public static string ProfileLevel(int l) => IsEn ? $"Level: {l}" : $"Nivel: {l}";
        public static string ProfileXP(int x) => IsEn ? $"Experience: {x} XP" : $"Experiencia: {x} XP";
        public static string ProfileAchs(int u, int t) => IsEn ? $"Achievements: {u} / {t}" : $"Logros: {u} / {t}";
        public static string ProfileOnline => IsEn ? "(Online system coming soon)" : "(Sistema online proximamente)";

        // Creditos
        public static string CreditsTitle => IsEn ? "CREDITS" : "CREDITOS";
        public static string CreditsEngine => IsEn ? "Built with C# + Windows Forms + GDI+" : "Desarrollado con C# + Windows Forms + GDI+";
        public static string CreditsProject => IsEn ? "Semester Project — 1SG121" : "Proyecto Semestral — 1SG121";
        public static string CreditsFacilitator => IsEn ? "Facilitator: Pablo Delgado" : "Facilitador: Pablo Delgado";

        // Mapa de mundos
        public static string WorldMapTitle => IsEn ? "Select a World" : "Selecciona un Mundo";
        public static string WorldUnlocked(int w) => IsEn
            ? $"Highest world unlocked: {w}"
            : $"Mundo mas alto desbloqueado: {w}";
        public static string WorldLocked => IsEn ? "Complete previous world first!" : "Completa el mundo anterior primero!";
        public static string WorldLockedTitle => IsEn ? "Locked" : "Bloqueado";
        public static string WorldNum(int w) => IsEn ? $"World {w}" : $"Mundo {w}";

        public static string LevelCompleted => IsEn ? "Completed" : "Completado";
        public static string LevelNoAttempts => IsEn ? "No attempts" : "Sin intentos";

        // HUD del juego
        public static string HudShip => IsEn ? "✈ SHIP" : "✈ NAVE";
        public static string HudCube => IsEn ? "■ CUBE" : "■ CUBO";
        public static string PressToPlay => IsEn ? "Press SPACE or Click to Play" : "Presiona ESPACIO o Haz Clic para Jugar";

        // Traduccion de nombres de dificultad
        public static string Difficulty(string d) => IsEn ? d : d switch
        {
            "Easy" => "Facil",
            "Normal" => "Normal",
            "Hard" => "Dificil",
            "Harder" => "Mas Dificil",
            "Insane" => "Insano",
            "Demon" => "Demonio",
            "ExtremeDemon" => "Demonio Extremo",
            _ => d
        };
    }
}