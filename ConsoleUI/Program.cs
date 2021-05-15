using System.Numerics;
using System.Diagnostics;
using System.Threading;
using ConsoleInput;
using ParticleEngine;
using ParticleEngine.Particles;
using System;
using System.Threading.Tasks;
using ConsoleUI.UI.Controls;

namespace ConsoleUI
{
    class Program
    {
        const int FramesPerSecondCap = 60;

        static void Main(string[] args)
        {
            // Define console screen sizes to allow easy line wrapping.
            ScreenBuffer.Setup(90, 40);

            // Call Windows API to enable our specific console needs.
            WinAPI.EnableANSIProcessing();
            //WinAPI.SetFontSize(16, 16);

            // Set up the window style
            WinAPI.SetupStyle();

            // Call console input library.
            Input.Setup(false);

            // Set up game
            ScreenBuffer.Controls.Add(new GameControl(false)
            {
                Width = ScreenBuffer.Width,
                Height = ScreenBuffer.Height - 20
            });

            ScreenBuffer.Controls.Add(new LabelControl("Hello world")
            {
                Y = ScreenBuffer.Height - 18
            });

            // Continue setting up engine
            // Stopwatch to measure game loop time.
            Stopwatch stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Restart();

                // Call console input library to update input-logic.
                Input.Update();

                if (Mouse.MouseDown[0])
                {
                    // Jeg har ændret på brush størrelsen og længden de er adskilt.
                    // ok - Kresten
                    // jeg sætter mit navn på det her nu :)
                    // ikke i orden - Kresten
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x - 1, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x + 1, Mouse.y));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y + 1));
                    Physics.Instantiate<Sand>(new Vector2(Mouse.x, Mouse.y - 1));
                }
                else if (Mouse.MouseDown[1])
                {
                    Physics.Instantiate<Water>(new Vector2(Mouse.x, Mouse.y));
                }
                else if (Mouse.MouseDown[2])
                {
                    Physics.Instantiate<Acid>(new Vector2(Mouse.x, Mouse.y));
                }

                Physics.Update();

                ScreenBuffer.Render();

                ScreenBuffer.ApplyBuffer();

                // Sat før timeout for at måle den potentielle FPS.
                Console.Title = $"FPS: {Math.Floor(1 / stopwatch.Elapsed.TotalSeconds)}";

                // Når man sætter Thread.Sleep's parameter til 0 sætter den tråden i en kø og så vender den tilbage når den "har tid".
                // Det vil give en mere nøjagtig tid end hvis man bare indtastede det antal milisekunder man ville vente, 
                // så ville det være: ventetid + abitrer timeout før der vendes tilbage til tråden.
                while (stopwatch.ElapsedMilliseconds < 1000 / FramesPerSecondCap)
                    Thread.Sleep(0);
            }
        }
    }
}