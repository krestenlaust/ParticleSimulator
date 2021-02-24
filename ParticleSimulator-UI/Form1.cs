using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ParticleSimulator_UI
{
    public partial class FormGame : Form
    {
        private int WorkLoops = 0;
        private static Graphics g;
        private Rectangle[] dots = new Rectangle[40000];
        private int currentIndex = 0;
        private bool clicked;

        public FormGame()
        {
            InitializeComponent();

            DoubleBuffered = true;
            g = CreateGraphics();
            backgroundWorkerGame.RunWorkerAsync();
        }

        private void FormGame_Paint(object sender, PaintEventArgs e)
        {
            Text = "Kresten tegneren :)";
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.Black, 0, 0, Width, Height - 100);
        }

        private Rectangle GetRect(ref Rectangle rect) => rect;
        
        private void backgroundWorkerGame_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                WorkLoops++;

                for (int i = 0; i < currentIndex; i++)
                {
                    dots[i].Y += 1;
                }

                var fakeDots = (Rectangle[])dots.Clone();
                for (int i = 0; i < fakeDots.Length; i++)
                {
                    g.
                }

                g.FillRectangles(Brushes.Black, fakeDots);
                g.FillRectangles(Brushes.OrangeRed, dots);

                Thread.Sleep(1*1000/60/1);
            }
        }

        private void FormGame_MouseMove(object sender, MouseEventArgs e)
        {
            if (!clicked)
            {
                return;
            }

            //Thread.Sleep(20);

            Rectangle rect = new Rectangle(
                (int)Math.Round((decimal)(Cursor.Position.X - Left) % Width / 4) * 4, 
                (int)Math.Round((decimal)(Cursor.Position.Y - Top) % Height / 4) * 4, 4, 4);
           
            if (!dots.Contains(rect))
            {
                dots[currentIndex++] = rect;
            }
        }

        private void FormGame_MouseDown(object sender, MouseEventArgs e)
        {
            clicked = true;
        }

        private void FormGame_MouseUp(object sender, MouseEventArgs e)
        {
            clicked = false;
        }
    }
}