# 🎮 Videojuego 2D de Plataformas

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Windows Forms](https://img.shields.io/badge/Windows%20Forms-0078D7?style=for-the-badge&logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/winforms/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

> Videojuego interactivo de plataformas en 2D desarrollado en **C#**, implementando un motor de físicas propio, renderizado gráfico en tiempo real, detección de colisiones, sistema de mundos y persistencia de partidas.

---

## 📖 Descripción del Proyecto

Este videojuego es una demostración práctica del diseño y desarrollo de arquitecturas de software orientadas a videojuegos utilizando tecnologías .NET. 

El jugador controla a un personaje a través de diversos niveles y mundos desafiantes, superando obstáculos, recolectando monedas y resolviendo mecánicas de plataformas con transiciones fluidas.

### Características Principales:
* **Motor de Juego Propio (`MotorJuego`):** Ciclo de juego (*Game Loop*), física de salto, gravedad, inercia y detección de colisiones AABB.
* **Sistema de Niveles y Mundos:** Progresión con múltiples etapas y portales de transición.
* **Sistema de Guardado (`SistemaGuardado`):** Almacenamiento y carga de progreso, estado de monedas y récords.
* **Sistema de Logros (`SistemaLogros`):** Desbloqueo dinámico de recompensas por desempeño.
* **Gestor de Audio (`GestorAudio`):** Efectos de sonido y ambientación sonora.
* **Interfaz de Usuario (`UI`):** Menú interactivo, selección de mundos, HUD en vivo y diálogos.

---

## 🛠️ Tecnologías Utilizadas

* **Lenguaje:** C# (.NET)
* **Motor / Gráficos:** GDI+ / Windows Forms con doble búfer (Double Buffering) para renderizado sin parpadeos (*anti-flicker*).
* **Arquitectura:** Patrón orientado a objetos modular (separación entre `Core`, `Models` y `UI`).
* **Control de Versiones:** Git y GitHub

---

## 🕹️ Controles del Juego

| Acción | Tecla |
| :--- | :--- |
| **Moverse a la Izquierda** | `A` o Flecha Izquierda `←` |
| **Moverse a la Derecha** | `D` o Flecha Derecha `→` |
| **Saltar** | Barra Espaciadora `Espacio` o `W` |
| **Pausar Juego** | `Esc` o `P` |

---

## ⚙️ Instrucciones de Instalación y Ejecución

### Prerrequisitos
* Sistema Operativo: Windows 10 / 11
* SDK de [.NET 6 / 8](https://dotnet.microsoft.com/download) o Visual Studio 2022.

### Pasos para ejecutar:
1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/Misael-Jaime/GrupoX-Videojuego.git
   cd GrupoX-Videojuego
   ```

2. **Abrir la solución:**
   * Haz doble clic en `GrupoX-Videojuego.sln`.

3. **Compilar y Correr:**
   * En Visual Studio presiona `F5` o el botón **Iniciar**.
   * O por terminal dentro de la carpeta:
     ```bash
     dotnet run --project GrupoX-Videojuego/GrupoX-Videojuego.csproj
     ```

---

## 👤 Autor

* **Misael Jaime** - [GitHub](https://github.com/Misael-Jaime) • [LinkedIn](https://www.linkedin.com/in/misael-jaime/)
* **Universidad Tecnológica de Panamá (UTP)**

---

## 📄 Licencia

Este proyecto está bajo la Licencia **MIT** - consulta el archivo `LICENSE` para más detalles.
