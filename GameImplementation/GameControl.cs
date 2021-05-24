using ConsoleInput;
using ConsoleUI;
using ConsoleUI.UI;
using ParticleEngine;
using ParticleEngine.Particles;
using System;
using System.Numerics;

namespace GameImplementation
{
    public class GameControl : Control
    {
        public const char DOT_REPRESENTATION = '\u26db';

        public GameControl(bool borders)
        {
            if (borders)
            {
                InstantiateBorders();
            }
        }

        public override void Draw(ScreenSegment segment)
        {
            foreach (var particleGroup in Physics.ParticleGroups)
            {
                Vector2[] dots = particleGroup.Particles.ToArray();

                PixelColor color;

                switch (particleGroup)
                {
                    case Sand _:
                        color = new PixelColor(ConsoleColor.Yellow);
                        break;
                    case Block _:
                        color = new PixelColor(ConsoleColor.Blue);
                        break;
                    case Acid _:
                        color = new PixelColor(ConsoleColor.Green);
                        break;
                    case Virus _:
                        color = new PixelColor(ConsoleColor.Magenta);
                        break;
                    case Water _:
                        color = new PixelColor(ConsoleColor.DarkBlue);
                        break;
                    default:
                        color = new PixelColor();
                        break;
                }

                DrawDots(segment, dots, dots.Length, DOT_REPRESENTATION, color);
            }
        }

        public void DrawDots(ScreenSegment screenSegment, Vector2[] dots, int length, char character, PixelColor color)
        {
            for (int i = 0; i < length; i++)
            {
                Vector2 dot = dots[i];

                if (screenSegment.Width <= (int)dot.X || (int)dot.X < 0 || screenSegment.Height <= dot.Y || dot.Y < 0)
                {
                    continue;
                }

                var charUnion = new WinAPI.CharUnion { AsciiChar = (byte)character };

                screenSegment[(int)dot.X, (int)dot.Y] = new WinAPI.CharInfo
                {
                    Char = charUnion,
                    Attributes = color.AttributeValue
                };
            }
        }

        protected override void UpdateButtonState(MouseButtonState newState)
        {
            if (Mouse.MouseDown[0])
            {
                // Jeg har ændret på brush størrelsen og længden de er adskilt.
                // ok - Kresten
                // jeg sætter mit navn på det her nu :)
                // ikke i orden - Kresten
                Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y));
                Physics.Instantiate<Sand>(new Vector2(Mouse.x - 1, Mouse.y));
                Physics.Instantiate<Sand>(new Vector2(Mouse.x + 1, Mouse.y));
                Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y + 1));
                Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y - 1));
            }
            else if (Mouse.MouseDown[1])
            {
                Physics.Instantiate<Water>(new Vector2(Mouse.x, Mouse.y));
            }
            else if (Mouse.MouseDown[2])
            {
                Physics.Instantiate<Block>(new Vector2(Mouse.x, Mouse.y));
            }

            base.UpdateButtonState(newState);
        }

        private static void InstantiateBorders()
        {
            for (int n = 0; n < UIManager.Width; n++)
            {
                Physics.Instantiate<Block>(new Vector2(n, UIManager.Height - 1));
                Physics.Instantiate<Block>(new Vector2(n, 0));
            }

            for (int n = 0; n < UIManager.Height; n++)
            {
                Physics.Instantiate<Block>(new Vector2(UIManager.Width - 1, n));
                Physics.Instantiate<Block>(new Vector2(0, n));
            }
        }
    }
}
