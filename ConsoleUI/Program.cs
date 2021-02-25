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
        static Vector2[] dots = new Vector2[10000];
        static int dotsIndex = 0;
        static int gravity = 1;

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

                if (Mouse.MouseDown[0])
                {
                    CreateDot(Mouse.x, Mouse.y);
                    CreateDot(Mouse.x - 1, Mouse.y);
                    CreateDot(Mouse.x + 1, Mouse.y);
                }

                if (Mouse.MousePress[1])
                {
                    gravity *= -1;
                }

                Gravity(dots, dotsIndex);

                DrawDots(dots, dotsIndex, '\x2588');

                while (stopwatch.ElapsedMilliseconds <= 1000 / 60)
                    Thread.Sleep(0);
            }
        }

        static void CreateDot(int x, int y)
        {
            Console.Title = (dots.Length - dotsIndex).ToString();
            dots[dotsIndex++] = new Vector2(x, y);
        }

        static void Gravity(Vector2[] dots, int length)
        {
            HashSet<Vector2> movedTo = new HashSet<Vector2>();

            for (int i = 0; i < length; i++)
            {
                movedTo.Add(dots[i]);
            }

            for (int i = 0; i < length; i++)
            {
                if (dots[i].Y != height - 1 && gravity == 1 || dots[i].Y != 1 && gravity == -1)
                {
                    dots[i].Y += 1 * gravity;

                    // below is occupied, check sides
                    if (movedTo.Contains(dots[i]))
                    {
                        dots[i].X += 1 * gravity;

                        // check if left side is occupied
                        if (movedTo.Contains(dots[i]))
                        {
                            dots[i].X -= 2 * gravity;

                            // cant move, go to original position
                            if (movedTo.Contains(dots[i]))
                            {
                                dots[i].X += 1 * gravity;
                                dots[i].Y -= 1 * gravity;
                            }
                        }
                    }
                }
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
