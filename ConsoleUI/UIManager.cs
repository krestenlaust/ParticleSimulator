using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleInput;
using ConsoleUI.UI;

namespace ConsoleUI
{
    public static class UIManager
    {
        public readonly static List<Control> Controls = new List<Control>();
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

        public static void LogicAndRender()
        {
            // Sorterer UI controls så de ligger i rækkefølge af deres z-position.
            // Laveste først (stigende), så de bliver skrevet over af dem der har en højere z-værdi.
            var orderToRender = from c in Controls
                                orderby c.Zindex ascending
                                select c;

            foreach (var control in orderToRender)
            {
                HandleEvents(control);

                // Render
                RenderControl(control);
            }
        }

        private static void HandleEvents(Control control)
        {
            bool cursorInside = control.IsPointInside(Mouse.x, Mouse.y);

            switch (control.InternalHoverState)
            {
                case Control.HoverState.Enter:
                    if (cursorInside)
                    {
                        control.UpdateHoverState(Control.HoverState.Stay);
                    }
                    else
                    {
                        control.UpdateHoverState(Control.HoverState.Exit);
                    }
                    break;
                case Control.HoverState.Stay:
                    if (cursorInside)
                    {
                        control.UpdateHoverState(Control.HoverState.Stay);
                    }
                    else
                    {
                        control.UpdateHoverState(Control.HoverState.Exit);
                    }
                    break;
                case Control.HoverState.Exit:
                    if (!cursorInside)
                    {
                        control.UpdateHoverState(Control.HoverState.None);
                    }
                    break;
                default:
                    if (cursorInside)
                    {
                        control.UpdateHoverState(Control.HoverState.Enter);
                    }
                    break;
            }
            
            switch (control.InternalMouseButtonState)
            {
                case Control.MouseButtonState.Down:
                    if (Mouse.MouseDown[0])
                    {
                        if (cursorInside)
                        {
                            control.UpdateButtonState(Control.MouseButtonState.Hold);
                        }
                    }
                    else
                    {
                        control.UpdateButtonState(Control.MouseButtonState.Release);
                    }
                    break;
                case Control.MouseButtonState.Hold:
                    if (!Mouse.MouseDown[0])
                    {
                        control.UpdateButtonState(Control.MouseButtonState.Release);
                    }
                    break;
                case Control.MouseButtonState.Release:
                    if (!Mouse.MouseDown[0])
                    {
                        control.UpdateButtonState(Control.MouseButtonState.None);
                    }
                    break;
                case Control.MouseButtonState.None:
                    if (Mouse.MouseDown[0] && cursorInside)
                    {
                        control.UpdateButtonState(Control.MouseButtonState.Down);
                    }
                    break;
                default:
                    break;
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
            ScreenSegment screenSegment = new ScreenSegment(buffer, Width, control.Width, control.Height, control.X, control.Y);
            control.Draw(screenSegment);
        }

        public static void ApplyBuffer()
        {
            WinAPI.WriteColorFast(buffer);

            buffer = new WinAPI.CharInfo[Width * Height];
        }
    }
}