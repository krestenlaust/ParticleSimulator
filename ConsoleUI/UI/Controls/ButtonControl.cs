using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.UI.Controls
{
    public class ButtonControl : Control
    {
        public char BorderChar;

        public string Text
        {
            get { return text.Text; } 
            set { text.Text = value; }
        }

        private LabelControl text = new LabelControl();

        public ButtonControl()
        {
            Height = 5;
            text.Height = 1;
        }

        public override void Draw(ScreenSegment segment)
        {
            if (Height < 3 || Width < 3)
            {
                return;
            }

            for (int y = 0; y < Height; y++)
            {
                segment[0, y] = UIManager.ConvertPixel('#');
                segment[segment.Width - 1, y] = UIManager.ConvertPixel('#');

                // center
                if ((y-1) / 2f == 0.5f)
                {
                    text.Width = Math.Min(text.Text.Length, Width - 2);
                    ScreenSegment subSegment = new ScreenSegment(segment, text.Width, 1, 2, y);
                    text.Draw(subSegment);
                }
                else if (y % (Height - 1) == 0)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        segment[x, y] = UIManager.ConvertPixel('\u26db');
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
