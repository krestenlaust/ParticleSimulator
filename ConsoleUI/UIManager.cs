using ConsoleInput;
using ConsoleUI.UI;
using System;
using System.Linq;

namespace ConsoleUI
{
    public static class UIManager
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static Scene CurrentScene { get; private set; }
        private static WinAPI.CharInfo[] buffer;

        public static void Setup(int width, int height)
        {
            Width = Math.Min(width, Console.LargestWindowWidth - 1);
            Height = Math.Min(height, Console.LargestWindowHeight - 1);

            buffer = new WinAPI.CharInfo[Width * Height];

            Console.WindowWidth = Width;
            Console.WindowHeight = Height + 1;
            Console.BufferWidth = Width;
            Console.BufferHeight = Height + 1;
            Console.CursorVisible = false;

            // Call Windows API to enable our specific console needs.
            WinAPI.EnableANSIProcessing();
            WinAPI.SetFontSize(15, 15);
            WinAPI.SetConsoleOutputCP(437);

            // Set up the window style
            WinAPI.SetupStyle();
        }

        public static void Draw()
        {
            if (CurrentScene is null)
            {
                return;
            }

            // Sorterer UI controls så de ligger i rækkefølge af deres z-position.
            // Laveste først (stigende), så de bliver skrevet over af dem der har en højere z-værdi.
            var orderToRender = from c in CurrentScene.Controls
                                orderby c.Zindex ascending
                                select c;

            foreach (var control in orderToRender)
            {
                HandleEvents(control);

                // Render
                RenderControl(control);
            }
        }

        /// <summary>
        /// Skriver den interne skærmbuffer oven på konsollens skærmbuffer, hvilket ændrer det der er på skærmen.
        /// </summary>
        public static void RenderBuffer()
        {
            WinAPI.WriteColorFast(buffer);

            buffer = new WinAPI.CharInfo[Width * Height];
        }

        public static void ChangeScene(Scene scene)
        {
            CurrentScene = scene;
        }

        /// <summary>
        /// Håndtér klik og hover events for et givent ui-element, bl.a. ved at ændre.
        /// </summary>
        /// <param name="control"></param>
        private static void HandleEvents(Control control)
        {
            bool cursorInside = control.IsPointInside(Mouse.x, Mouse.y);
            
            // Sætter newState til ingen-værdi.
            Control.HoverState? newState = null;

            switch (control.InternalHoverState)
            {
                case Control.HoverState.Enter:
                    newState = cursorInside ? Control.HoverState.Stay : Control.HoverState.Exit;
                    break;
                case Control.HoverState.Stay:
                    newState = cursorInside ? Control.HoverState.Stay : Control.HoverState.Exit;
                    break;
                case Control.HoverState.Exit when !cursorInside:
                    newState = Control.HoverState.None;
                    break;
                case Control.HoverState.None when cursorInside:
                    newState = Control.HoverState.Enter;
                    break;
            }

            // Notificerer kontrollen om musens tilstand, hvis newState er blevet tildelt en værdi.
            if (newState.HasValue)
            {
                control.UpdateHoverState(newState.Value);
            }

            // Hvis musen ikke er indenfor kontrollens rammer, er det ikke relevant at tjekke museknapperne.
            if (!cursorInside)
                return;

            control.UpdateButtonState();
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
    }
}