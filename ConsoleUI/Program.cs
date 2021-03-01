using System.Numerics;
using System.Diagnostics;
using System.Threading;
using ConsoleInput;
using ParticleEngine;
using ParticleEngine.Particles;

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
            WinAPI.SetFontSize(14, 14);

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
                    Physics.Instantiate<AcidPowder>(new Vector2(Mouse.x, Mouse.y));
                    Physics.Instantiate<AcidPowder>(new Vector2(Mouse.x - 1, Mouse.y));
                    Physics.Instantiate<AcidPowder>(new Vector2(Mouse.x + 1, Mouse.y));
                }

                Physics.Update();

                foreach (var particleGroup in Physics.ParticleTypes)
                {
                    Vector2[] dots = particleGroup.Particles.ToArray();

                    char character = '\u2588';
                    ANSIColor color;

                    switch (particleGroup)
                    {
                        case Sand _:
                            color = new ANSIColor(ANSIColor.Color.Yellow, ANSIColor.Ground.Fore, false);
                            break;
                        case Block _:
                            color = new ANSIColor(ANSIColor.Color.White, ANSIColor.Ground.Fore, false);
                            break;
                        case AcidPowder _:
                            color = new ANSIColor(ANSIColor.Color.Green, ANSIColor.Ground.Fore, true);
                            break;
                        default:
                            color = new ANSIColor();
                            break;
                    }

                    ScreenBuffer.DrawDots(dots, dots.Length, character, color);
                }

                ScreenBuffer.ApplyBuffer();

                while (stopwatch.ElapsedMilliseconds < 1000 / 60)
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