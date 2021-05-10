using System;
using System.Numerics;

namespace ConsoleUI
{
    public static class ScreenBuffer
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        private static WinAPI.CharInfo[] buffer;

        public static void Setup(int width, int height)
        {
            Width = width;
            Height = height;

            buffer = new WinAPI.CharInfo[Width * Height];
            
            Console.WindowWidth = Width;
            Console.WindowHeight = Height + 1;
            Console.BufferWidth = Width;
            Console.BufferHeight = Height + 1;
            Console.CursorVisible = false;
        }

        public static void DrawDots(Vector2[] dots, int length, char character, PixelColor color)
        {
            for (int i = 0; i < length; i++)
            {
                Vector2 dot = dots[i];
                
                int index = (int)dot.X + (int)dot.Y * Width;

                if (buffer.Length <= index || index < 0)
                {
                    continue;
                }

                var charUnion = new WinAPI.CharUnion { AsciiChar = (byte)character };

                buffer[index] = new WinAPI.CharInfo { 
                    Char = charUnion, 
                    Attributes = color.AttributeValue 
                };              
            }
            WinAPI.WriteColorFast(buffer, character);
        }

        public static void ApplyBuffer()
        {
            WinAPI.WriteColorFast(buffer, '1');

            buffer = new WinAPI.CharInfo[Width * Height];
        }
    }
}