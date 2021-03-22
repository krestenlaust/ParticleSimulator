using System.Numerics;
using System.Diagnostics;
using System.Threading;
using ConsoleInput;
using ParticleEngine;
using ParticleEngine.Particles;
using System;
using System.Threading.Tasks;

namespace ConsoleUI
{
    class Program
    {
        const int FramesPerSecondCap = 30;

        static void Main(string[] args)
        {
            // Define console screen sizes to allow easy line wrapping.
            ScreenBuffer.Setup(90, 40);

            // Call Windows API to enable our specific console needs.
            WinAPI.EnableANSIProcessing();
            //WinAPI.SetFontSize(16, 16);

            // Set up the window style
            WinAPI.SetupStyle();

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
                    // ikke i orden - Kresten
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x - 1, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x + 1, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y + 1));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y - 1));
                }
                else if (Mouse.MouseDown[1])
                {
                    Physics.Instantiate<Block>(new Vector2(Mouse.x, Mouse.y));
                }
                else if (Mouse.MouseDown[2])
                {
                    Physics.Instantiate<Acid>(new Vector2(Mouse.x, Mouse.y));
                }
                else if (Console.CapsLock)
                {
                    Physics.Instantiate<Gas>(new Vector2(Mouse.x, Mouse.y));
                }

                Physics.Update();

                Parallel.ForEach(Physics.ParticleGroups, particleGroup =>
                {
                    Vector2[] dots = particleGroup.Particles.ToArray();

                    char character = '\u2588';
                    ANSIColor color;

                    switch (particleGroup)
                    {
                        case Sand _:
                            color = new ANSIColor(ANSIColor.Color.Yellow);
                            //character = '*';
                            break;
                        case Block _:
                            color = new ANSIColor(ANSIColor.Color.Blue);
                            break;
                        case Acid _:
                            color = new ANSIColor(ANSIColor.Color.Green);
                            //character = '_';
                            break;
                        case Gas _:
                            color = new ANSIColor(ANSIColor.Color.White);
                            //character = '(';
                            break;
                        case Virus _:
                            //character = '.';
                            color = new ANSIColor(ANSIColor.Color.Magenta);
                            break;
                        default:
                            color = new ANSIColor();
                            break;
                    }

                    ScreenBuffer.DrawDots(dots, dots.Length, character, color);
                });

                ScreenBuffer.ApplyBuffer();

                //Console.Title = $"Performance: {Math.Floor(1000f / FramesPerSecondCap / stopwatch.ElapsedMilliseconds * 100)}%";
                Console.Title = $"FPS: {Math.Floor(1 / stopwatch.Elapsed.TotalSeconds)}";

                while (stopwatch.ElapsedMilliseconds < 1000 / FramesPerSecondCap)
                    Thread.Sleep(0);
            }
        }

        static void InstantiateBorders()
        {
            for (int n = 0; n < ScreenBuffer.Width; n++)
            {
                Physics.Instantiate<Block>(new Vector2(n, ScreenBuffer.Height - 1));
                Physics.Instantiate<Block>(new Vector2(n, 0));
            }

            for (int n = 0; n < ScreenBuffer.Height; n++)
            {
                Physics.Instantiate<Block>(new Vector2(ScreenBuffer.Width - 1, n));
                Physics.Instantiate<Block>(new Vector2(0, n));
            }
        }
    }
}