namespace ConsoleInteraction.UI
{
    public abstract class Control
    {
        public enum HoverState
        {
            None,
            Enter,
            Stay,
            Exit
        }

        public int Zindex;
        public int X, Y;
        public int Width, Height;
        public event HoverStateChanged OnHoverStateChanged;
        /// <summary>
        /// Kaldt mens musen er over en kontrol. Brug <c>Mouse</c>-klassen for at tilgå musens tilstand.
        /// </summary>
        public event MouseButtonStateChanged OnMouseButtonStateChanged;

        internal HoverState InternalHoverState { get; private set; } = HoverState.None;

        public abstract void Draw(ScreenSegment segment);
        public delegate void HoverStateChanged(Control source, HoverState newState);
        public delegate void MouseButtonStateChanged(Control source);

        public Control(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool IsPointInside(int pointX, int pointY)
        {
            // Er punktet til venstre-for eller ovenover elementet, kan det ikke være på det.
            if (pointX < X || pointY < Y)
            {
                return false;
            }

            // Er punktet indenfor bredden og højden af elementet.
            if (pointX - X >= Width || pointY - Y >= Height)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kaldt når musen flytter sig ud eller ind i et ui-element. Den bliver også kaldt hele den tid den er indenfor.
        /// </summary>
        /// <param name="newState"></param>
        protected internal virtual void UpdateHoverState(HoverState newState)
        {
            InternalHoverState = newState;

            if (newState == HoverState.None)
            {
                return;
            }

            OnHoverStateChanged?.Invoke(this, newState);
        }

        /// <summary>
        /// Kaldt mens musen er over en kontrol. Brug <c>Mouse</c>-klassen for at tilgå musens tilstand.
        /// </summary>
        protected internal virtual void UpdateButtonState() => OnMouseButtonStateChanged?.Invoke(this);
    }
}
