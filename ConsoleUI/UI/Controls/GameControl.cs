using ParticleEngine;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ConsoleUI.UI.Controls
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

        public override void Draw(TableSegment<WinAPI.CharInfo> segment)
        {
            foreach (var particleGroup in Physics.ParticleGroups)
            {
                Vector2[] dots = particleGroup.Particles.ToArray();

                PixelColor color;

                switch (particleGroup)
                {
                    case ParticleEngine.Particles.Sand _:
                        color = new PixelColor(ConsoleColor.Yellow);
                        break;
                    case ParticleEngine.Particles.Block _:
                        color = new PixelColor(ConsoleColor.Blue);
                        break;
                    case ParticleEngine.Particles.Acid _:
                        color = new PixelColor(ConsoleColor.Green);
                        break;
                    case ParticleEngine.Particles.Virus _:
                        color = new PixelColor(ConsoleColor.Magenta);
                        break;
                    case ParticleEngine.Particles.Water _:
                        color = new PixelColor(ConsoleColor.DarkBlue);
                        break;
                    default:
                        color = new PixelColor();
                        break;
                }

                DrawDots(segment, dots, dots.Length, DOT_REPRESENTATION, color);
            }
        }

        public void DrawDots(TableSegment<WinAPI.CharInfo> screenSegment, Vector2[] dots, int length, char character, PixelColor color)
        {
            for (int i = 0; i < length; i++)
            {
                Vector2 dot = dots[i];

                if (screenSegment.Width < (int)dot.X || (int)dot.X < 0 || screenSegment.Height < dot.Y || dot.Y < 0)
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

        private static void InstantiateBorders()
        {
            for (int n = 0; n < UIManager.Width; n++)
            {
                Physics.Instantiate<ParticleEngine.Particles.Block>(new Vector2(n, UIManager.Height - 1));
                Physics.Instantiate<ParticleEngine.Particles.Block>(new Vector2(n, 0));
            }

            for (int n = 0; n < UIManager.Height; n++)
            {
                Physics.Instantiate<ParticleEngine.Particles.Block>(new Vector2(UIManager.Width - 1, n));
                Physics.Instantiate<ParticleEngine.Particles.Block>(new Vector2(0, n));
            }
        }
    }
}
