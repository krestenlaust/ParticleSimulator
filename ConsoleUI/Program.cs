using System;
using System.Numerics;
using System.Diagnostics;
using System.Threading;
using ConsoleInput;
using System.Collections.Generic;
using ParticleEngine;

namespace ConsoleUI
{
    class Program
    {
        static int width;
        static int height;

        static void Main(string[] args)
        {
            width = 100;
            height = 45;

            Console.WindowWidth = width;
            Console.WindowHeight = height + 1;
            Console.BufferWidth = width;
            Console.BufferHeight = height + 1;
            Console.CursorVisible = false;
            
            Input.Setup(false);

            Stopwatch stopwatch = new Stopwatch();
            Stopwatch physicsStopwatch = new Stopwatch();
            physicsStopwatch.Start();

            while (true)
            {
                stopwatch.Restart();

                Input.Update();

                if (Mouse.MouseDown[0])
                {
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x - 2, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x + 2, Mouse.y));
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
                            character = '*';
                            color = ConsoleColor.Yellow;
                            break;
                    }

                    DrawDots(dots, dots.Length, character, color);
                }

                while (stopwatch.ElapsedMilliseconds < 1000 / 60)
                    Thread.Sleep(0);
            }
        }

        static void DrawDots(Vector2[] dots, int length, char character, ConsoleColor color)
        {
            char[] newDots = new char[width * height];

            for (int i = 0; i < length; i++)
            {
                Vector2 dot = dots[i];

                try
                {
                    newDots[(int)dot.X + (int)dot.Y * width] = character;
                }
                catch (Exception)
                {

                }
            }

            Console.ForegroundColor = color;
            Console.SetCursorPosition(0, 0);
            Console.Write(newDots);
        }
    }
}
