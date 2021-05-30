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
            Assert.IsNotNull(WinAPI.GetStdHandle((int)WinAPI.StdHandle.OutputHandle));
        }
    }
}
