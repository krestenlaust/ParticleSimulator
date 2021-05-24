using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleUI.UI
{
    /// <summary>
    /// En scene er en samling af ui-elementer.
    /// </summary>
    public class Scene
    {
        public readonly List<Control> Controls = new List<Control>();
    }
}
