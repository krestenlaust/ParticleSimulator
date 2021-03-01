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

        public static void DrawDots(Vector2[] dots, int length, char character, ANSIColor color)
        {
            for (int i = 0; i < length; i++)
            {
                Vector2 dot = dots[i];
                
                int index = (int)dot.X + (int)dot.Y * Width;

                if (buffer.Length <= index || index < 0)
                {
                    continue;
                }

                buffer[index] = character;
                ansiBuffer[index * 2] = color.CharArrayRepresentation[0];
                ansiBuffer[index * 2 + 1] = color.CharArrayRepresentation[1];
            }
        }

        public static void ApplyBuffer()
        {
            Console.SetCursorPosition(0, 0);

            StringBuilder sb = new StringBuilder(buffer.Length * 6);

            for (int i = 0; i < buffer.Length; i++)
            {
                if (ansiBuffer[i * 2] != '\0')
                {
                    sb.Append("\u001b[");
                    sb.Append(ansiBuffer[i * 2]);
                    sb.Append(ansiBuffer[i * 2 + 1]);
                    sb.Append('m');
                }

                sb.Append(buffer[i] == '\0' ? ' ' : buffer[i]);
            }

            Console.Write(sb);

            buffer = new char[Width * Height];
            ansiBuffer = new char[Width * Height * 2];
        }
    }
}