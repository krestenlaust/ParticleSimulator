using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI.UI
{
    public abstract class Control
    {
        public enum HoverState
        {
            None,
            Enter,
            Stay,
            Exit,
        }
        public enum MouseButtonState
        {
            None,
            Down,
            Hold,
            Release,
        }

        public int Zindex;
        public int X, Y;
        public int Width = 1, Height = 1;
        public event HoverStateChanged OnHoverStateChanged;
        public event MouseButtonStateChanged OnMouseButtonStateChanged;

        internal HoverState InternalHoverState { get; private set; } = HoverState.None;
        internal MouseButtonState InternalMouseButtonState { get; private set; } = MouseButtonState.None;

        public abstract void Draw(ScreenSegment segment);
        public delegate void HoverStateChanged(Control source, HoverState newState);
        public delegate void MouseButtonStateChanged(Control source, MouseButtonState newState);

        public bool IsPointInside(int pointX, int pointY)
        {
            // Er punktet til venstre-for eller ovenover elementet, kan det ikke være på det.
            if (pointX < X || pointY < Y)
            {
                return false;
            }

            // Er punktet indenfor bredden og højden af elementet.
            if (pointX - X > Width || pointY - Y > Height)
            {
                return false;
            }

            return true;
        }

        protected internal virtual void UpdateHoverState(HoverState newState)
        {
            InternalHoverState = newState;

            if (newState == HoverState.None)
            {
                return;
            }

            OnHoverStateChanged?.Invoke(this, newState);
        }

        protected internal virtual void UpdateButtonState(MouseButtonState newState)
        {
            InternalMouseButtonState = newState;

            if (newState == MouseButtonState.None)
            {
                return;
            }

            OnMouseButtonStateChanged?.Invoke(this, newState);
        }
    }
}
