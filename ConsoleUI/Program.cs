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

            while (true)
            {
                stopwatch.Restart();

                Input.Update();
                Physics.Update();

                if (Mouse.MouseDown[0])
                {
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x - 2, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x + 2, Mouse.y));
                }

                while (stopwatch.ElapsedMilliseconds <= 1000 / 60)
                    Thread.Sleep(0);
            }
        }

        static void Gravity(Vector2[] dots, int length)
        {
            HashSet<Vector2> movedTo = new HashSet<Vector2>();

            for (int i = 0; i < length; i++)
            {
                movedTo.Add(dots[i]);
            }

        }

        static void DrawDots(Vector2[] dots, int length, char character)
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

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 0);
            Console.Write(newDots);
        }
    }
}
