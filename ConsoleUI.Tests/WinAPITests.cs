using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ConsoleUI.Tests
{
    [TestClass]
    public class WinAPITests
    {
        /// <summary>
        /// Det forventes at GetStdHandle returnerer et "Handle", hvis værdi ikke er nul
        /// </summary>
        [TestMethod]
        public void TestGetStdHandle()
        {
            const int OutputHandle = (int)WinAPI.StdHandle.OutputHandle; // constant indicating StdOut handle
            IntPtr handle = WinAPI.GetStdHandle(OutputHandle); // Try to get the handle
            Assert.IsNotNull(handle); // Make sure the handle returned is not null
        }
    }
}
