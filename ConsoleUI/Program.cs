using System;
using System.Numerics;
using System.Diagnostics;
using System.Threading;
using ConsoleInput;
using ParticleEngine;
using ParticleEngine.Particles;
using System.Text;

namespace ConsoleUI
{
    class Program
    {
        static int width;
        static int height;
        static char[] screenBuffer;
        static char[] ansiBuffer;

        static void Main(string[] args)
        {
            // Define console screen sizes to allow easy line wrapping.
            width = 100;
            height = 45;

            screenBuffer = new char[width * height];
            ansiBuffer = new char[width * height * 5];

            Console.WindowWidth = width;
            Console.WindowHeight = height + 1;
            Console.BufferWidth = width;
            Console.BufferHeight = height + 1;
            Console.CursorVisible = false;

            // Call Windows API to enable our specific console needs.
            WinAPI.EnableANSIProcessing();
            //WinAPI.SetFontSize(10, 10);

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
                            ansiCode = "\u001b[33m".ToCharArray();
                            break;
                        case Block _:
                            character = '\u2588';
                            ansiCode = "\u001b[37m".ToCharArray();
                            break;
                        default:
                            ansiCode = null;
                            break;
                    }

                    DrawDots(dots, dots.Length, character, ansiCode);
                }

                ApplyBuffer();

                while (stopwatch.ElapsedMilliseconds < 1000 / 60)
                    Thread.Sleep(0);
            }
        }

        static void DrawDots(Vector2[] dots, int length, char character, char[] ansiCode)
        {
            for (int i = 0; i < length; i++)
            {
                Vector2 dot = dots[i];

                // TODO: Patrick, det her kan gøres bedre 🤔...
                int index = Math.Abs((int)dot.X + (int)dot.Y * width); //Tilføjelse

                if (screenBuffer.Length > index + 1)
                {
                    screenBuffer[index] = character;
                    ansiBuffer[index * 5] = ansiCode[0];
                    ansiBuffer[index * 5 + 1] = ansiCode[1];
                    ansiBuffer[index * 5 + 2] = ansiCode[2];
                    ansiBuffer[index * 5 + 3] = ansiCode[3];
                    ansiBuffer[index * 5 + 4] = ansiCode[4];
                }
            }
        }

        public static void ApplyBuffer()
        {
            Console.SetCursorPosition(0, 0);

            StringBuilder sb = new StringBuilder(screenBuffer.Length + ansiBuffer.Length * 5);

            for (int i = 0; i < screenBuffer.Length; i++)
            {
                if (ansiBuffer[i * 5] != '\0')
                {
                    sb.Append(ansiBuffer[i * 5]);
                    sb.Append(ansiBuffer[i * 5 + 1]);
                    sb.Append(ansiBuffer[i * 5 + 2]);
                    sb.Append(ansiBuffer[i * 5 + 3]);
                    sb.Append(ansiBuffer[i * 5 + 4]);
                }

                sb.Append(screenBuffer[i]);
            }

            Console.Write(sb);

            screenBuffer = new char[width * height];
            ansiBuffer = new char[width * height * 5];
        }

        static void InstantiateBorders()
        {
            for (int i = 0; i < width; i++)
            {
                Physics.Instantiate<Block>(new Vector2(i, height - 1));
                Physics.Instantiate<Block>(new Vector2(i, 0));
            }
            for (int n = 0; n < height; n++)
            {
                Physics.Instantiate<Block>(new Vector2(0, n));
                Physics.Instantiate<Block>(new Vector2(width - 1, n));
            }
        }
    }
}