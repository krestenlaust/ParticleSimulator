using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ConsoleUI.UI;

namespace ConsoleUI
{
    public static class ScreenBuffer
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public readonly static List<Control> Controls = new List<Control>();
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

        public static void Render()
        {
            // Sorterer UI controls så de ligger i rækkefølge af deres z-position.
            // Laveste først (stigende), så de bliver skrevet over af dem der har en højere z-værdi.
            var orderToRender = from c in Controls
                                orderby c.Zindex ascending
                                select c;

            foreach (var item in orderToRender)
            {
                RenderControl(item);
            }
        }

        /// <summary>
        /// Default color: foreground white.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static WinAPI.CharInfo ConvertPixel(char character) => ConvertPixel(character, 15);
        public static WinAPI.CharInfo ConvertPixel(char character, PixelColor color) => ConvertPixel(character, color.AttributeValue);
        private static WinAPI.CharInfo ConvertPixel(char character, short colorAttribute) => new WinAPI.CharInfo { Char = new WinAPI.CharUnion { UnicodeChar = character }, Attributes = colorAttribute };

        private static void RenderControl(Control control)
        {
            // Partionere en del af skærmen som vil blive "udlejet" til en control så den kan tegne sig selv.
            TableSegment<WinAPI.CharInfo> screenSegment = new TableSegment<WinAPI.CharInfo>(buffer, Width, control.Width, control.Height, control.X, control.Y);
            control.Draw(screenSegment);
        }
         
        /*
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
        }*/

        public static void ApplyBuffer()
        {
            WinAPI.WriteColorFast(buffer);

            buffer = new WinAPI.CharInfo[Width * Height];
        }
    }
}