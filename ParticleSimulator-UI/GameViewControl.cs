using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParticleSimulator_UI
{
    public partial class GameViewControl : PictureBox
    {
        Bitmap renderingBitmap;
        Bitmap shownBitmap;

        public GameViewControl()
        {
            InitializeComponent();

            renderingBitmap = new Bitmap(Size.Width, Size.Height);
            shownBitmap = new Bitmap(Size.Width, Size.Height);
        }

        public void SetPixels(Vector2[] vectors, int length, Color color)
        {
            for (int i = 0; i < length; i++)
            {
                try
                {
                    renderingBitmap.SetPixel((int)vectors[i].X % renderingBitmap.Width, (int)vectors[i].Y % renderingBitmap.Height, color);
                }
                catch (Exception)
                {
                }
            }
        }

        public void RefreshView()
        {
            shownBitmap = renderingBitmap;
            renderingBitmap = new Bitmap(shownBitmap.Width, shownBitmap.Height);
            Image = shownBitmap;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
