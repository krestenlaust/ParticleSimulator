using System;

namespace ConsoleUI
{
    public struct PixelColor
    {
        public ConsoleColor ForegroundColor => (ConsoleColor)foregroundValue;
        public ConsoleColor BackgroundColor => (ConsoleColor)backgroundValue;
        public short AttributeValue => (short)(foregroundValue + backgroundValue * 16);

        private byte foregroundValue;
        private byte backgroundValue;

        public PixelColor(ConsoleColor foregroundColor)
        {
            foregroundValue = (byte)foregroundColor;
            backgroundValue = (byte)ConsoleColor.Black;
        }

        public PixelColor(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            foregroundValue = (byte)foregroundColor;
            backgroundValue = (byte)backgroundColor;
        }
    }
}
