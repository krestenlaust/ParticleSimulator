using ConsoleUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConsoleUI.UI;

namespace ConsoleUI.Tests
{
    [TestClass]
    public class ScreenSegmentTests
    {
        [TestMethod]
        public void TestReadScreenSegmentWith1DimensionalArray()
        {
            WinAPI.CharInfo[] charArray = new WinAPI.CharInfo[]
            {
                new WinAPI.CharInfo{ Char = new WinAPI.CharUnion{ UnicodeChar = 'A' } },
                new WinAPI.CharInfo{ Char = new WinAPI.CharUnion{ UnicodeChar = 'B' } },
                new WinAPI.CharInfo{ Char = new WinAPI.CharUnion{ UnicodeChar = 'C' } }
            };

            // 1 Tall, 2 Wide, 1 offset-x, 0 offset-y
            ScreenSegment segment = new ScreenSegment(charArray, charArray.Length, 1, 2, 1, 0);

            // Antag første element af segmentet er lig andet element af den originale.
            Assert.AreEqual(charArray[1], segment[0, 0]);
        }

        [TestMethod]
        public void TestScreenSegmentBounds()
        {
            WinAPI.CharInfo[] charArray = new WinAPI.CharInfo[]
            {
                new WinAPI.CharInfo{ Char = new WinAPI.CharUnion{ UnicodeChar = 'A' } },
                new WinAPI.CharInfo{ Char = new WinAPI.CharUnion{ UnicodeChar = 'B' } },
                new WinAPI.CharInfo{ Char = new WinAPI.CharUnion{ UnicodeChar = 'C' } }
            };

            // 1 Tall, 2 Wide, 1 offset-x, 0 offset-y
            ScreenSegment segment = new ScreenSegment(charArray, charArray.Length, 1, 2, 1, 0);

            // Antag den kaster en exception siden 2 er udenfor.
            Assert.ThrowsException<IndexOutOfRangeException>(() => segment[2, 0]);
        }
    }
}
