using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public static class WinAPI
    {
        [DllImport("Kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("Kernel32.dll")]
        private static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFO_EX pConsoleCurrentFontEx);

        [DllImport("Kernel32.dll")]
        public static extern bool SetConsoleMode(IntPtr hConsole, uint dwMode);

        [DllImport("Kernel32.dll")]
        public static extern bool GetConsoleMode(IntPtr hConsole, out uint lpMode);

        public enum ConsoleMode : uint
        {
            ENABLE_PROCESSED_OUTPUT = 0x0001,
            ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004
        }

        public enum StdHandle : int
        {
            OutputHandle = -11,
            InputHandle = -10
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public short X;
            public short Y;

            public COORD(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CONSOLE_FONT_INFO_EX
        {
            public uint cbSize;
            public uint nFont;
            public COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // Edit sizeconst if the font name is too big
            public string FaceName;
        }

        
        public static bool SetFontSize(short width, short height)
        {
            CONSOLE_FONT_INFO_EX fontInfo = new CONSOLE_FONT_INFO_EX();
            fontInfo.dwFontSize.X = width;
            fontInfo.dwFontSize.Y = height;
            fontInfo.FontFamily = 0x04;
            fontInfo.cbSize = (uint)Marshal.SizeOf(fontInfo);

            if (SetCurrentConsoleFontEx(GetStdHandle((int)StdHandle.OutputHandle), false, ref fontInfo))
                return true;

            return false;
        }

    }
}
