using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParticleSimulator_UI
{
    public partial class FormGame : Form
    {
        private int WorkLoops = 0;
        private static Graphics g;

        public FormGame()
        {
            InitializeComponent();

            g = CreateGraphics();
            backgroundWorkerGame.RunWorkerAsync();
        }

        private void FormGame_Paint(object sender, PaintEventArgs e)
        {
            Text = "Kresten tegneren";
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.Red, 0, 0, Width, Height - 100);
        }

        private void backgroundWorkerGame_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                WorkLoops++;

                g.FillRectangle(Brushes.Black, 100, 100, 100, 100);
                g.FillRectangles()
                //g.FillRectangle(Brushes.Black, 10+WorkLoops, 100, 100, 100);
                //g.DrawString(WorkLoops.ToString(), Font, Brushes.White, 10, 10);
                Thread.Sleep(10);
            }
        }
    }
}