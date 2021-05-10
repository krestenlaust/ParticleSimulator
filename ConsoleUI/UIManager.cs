using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public static class UIManager
    {
        private static readonly List<Control> controls;

        public static void AddControl(Control control)
        {
            controls.Add(control);
        }
    }
}
