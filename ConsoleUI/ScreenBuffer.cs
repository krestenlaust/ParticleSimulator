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
        private static ANSIColor[] ansiBuffer;
        private static readonly char[] ANSICodePrefix = new char[] { '\u001b', '[' };

        public static void Setup(int width, int height)
        {
            Width = width;
            Height = height;

            buffer = new char[width * height];
            ansiBuffer = new ANSIColor[width * height];
            
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
                ansiBuffer[index] = color;
            }
        }

        public static void ApplyBuffer()
        {
            Console.SetCursorPosition(0, 0);

            // Allocate half of max, 1 buffer length for every char, and 5 chars for every color.
            StringBuilder sb = new StringBuilder(buffer.Length * 6);

            for (int i = 0; i < buffer.Length; i++)
            {
                /*if (ansiBuffer[i].Value != 0)
                {
                    sb.Append(ANSICodePrefix);
                    sb.Append(ansiBuffer[i].ToString());
                    sb.Append('m');
                }*/

                sb.Append(buffer[i] == '\0' ? ' ' : buffer[i]);
            }

            //Console.Write(sb.ToString());
            //WinAPI.WriteConsoleNative(sb.ToString());

            WinAPI.CharInfo[] chars = new WinAPI.CharInfo[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                var Char = new WinAPI.CharUnion { AsciiChar = (byte)buffer[i] };
                var Attributes = (short)((short)((short)ANSIColor.Color.White + (short)ANSIColor.Color.Black + 42)); // fiks farve pls :)
                chars[i].Char = Char;
                chars[i].Attributes = Attributes;
            }
            WinAPI.WriteColorFast(chars);

            buffer = new char[Width * Height];
            ansiBuffer = new ANSIColor[Width * Height];
        }
    }
}