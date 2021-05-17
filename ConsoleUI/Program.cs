using System.Numerics;
using System.Diagnostics;
using System.Threading;
using ConsoleInput;
using ParticleEngine;
using ParticleEngine.Particles;
using System;
using System.Threading.Tasks;
using ConsoleUI.UI.Controls;
using ConsoleUI.UI;

namespace ConsoleUI
{
    class Program
    {
        const int FramesPerSecondCap = 60;

        static void Main(string[] args)
        {
            // Define console screen sizes to allow easy line wrapping.
            UIManager.Setup(90, 40);

            // Call Windows API to enable our specific console needs.
            WinAPI.EnableANSIProcessing();
            //WinAPI.SetFontSize(16, 16);

            // Set up the window style
            WinAPI.SetupStyle();

            // Call console input library.
            Input.Setup(false);

            // Set up game
            UIManager.Controls.Add(new GameControl(false)
            {
                Width = UIManager.Width,
                Height = UIManager.Height - 20
            });


            LabelControl helloworldLabel = new LabelControl("Hello world")
            {
                Y = UIManager.Height - 18
            };
            helloworldLabel.OnMouseButtonStateChanged += HelloworldLabel_OnMouseButtonStateChanged;
            helloworldLabel.OnHoverStateChanged += HelloworldLabel_OnHoverStateChanged;
            UIManager.Controls.Add(helloworldLabel);

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
                    Physics.Instantiate<Block>(new Vector2(Mouse.x, Mouse.y));
                }

                Physics.Update();

                UIManager.LogicAndRender();

                UIManager.ApplyBuffer();

                int dotCount = 0;
                foreach (var item in Physics.ParticleGroups)
                {
                    dotCount += item.Particles.Count;
                }

                // Sat før timeout for at måle den potentielle FPS.
                Console.Title = $"DOTS: {dotCount}; FPS: {Math.Floor(1 / stopwatch.Elapsed.TotalSeconds)}";

                // Når man sætter Thread.Sleep's parameter til 0 sætter den tråden i en kø og så vender den tilbage når den "har tid".
                // Det vil give en mere nøjagtig tid end hvis man bare indtastede det antal milisekunder man ville vente, 
                // så ville det være: ventetid + abitrer timeout før der vendes tilbage til tråden.
                while (stopwatch.ElapsedMilliseconds < 1000 / FramesPerSecondCap)
                    Thread.Sleep(0);
            }
        }

        private static void HelloworldLabel_OnMouseButtonStateChanged(Control source, Control.MouseButtonState newState)
        {
            LabelControl label = (LabelControl)source;

            switch (newState)
            {
                case Control.MouseButtonState.None:
                    break;
                case Control.MouseButtonState.Down:
                    label.Text = "Halløj World";
                    break;
                case Control.MouseButtonState.Hold:
                    break;
                case Control.MouseButtonState.Release:
                    label.Text = "Hello World";
                    break;
                default:
                    break;
            }
        }

        private static void HelloworldLabel_OnHoverStateChanged(Control source, Control.HoverState newState)
        {
            LabelControl label = (LabelControl)source;

            switch (newState)
            {
                case Control.HoverState.None:
                    break;
                case Control.HoverState.Enter:
                    label.X += 1;
                    break;
                case Control.HoverState.Stay:
                    break;
                case Control.HoverState.Exit:
                    label.X -= 1;
                    break;
                default:
                    break;
            }
        }
    }
}