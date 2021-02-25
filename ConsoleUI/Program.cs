using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ConsoleUI
{
    class Program
    {
        static int width;
        static int height;

        static void Main(string[] args)
        {
            width = 100;
            height = 50;

            Console.WindowWidth = width;
            Console.WindowHeight = height;
        }

        static void DrawDots(Vector2[] dots, int length)
        {
            char[] newDots = new char[width * height];

            for (int i = 0; i < length; i++)
            {

            }
        }
    }
}
