using ConsoleInput;
using ConsoleInteraction;
using ConsoleInteraction.UI;
using ParticleEngine;
using ParticleEngine.Particles;
using System;
using System.Linq;
using System.Numerics;

namespace GameImplementation
{
    public class GameControl : Control
    {

        // U+2588 - Full block
        // For some reason the hexadecimal value 153 must be added to the unicode hex value to get the correct character
        // This probably has something to do with the codepage being weird ¯\_(ツ)_/¯
        public const char DOT_REPRESENTATION = '\u26db'; // Full block + hex 153
        private ParticleGroup selectedParticleType;

        public GameControl(bool borders, int width, int height) : base(width, height)
        {
            if (borders)
            {
                InstantiateBorders();
            }

            // Select sand as default.
            SelectParticleType<Sand>();
        }

        public void SelectParticleType<T>() where T : ParticleGroup, new()
        {
            selectedParticleType = Physics.GetParticleGroup<T>();
        }

        public void TryPlaceParticle<T>(Vector2 position) where T : ParticleGroup, new()
        {
            if (Physics.IsOccupied(position))
            {
                return;
            }

            Physics.Instantiate<T>(position);
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
                        color = new PixelColor(ConsoleColor.DarkCyan);
                        break;
                    case Acid _:
                        color = new PixelColor(ConsoleColor.Green);
                        break;
                    case Virus _:
                        color = new PixelColor(ConsoleColor.Magenta);
                        break;
                    case Water _:
                        color = new PixelColor(ConsoleColor.Blue);
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

        protected override void UpdateButtonState()
        {
            // SelectedParticleType burde aldrig være null, men hvis den er, så return.
            if (selectedParticleType is null)
            {
                base.UpdateButtonState();
                return;
            }

            // Place 1 particle.
            if (Mouse.MouseDown[0])
            {
                Physics.Instantiate(new Vector2(Mouse.x, Mouse.y), selectedParticleType);
            }
            else if (Mouse.MouseDown[1]) // Place multiple particles.
            {
                Physics.Instantiate(new Vector2(Mouse.x, Mouse.y), selectedParticleType);
                Physics.Instantiate(new Vector2(Mouse.x - 1, Mouse.y), selectedParticleType);
                Physics.Instantiate(new Vector2(Mouse.x + 1, Mouse.y), selectedParticleType);
                Physics.Instantiate(new Vector2(Mouse.x, Mouse.y + 1), selectedParticleType);
                Physics.Instantiate(new Vector2(Mouse.x, Mouse.y - 1), selectedParticleType);
                // Lavet af Patrick
            }

            base.UpdateButtonState();
        }

        public void InstantiateBorders()
        {
            for (int n = 0; n < Width; n++)
            {
                Physics.Instantiate<Block>(new Vector2(n, Height - 1));
                Physics.Instantiate<Block>(new Vector2(n, 0));
            }

            for (int n = 0; n < Height; n++)
            {
                Physics.Instantiate<Block>(new Vector2(Width - 1, n));
                Physics.Instantiate<Block>(new Vector2(0, n));
            }
        }
    }
}
