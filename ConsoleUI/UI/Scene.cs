using System.Collections.Generic;

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
