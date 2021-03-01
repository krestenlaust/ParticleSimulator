using System.Numerics;
using System.Diagnostics;
using System.Threading;
using ConsoleInput;
using ParticleEngine;
using ParticleEngine.Particles;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define console screen sizes to allow easy line wrapping.
            ScreenBuffer.Setup(100, 45);

            // Call Windows API to enable our specific console needs.
            WinAPI.EnableANSIProcessing();
            //WinAPI.SetFontSize(10, 10);

            // Set up the window style
            SetupStyle();

            // Call console input library.
            Input.Setup(false);

            // Instantiates game borders.
            InstantiateBorders();

            // Stopwatch to measure game loop time.
            Stopwatch stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Restart();

                // Call console input library to update input-logic.
                Input.Update();

                if (Mouse.MouseDown[0])
                {
                    // Jeg har ændret på brush størrelsen og længden de er adskilt.
                    // ok - Kresten
                    // jeg sætter mit navn på det her nu :)
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x - 1, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x + 1, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y + 1));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y - 1));
                }

                Physics.Update();

                foreach (var particleGroup in Physics.ParticleTypes)
                {
                    Vector2[] dots = particleGroup.Particles.ToArray();

                    char character = '#';
                    char[] ansiCode;

                    switch (particleGroup)
                    {
                        case Sand _:
                            character = '\u2588';
                            ansiCode = new char[] { '3', '3' };
                            break;
                        case Block _:
                            character = '\u2588';
                            ansiCode = new char[] { '3', '7' };
                            break;
                        default:
                            ansiCode = null;
                            break;
                    }

                    ScreenBuffer.DrawDots(dots, dots.Length, character, ansiCode);
                }

                ScreenBuffer.ApplyBuffer();

                while (stopwatch.ElapsedMilliseconds < 1000 / 60)
                    Thread.Sleep(0);
            }
        }

        static void SetupStyle()
        {
            IntPtr hWindow = WinAPI.GetConsoleWindow(); // Get handle to console window
            long style = WinAPI.GetWindowLongA(hWindow, WinAPI.GWL_STYLE); // Retrieve style
            style ^= (long)WinAPI.WindowStyles.WS_SIZEBOX; // Zero the WS_SIZEBOX bit to prevent resizing
            style ^= (long)WinAPI.WindowStyles.WS_MAXIMIZEBOX; // Zero the WS_MAXIMIZEBOX bit to remove the maximize button
            WinAPI.SetWindowLongA(hWindow, WinAPI.GWL_STYLE, style); // Set the modified style
        }

        static void InstantiateBorders()
        {
            for (int i = 0; i < ScreenBuffer.Width; i++)
            {
                Physics.Instantiate<Block>(new Vector2(i, ScreenBuffer.Height - 1));
                Physics.Instantiate<Block>(new Vector2(i, 0));
            }
            for (int n = 0; n < ScreenBuffer.Height; n++)
            {
                Physics.Instantiate<Block>(new Vector2(0, n));
                Physics.Instantiate<Block>(new Vector2(ScreenBuffer.Width - 1, n));
            }
        }
    }
}