using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public readonly byte Value;

        public ANSIColor(Color color, Ground ground, bool bright)
        {
            Value = (byte)((byte)color + (byte)ground + (bright ? 60 : 0));
        }

        public override string ToString() => Value.ToString();
    }
}
