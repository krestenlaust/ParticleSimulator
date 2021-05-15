using System;

namespace ConsoleUI.UI.Controls
{
    public class LabelControl : Control
    {
        public string Text;

        public LabelControl(string text)
        {
            Width = text.Length + 2;
            Text = text;
        }

        public override void Draw(TableSegment<WinAPI.CharInfo> segment)
        {
            int writeLength = Math.Min(segment.Width, Text.Length + 2);
            for (int i = 0; i < writeLength; i++)
            {
                if (i == 0 || i == Text.Length + 1)
                {
                    segment[i, 0] = ScreenBuffer.ConvertPixel('#');
                }
                else
                {
                    segment[i, 0] = ScreenBuffer.ConvertPixel(Text[i - 1]);
                }
            }
        }
    }
}
