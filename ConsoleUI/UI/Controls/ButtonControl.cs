﻿using ConsoleInput;
using System;

namespace ConsoleInteraction.UI.Controls
{
    public class ButtonControl : Control
    {
        /// <summary>
        /// En simpel delegate til at lytte efter kliks. Man kan vælge bruge det event der er på forældre <c>Control</c>  i stedet.
        /// </summary>
        public Action OnClick;
        private readonly char borderChar;
        private readonly PixelColor borderHoverColor;
        private readonly PixelColor borderDefaultColor;
        private readonly PixelColor borderPressedColor;
        private readonly LabelControl text;
        private PixelColor borderColor;
        private bool pressed;

        /// <summary>
        /// Ændrer teksten inde i tekst elementet.
        /// </summary>
        public string Text
        {
            get { return text.Text; }
            set { text.Text = value; }
        }

        /// <summary>
        /// Instantiates a <i>blue</i> button control with a default size of (10, 5) and </i>no text</i>.
        /// </summary>
        public ButtonControl() : base(10, 5)
        {
            text = new LabelControl("");
            borderChar = '\u26db';
            borderHoverColor = new PixelColor(ConsoleColor.DarkBlue);
            borderDefaultColor = new PixelColor(ConsoleColor.Blue);
            borderPressedColor = new PixelColor(ConsoleColor.DarkCyan);
            borderColor = borderDefaultColor;
        }

        /// <summary>
        /// Instantiates a button control
        /// </summary>
        /// <param name="borderDefaultColor"></param>
        /// <param name="borderHoverColor"></param>
        /// <param name="borderPressedColor"></param>
        /// <param name="borderChar"></param>
        public ButtonControl(PixelColor borderDefaultColor, PixelColor borderHoverColor, PixelColor borderPressedColor, char borderChar = '\u26db') : base(0, 5)
        {
            text = new LabelControl("");
            this.borderChar = borderChar;
            this.borderHoverColor = borderHoverColor;
            this.borderDefaultColor = borderDefaultColor;
            this.borderPressedColor = borderPressedColor;
            borderColor = this.borderDefaultColor;
        }

        protected internal override void UpdateHoverState(HoverState newState)
        {
            switch (newState)
            {
                case HoverState.Enter:
                    borderColor = borderHoverColor;
                    break;
                case HoverState.Stay when !pressed:
                    borderColor = borderHoverColor;
                    break;
                case HoverState.Exit:
                    borderColor = borderDefaultColor;
                    break;
                default:
                    break;
            }

            base.UpdateHoverState(newState);
        }

        protected internal override void UpdateButtonState()
        {
            if ((Mouse.MouseDown[0] || Mouse.MousePress[0]) && !pressed)
            {
                pressed = true;

                borderColor = borderPressedColor;
            }
            else if (Mouse.MouseUp[0])
            {
                pressed = false;

                OnClick?.Invoke();
            }

            base.UpdateButtonState();
        }

        public override void Draw(ScreenSegment segment)
        {
            if (Height < 3 || Width < 3)
            {
                return;
            }

            for (int y = 0; y < Height; y++)
            {
                segment[0, y] = UIManager.ConvertPixel(borderChar, borderColor);
                segment[segment.Width - 1, y] = UIManager.ConvertPixel(borderChar, borderColor);

                // center
                if (Math.Floor(Height / 2f) == y)
                {
                    text.Width = Math.Min(text.Text.Length, Width - 2);
                    ScreenSegment subSegment = new ScreenSegment(segment, text.Width, 1, 2, y);
                    text.Draw(subSegment);
                }
                else if (y % (Height - 1) == 0)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        segment[x, y] = UIManager.ConvertPixel(borderChar, borderColor);
                    }
                }
                else
                {
                    for (int x = 1; x < Width - 1; x++)
                    {
                        segment[x, y] = UIManager.ConvertPixel(' ');
                    }
                }
            }
        }
    }
}
