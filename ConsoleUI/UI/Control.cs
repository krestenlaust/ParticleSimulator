using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.UI
{
    public abstract class Control
    {
        public int Zindex;
        public int X, Y;
        public int Width = 1, Height = 1;

        public abstract void Draw(TableSegment<WinAPI.CharInfo> segment);
    }
}
