using System;

namespace ConsoleInteraction.UI.Controls
{
    public class LabelControl : Control
    {
        public string Text;

        public LabelControl(string text) : base(text.Length, 1)
        {
            Width = text.Length;
            Text = text;
        }

        public override void Draw(ScreenSegment segment)
        {
            int writeLength = Math.Min(segment.Width, Text.Length);
            for (int i = 0; i < writeLength; i++)
            {
                segment[i, 0] = UIManager.ConvertPixel(Text[i]);
            }
        }
    }
}
