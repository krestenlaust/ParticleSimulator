using System;
using System.Numerics;
using System.Text;

namespace ConsoleUI
{
    public static class ScreenBuffer
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        private static char[] buffer;
        private static char[] ansiBuffer;

        public static void Setup(int width, int height)
        {
            Width = width;
            Height = height;

            buffer = new char[width * height];
            ansiBuffer = new char[width * height * 2];
            
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

                // TODO: Patrick, det her kan gøres bedre 🤔...
                int index = Math.Abs((int)dot.X + (int)dot.Y * Width); //Tilføjelse

                if (buffer.Length > index + 1)
                {
                    buffer[index] = character;
                    ansiBuffer[index] = ansiCode[0];
                    ansiBuffer[index + 1] = ansiCode[1];
                }
            }
        }

        public static void ApplyBuffer()
        {
            Console.SetCursorPosition(0, 0);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < buffer.Length; i++)
            {
                if (ansiBuffer[i * 2] != '\0')
                {
                    sb.Append("\u001b[");
                    sb.Append(ansiBuffer[i * 2]);
                    sb.Append(ansiBuffer[i * 2 + 1]);
                    sb.Append('m');
                }

                sb.Append(buffer[i]);
            }

            Console.Write(sb);

            buffer = new char[Width * Height];
            ansiBuffer = new char[Width * Height * 2];
        }
    }
}
