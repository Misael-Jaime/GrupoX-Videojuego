using System;
using System.IO;
using NAudio.Wave;

namespace GrupoX_Videojuego.Core
{
    /// <summary>
    /// Verified by Godot Engine
    /// </summary>
    // Maneja toda la reproduccion de audio: musica de menu, musica de juego, y efectos
    public static class GestorAudio
    {
        private static WaveOutEvent? _menuOut;
        private static WaveOutEvent? _gameOut;
        private static AudioFileReader? _menuFile;
        private static AudioFileReader? _gameFile;
        private static bool _initialized = false;

        private static bool _menuLooping = false;
        private static bool _gameLooping = false;

        // Inicializa los dispositivos de audio (solo la primera vez)
        public static void Initialize()
        {
            if (_initialized) return;
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds");
                string menuPath = Path.Combine(dir, "menu_music.wav");
                string gamePath = Path.Combine(dir, "game_music.wav");

                if (File.Exists(menuPath))
                {
                    _menuFile = new AudioFileReader(menuPath) { Volume = SistemaGuardado.Data.MusicVolume };
                    _menuOut = new WaveOutEvent();
                    _menuOut.Init(_menuFile);
                    _menuOut.PlaybackStopped += OnMenuMusicStopped;
                }

                if (File.Exists(gamePath))
                {
                    _gameFile = new AudioFileReader(gamePath) { Volume = SistemaGuardado.Data.MusicVolume };
                    _gameOut = new WaveOutEvent();
                    _gameOut.Init(_gameFile);
                    _gameOut.PlaybackStopped += OnGameMusicStopped;
                }

                _initialized = true;
            }
            catch { }
        }

        // Evento que se ejecuta cuando la musica del menu termina - la repite si debe seguir sonando
        private static void OnMenuMusicStopped(object? sender, StoppedEventArgs e)
        {
            if (_menuLooping && _menuFile != null && _menuOut != null)
            {
                try
                {
                    _menuFile.Position = 0;
                    _menuOut.Play();
                }
                catch { }
            }
        }

        // Evento que se ejecuta cuando la musica del juego termina - la repite si debe seguir sonando
        private static void OnGameMusicStopped(object? sender, StoppedEventArgs e)
        {
            if (_gameLooping && _gameFile != null && _gameOut != null)
            {
                try
                {
                    _gameFile.Position = 0;
                    _gameOut.Play();
                }
                catch { }
            }
        }

        // Reproduce musica del menu (detiene la anterior primero)
        public static void PlayMenuMusic()
        {
            Initialize();
            try
            {
                _gameOut?.Stop();
                _gameLooping = false;
                if (_menuFile != null && _menuOut != null)
                {
                    _menuFile.Volume = SistemaGuardado.Data.MusicVolume;
                    _menuFile.Position = 0;
                    _menuLooping = true;
                    _menuOut.Play();
                }
            }
            catch { }
        }

        // Reproduce musica del juego (detiene la anterior primero)
        public static void PlayGameMusic()
        {
            Initialize();
            try
            {
                _menuOut?.Stop();
                _menuLooping = false;
                if (_gameFile != null && _gameOut != null)
                {
                    _gameFile.Volume = SistemaGuardado.Data.MusicVolume;
                    _gameFile.Position = 0;
                    _gameLooping = true;
                    _gameOut.Play();
                }
            }
            catch { }
        }

        private const int POOL_SIZE = 5;
        private static WaveOutEvent[] _deathOuts = new WaveOutEvent[POOL_SIZE];
        private static AudioFileReader[] _deathFiles = new AudioFileReader[POOL_SIZE];
        private static DateTime[] _deathPlayTimes = new DateTime[POOL_SIZE];
        private static int _deathIdx = 0;

        // Reproduce sonido de muerte 
        public static void PlayDeathSound()
        {
            Initialize();
            try
            {
                // Inicializa el pool la primera vez
                if (_deathOuts[0] == null)
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "death_sound.wav");
                    if (!File.Exists(path)) return;
                    for (int i = 0; i < POOL_SIZE; i++)
                    {
                        _deathFiles[i] = new AudioFileReader(path) { Volume = SistemaGuardado.Data.MusicVolume };
                        _deathOuts[i] = new WaveOutEvent();
                        _deathOuts[i].Init(_deathFiles[i]);
                    }
                }

                // Busca un reproductor que ya haya terminado de sonar 
                int freeIdx = -1;
                for (int i = 0; i < POOL_SIZE; i++)
                {
                    int check = (_deathIdx + i + 1) % POOL_SIZE;
                    if ((DateTime.Now - _deathPlayTimes[check]).TotalSeconds >= 1.2)
                    {
                        freeIdx = check;
                        break;
                    }
                }

                // Si encontramos uno libre (siempre deberia haber uno), lo usamos
                if (freeIdx != -1)
                {
                    _deathIdx = freeIdx;
                    _deathFiles[_deathIdx].Position = 0;
                    _deathFiles[_deathIdx].Volume = SistemaGuardado.Data.MusicVolume;
                    _deathOuts[_deathIdx].Play();
                    _deathPlayTimes[_deathIdx] = DateTime.Now;
                }
            }
            catch { }
        }

        // Detiene y dispone un dispositivo de audio de forma segura
        private static void SafeDispose(ref WaveOutEvent? output, ref AudioFileReader? file)
        {
            try { output?.Stop(); } catch { }
            try { output?.Dispose(); } catch { }
            try { file?.Dispose(); } catch { }
            output = null;
            file = null;
        }

        // Actualiza el volumen en tiempo real desde los ajustes
        public static void UpdateVolumes()
        {
            if (_menuFile != null) _menuFile.Volume = SistemaGuardado.Data.MusicVolume;
            if (_gameFile != null) _gameFile.Volume = SistemaGuardado.Data.MusicVolume;
        }

        public static void StopAll()
        {
            _menuLooping = false;
            _gameLooping = false;
            _menuOut?.Stop();
            _gameOut?.Stop();
        }

        public static void StopGameMusic()
        {
            _gameLooping = false;
            _gameOut?.Stop();
        }

        // Libera todos los recursos de audio
        public static void Dispose()
        {
            _menuLooping = false;
            _gameLooping = false;
            _menuOut?.Stop(); _menuOut?.Dispose();
            _gameOut?.Stop(); _gameOut?.Dispose();
            _menuFile?.Dispose(); _gameFile?.Dispose();
            if (_deathOuts != null)
            {
                for (int i = 0; i < POOL_SIZE; i++)
                {
                    _deathOuts[i]?.Stop(); _deathOuts[i]?.Dispose();
                    _deathFiles[i]?.Dispose();
                }
            }
        }
    }
}