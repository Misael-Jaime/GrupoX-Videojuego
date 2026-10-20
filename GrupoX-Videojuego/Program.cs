using System;
using System.Windows.Forms;
using GrupoX_Videojuego.Core;
using GrupoX_Videojuego.UI;

namespace GrupoX_Videojuego
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            SistemaGuardado.Load();
            SistemaLogros.CheckAndUnlock(SistemaGuardado.Data);
            SistemaGuardado.Save();
            Application.Run(new Menu());
        }
    }
}