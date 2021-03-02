namespace ConsoleUI
{
    /// <summary>
    /// Based on these codes: https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
    /// </summary>
    public struct ANSIColor
    {
        public enum Color : byte
        {
            Black = 30,
            Red = 31,
            Green = 32,
            Yellow = 33,
            Blue = 34,
            Magenta = 35,
            Cyan = 36,
            White = 37
        }

        public enum Ground : byte
        {
            Fore = 0,
            Back = 10
        }

        public enum Special : byte
        {
            Default = 0,
            AddUnderline = 4,
            RemoveUnderline = 24,
            AddBold = 1,
            RemoveBold = 22,
            SwapForegroundBackground = 7,
            UnSwapForegroundBackground = 27
        }

        public readonly byte Value;

        /// <summary>
        /// Foreground color.
        /// </summary>
        /// <param name="color"></param>
        public ANSIColor(Color color)
        {
            Value = (byte)color;
        }

        /// <summary>
        /// Foreground color, bright or non-bright color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="bright"></param>
        public ANSIColor(Color color, bool bright)
        {
            Value = (byte)((byte)color + (bright ? 60 : 0));
        }

        /// <summary>
        /// Foreground or background color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="ground"></param>
        public ANSIColor(Color color, Ground ground)
        {
            Value = (byte)((byte)color + (byte)ground);
        }

        /// <summary>
        /// Special cases like underline.
        /// </summary>
        /// <param name="special"></param>
        public ANSIColor(Special special)
        {
            Value = (byte)special;
        }

        /// <summary>
        /// Foreground or background, bright or non-bright color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="ground"></param>
        /// <param name="bright"></param>
        public ANSIColor(Color color, Ground ground, bool bright)
        {
            Value = (byte)((byte)color + (byte)ground + (bright ? 60 : 0));
        }

        public override string ToString() => Value.ToString();
    }
}
