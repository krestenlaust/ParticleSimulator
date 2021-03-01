using System;
using System.Numerics;

namespace ConsoleUI
{
    public static class ScreenBuffer
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        private static char[] buffer;

        public static void Setup(int width, int height)
        {
            Width = width;
            Height = height;

            buffer = new char[width * height * 6];
            
            Console.WindowWidth = Width;
            Console.WindowHeight = Height + 1;
            Console.BufferWidth = Width;
            Console.BufferHeight = Height + 1;
            Console.CursorVisible = false;
        }

        public static void DrawDots(Vector2[] dots, int length, char character, char[] ansiCode)
        {
            for (int i = 0; i < length; i++)
            {
                Vector2 dot = dots[i];
                
                // TODO: Patrick, det her kan gøres bedre 🤔... 🤢
                int index = Math.Abs((int)dot.X + (int)dot.Y * Width); //Tilføjelse

                if (buffer.Length > index + 1)
                {
                    buffer[index * 6] = character;
                    buffer[index * 6 + 1] = ansiCode[0];
                    buffer[index * 6 + 2] = ansiCode[1];
                    buffer[index * 6 + 3] = ansiCode[2];
                    buffer[index * 6 + 4] = ansiCode[3];
                    buffer[index * 6 + 5] = ansiCode[4];
                }
            }
        }

        public static void ApplyBuffer()
        {
            Console.SetCursorPosition(0, 0);

            Console.Write(buffer);

            buffer = new char[Width * Height * 6];
        }
    }
}