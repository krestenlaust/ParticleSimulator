using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ConsoleUI
{
    public static class WinAPI
    {
        private enum ConsoleMode : uint
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

        private const int GWL_STYLE = -16;

        private enum WindowStyles : long
        {
            WS_MAXIMIZEBOX = 0x00010000L,
            WS_SIZEBOX = 0x00040000L
        }

        [DllImport("Kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);

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

        [DllImport("Kernel32.dll")]
        public static extern bool SetConsoleOutputCP(uint CodePageID);


        /* Hurtigere console write */
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
        string fileName,
        [MarshalAs(UnmanagedType.U4)] uint fileAccess,
        [MarshalAs(UnmanagedType.U4)] uint fileShare,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] int flags,
        IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
        SafeFileHandle hConsoleOutput,
        CharInfo[] lpBuffer,
        COORD dwBufferSize,
        COORD dwBufferCoord,
        ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        /// Writes to the console faster with color supported.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static void WriteColorFast(CharInfo[] buffer)
        {
            // Open a safe handle to STDOUT
            SafeFileHandle stdOut = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            if (!stdOut.IsInvalid)
            {
                COORD bufferSize = new COORD((short)UIManager.Width, (short)UIManager.Height);
                SmallRect writeArea = new SmallRect() { Left = 0, Top = 0, Right = (short)UIManager.Width, Bottom = (short)UIManager.Height };
                WriteConsoleOutput(stdOut, buffer, bufferSize, new COORD(0, 0), ref writeArea);
            }
        }
        /* hurtigere console write slut */

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