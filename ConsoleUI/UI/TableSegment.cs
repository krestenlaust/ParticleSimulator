﻿
namespace ConsoleUI.UI
{
    /// <summary>
    /// En struktur, der beskriver en begrænset tabel af en 1-dimensionel array.
    /// </summary>
    public struct TableSegment<T>
    {
        public readonly int Height, Width;
        /// <summary>
        /// Forskydningen på de 2-dimensioner.
        /// </summary>
        private readonly int offsetX, offsetY;
        /// <summary>
        /// Den array som begrænsningen er baseret på.
        /// </summary>
        private readonly T[] internalArray;
        /// <summary>
        /// Bredden af <c>internalArray</c>. Siden <c>internalArray</c> er 1-dimensionel i praksis, skal man kende bredden for at udregne et givent 1D indeks ud fra 2D indeks.
        /// </summary>
        private readonly int internalWidth;

        public TableSegment(T[] array, int arrayWidth, int segmentWidth, int segmentHeight, int offsetX, int offsetY)
        {
            internalArray = array;
            Height = segmentHeight;
            Width = segmentWidth;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            internalWidth = arrayWidth;
        }

        public T this[int x, int y]
        {
            get
            {
                return internalArray[CalculateIndex(x, y)];
            }
            set
            {
                internalArray[CalculateIndex(x, y)] = value;
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
