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
        private enum ConsoleMode : uint
        {
            ENABLE_PROCESSED_OUTPUT = 0x0001,
            ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004
        }

        private enum StdHandle : int
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

        private const int GWL_STYLE = -16;

        private enum WindowStyles : long
        {
            WS_MAXIMIZEBOX = 0x00010000L,
            WS_SIZEBOX = 0x00040000L
        }


        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("Kernel32.dll")]
        private static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFO_EX pConsoleCurrentFontEx);

        [DllImport("Kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsole, uint dwMode);

        [DllImport("Kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsole, out uint lpMode);

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        private static extern long GetWindowLongA(IntPtr hWindow, int nIndex);

        [DllImport("User32.dll")]
        private static extern long SetWindowLongA(IntPtr hWindow, int nIndex, long dwNewLong);
    

        /// <summary>
        /// Enables console output mode "VIRTUAL_TERMINAL_PROCESSING" to enable processing of ANSI colors.
        /// </summary>
        public static void EnableANSIProcessing()
        {
            IntPtr outHandle = GetStdHandle((int)StdHandle.OutputHandle);

            // Gets current console modes to add a mode to it.
            GetConsoleMode(outHandle, out uint lpMode);
            // Setting console mode, including old settings.
            SetConsoleMode(outHandle, (int)ConsoleMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING | lpMode);
        }

        public static void SetupStyle()
        {
            IntPtr hWindow = GetConsoleWindow(); // Get handle to console window
            long style = GetWindowLongA(hWindow, GWL_STYLE); // Retrieve style
            style ^= (long)WindowStyles.WS_SIZEBOX; // Zero the WS_SIZEBOX bit to prevent resizing
            style ^= (long)WindowStyles.WS_MAXIMIZEBOX; // Zero the WS_MAXIMIZEBOX bit to remove the maximize button
            SetWindowLongA(hWindow, GWL_STYLE, style); // Set the modified style
        }

        /// <summary>
        /// Updates the console font size.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool SetFontSize(short width, short height)
        {
            CONSOLE_FONT_INFO_EX fontInfo = new CONSOLE_FONT_INFO_EX();
            fontInfo.dwFontSize.X = width;
            fontInfo.dwFontSize.Y = height;
            fontInfo.FontFamily = 0x04;
            fontInfo.cbSize = (uint)Marshal.SizeOf(fontInfo);

            IntPtr outHandle = GetStdHandle((int)StdHandle.OutputHandle);

            return SetCurrentConsoleFontEx(outHandle, false, ref fontInfo);
        }
    }
}
