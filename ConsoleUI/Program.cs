using System;
using System.Numerics;
using System.Diagnostics;
using System.Threading;
using ConsoleInput;
using System.Collections.Generic;
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
        static ConsoleColor[] colorBuffer;

        static void Main(string[] args)
        {
            width = 100;
            height = 45;

            screenBuffer = new char[width * height];
            colorBuffer = new ConsoleColor[width * height];

            Console.WindowWidth = width;
            Console.WindowHeight = height + 1;
            Console.BufferWidth = width;
            Console.BufferHeight = height + 1;
            Console.CursorVisible = false;
            
            Input.Setup(false);

            //Instantiates walls
            InstantiateBorders();

            Stopwatch stopwatch = new Stopwatch();
            Stopwatch physicsStopwatch = new Stopwatch();
            physicsStopwatch.Start();

            while (true)
            {
                stopwatch.Restart();

                Input.Update();

                if (Mouse.MouseDown[0])
                {
                    //Jeg har ændret på brush størrelsen og længden de er adskilt
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x - 1, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x + 1, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y + 1));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y - 1));
                }

                if (physicsStopwatch.ElapsedMilliseconds > 1000 / 30)
                {
                    physicsStopwatch.Restart();
                    Physics.Update();
                }

                foreach (var particleGroup in Physics.ParticleTypes)
                {
                    Vector2[] dots = particleGroup.Particles.ToArray();

                    char character = '#';
                    ConsoleColor color = ConsoleColor.White;

                    switch (particleGroup)
                    {
                        case Sand _:
                            character = '#';
                            color = ConsoleColor.Yellow;
                            break;
                        case Block _:
                            character = '\u2588';
                            break;
                    }

                    DrawDots(dots, dots.Length, character, color);
                }

                ApplyBuffer();

                while (stopwatch.ElapsedMilliseconds < 1000 / 60)
                    Thread.Sleep(0);
            }
        }

        static void DrawDots(Vector2[] dots, int length, char character, ConsoleColor color)
        {
            for (int i = 0; i < length; i++)
            {
                Vector2 dot = dots[i];

                int index = Math.Abs((int)dot.X + (int)dot.Y * width); //Tilføjelse

                if (screenBuffer.Length > index + 1)
                {
                    screenBuffer[index] = character;
                    colorBuffer[index] = color;
                }
            }
        }

        public static void ApplyBuffer()
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < screenBuffer.Length; i++)
            {
                Console.ForegroundColor = colorBuffer[i];
                Console.Write(screenBuffer[i]);
            }

            screenBuffer = new char[width * height];
            colorBuffer = new ConsoleColor[width * height];
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