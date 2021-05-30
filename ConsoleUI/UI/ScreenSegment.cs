
using System;

namespace ConsoleUI.UI
{
    /// <summary>
    /// En struktur, der beskriver en begrænset tabel af en 1-dimensionel array.
    /// </summary>
    public struct ScreenSegment
    {
        public readonly int Height, Width;
        /// <summary>
        /// Forskydningen på de 2-dimensioner.
        /// </summary>
        private readonly int offsetX, offsetY;
        /// <summary>
        /// Den array som begrænsningen er baseret på.
        /// </summary>
        private readonly WinAPI.CharInfo[] internalArray;
        /// <summary>
        /// Bredden af <c>internalArray</c>. Siden <c>internalArray</c> er 1-dimensionel i praksis, skal man kende bredden for at udregne et givent 1D indeks ud fra 2D indeks.
        /// </summary>
        private readonly int internalWidth;

        public ScreenSegment(WinAPI.CharInfo[] array, int arrayWidth, int segmentWidth, int segmentHeight, int offsetX, int offsetY)
        {
            internalArray = array;
            Height = segmentHeight;
            Width = segmentWidth;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            internalWidth = arrayWidth;
        }

        public ScreenSegment(ScreenSegment segment, int segmentWidth, int segmentHeight, int offsetX, int offsetY)
        {
            internalArray = segment.internalArray;
            Height = segmentHeight;
            Width = segmentWidth;
            this.offsetX = offsetX + segment.offsetX;
            this.offsetY = offsetY + segment.offsetY;
            internalWidth = segment.internalWidth;
        }

        public WinAPI.CharInfo this[int x, int y]
        {
            get
            {
                // Hvis området er udenfor det der er blevet tildelt.
                if (x >= Width || x < 0 || y >= Height || y < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                int calculatedIndex = CalculateIndex(x, y);

                // Hvis det faktiske indeks er udenfor. Dette sker fx. når et element er ude i siden.
                if (calculatedIndex < 0 || calculatedIndex >= internalArray.Length)
                {
                    return new WinAPI.CharInfo { };
                }

                return internalArray[calculatedIndex];
            }
            set
            {
                // Hvis området er udenfor det der er blevet tildelt.
                if (x >= Width || x < 0 || y >= Height || y < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                int calculatedIndex = CalculateIndex(x, y);

                // Hvis det faktiske indeks er udenfor.
                if (calculatedIndex < 0 || calculatedIndex >= internalArray.Length)
                {
                    return;
                }

                internalArray[calculatedIndex] = value;
            }
        }

        /// <summary>
        /// Udregner det 1-dimensionelle indeks af en placering inde i den begrænsede (2-dimensionelle) array, baseret på det 2-dimensionelle indeks.
        /// </summary>
        /// <param name="externalX"></param>
        /// <param name="externalY"></param>
        /// <returns></returns>
        private int CalculateIndex(int externalX, int externalY) => (externalY + offsetY) * internalWidth + (externalX + offsetX);
    }
}
