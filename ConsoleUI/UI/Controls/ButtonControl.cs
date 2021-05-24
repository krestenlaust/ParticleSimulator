using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI.UI.Controls
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
        private readonly LabelControl text = new LabelControl();
        private PixelColor borderColor;
        private bool pressed;

        public string Text
        {
            get { return text.Text; } 
            set { text.Text = value; }
        }

        public ButtonControl()
        {
            Height = 5;
            text.Height = 1;
            borderChar = '\u26db';
            borderHoverColor = new PixelColor(ConsoleColor.DarkBlue);
            borderDefaultColor = new PixelColor(ConsoleColor.Blue);
            borderPressedColor = new PixelColor(ConsoleColor.DarkCyan);
            borderColor = borderDefaultColor;
        }

        public ButtonControl(PixelColor borderHoverColor, PixelColor borderDefaultColor, char borderChar='\u26db')
        {
            Height = 5;
            text.Height = 1;
            this.borderChar = borderChar;
            this.borderHoverColor = borderHoverColor;
            this.borderDefaultColor = borderDefaultColor;
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

        protected internal override void UpdateButtonState(MouseButtonState newState)
        {
            switch (newState)
            {
                case MouseButtonState.Down:
                    pressed = true;

                    borderColor = borderPressedColor;
                    break;
                case MouseButtonState.Release:
                    pressed = false;

                    OnClick?.Invoke();
                    //borderColor = borderPressedColor;
                    break;
                default:
                    break;
            }

            base.UpdateButtonState(newState);
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
