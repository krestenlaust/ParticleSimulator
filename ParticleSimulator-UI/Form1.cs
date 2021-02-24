using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Numerics;
using System.Diagnostics;

namespace ParticleSimulator_UI
{
    public partial class FormGame : Form
    {
        private int WorkLoops = 0;
        private static Graphics g;
        private Vector2[] dots = new Vector2[40000];
        private int currentIndex = 0;
        private bool clicked;
        private int GameWidth;
        private int GameHeight;
        private readonly Stopwatch stopwatch = new Stopwatch();

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

        private void backgroundWorkerGame_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                WorkLoops++;

                HashSet<Vector2> drawn = new HashSet<Vector2>();

                for (int i = 0; i < currentIndex; i++)
                {
                    if (drawn.Contains(dots[i]))
                    {
                        continue;
                    }

                    drawn.Add(dots[i]);

                    g.FillRectangle(Brushes.Black, dots[i].X, dots[i].Y, 8, 8);

                    dots[i].Y += 10;

                    g.FillRectangle(Brushes.Yellow, dots[i].X, dots[i].Y, 8, 8);

                }

                stopwatch.Restart();
                while (stopwatch.ElapsedMilliseconds >= 1000 / 60)
                    Thread.Sleep(0);
            }
        }

        private void FormGame_MouseMove(object sender, MouseEventArgs e)
        {
            if (!clicked)
            {
                return;
            }

            //Thread.Sleep(20);

            Vector2 rect = new Vector2(
                (int)Math.Round((decimal)(Cursor.Position.X - Left) % Width / 8) * 8,
                (int)Math.Round((decimal)(Cursor.Position.Y - Top) % Height / 8) * 8);
           
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