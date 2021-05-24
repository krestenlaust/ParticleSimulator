using System.Numerics;
using System.Diagnostics;
using System.Threading;
using ConsoleInput;
using ParticleEngine;
using ParticleEngine.Particles;
using System;
using ConsoleUI.UI.Controls;
using ConsoleUI.UI;
using ConsoleUI;

namespace GameImplementation
{
    class Program
    {
        const int FramesPerSecondCap = 60;
        static Scene mainMenu;
        static Scene gameScene;
        static Scene pauseMenu;
        static Scene creditsScene;
        static bool running = true;

        /// <summary>
        /// Kan bl.a. bruges til at opsætte scener.
        /// </summary>
        static void Start()
        {
            // Set up menu scene
            mainMenu = new Scene();
            mainMenu.Controls.Add(new ButtonControl
            {
                Height = 5,
                Width = 15,
                X = UIManager.Width / 2 - 15,
                Y = UIManager.Height / 5 + 5,
                Text = "Start",
                OnClick = () =>
                {
                    UIManager.ChangeScene(gameScene);
                }
            });
            mainMenu.Controls.Add(new ButtonControl
            {
                Height = 5,
                Width = 15,
                X = UIManager.Width / 2 - 15,
                Y = UIManager.Height / 5 * 2 + 5,
                Text = "Credits",
                OnClick = () =>
                {
                    UIManager.ChangeScene(creditsScene);
                }
            });
            mainMenu.Controls.Add(new ButtonControl
            {
                Height = 5,
                Width = 15,
                X = UIManager.Width / 2 - 15,
                Y = UIManager.Height / 5 * 3 + 5,
                Text = "Exit",
                OnClick = () =>
                {
                    running = false;
                }
            });

            // Set up pause scene
            pauseMenu = new Scene();
            pauseMenu.Controls.Add(new ButtonControl
            {
                Height = 5,
                Width = 15,
                X = UIManager.Width / 2 - 15,
                Y = UIManager.Height / 5 + 5,
                Text = "Resume",
                OnClick = () =>
                {
                    UIManager.ChangeScene(gameScene);
                }
            });
            pauseMenu.Controls.Add(new ButtonControl
            {
                Height = 5,
                Width = 15,
                X = UIManager.Width / 2 - 15,
                Y = UIManager.Height / 5 * 2 + 5,
                Text = "Back to menu",
                OnClick = () =>
                {
                    UIManager.ChangeScene(mainMenu);
                }
            });

            // Set up credits scene
            creditsScene = new Scene();
            creditsScene.Controls.Add(new ButtonControl
            {
                Height = 3,
                Width = 10,
                X = 0,
                Y = UIManager.Height / 5,
                Text = "Back",
                OnClick = () =>
                {
                    UIManager.ChangeScene(mainMenu);
                }
            });
            creditsScene.Controls.Add(new LabelControl("Made by: Patrick, Alexander and Kresten")
            {
                Y = UIManager.Height / 5 + 5
            });

            // Set up game scene
            gameScene = new Scene();
            gameScene.Controls.Add(new GameControl(true)
            {
                Width = UIManager.Width,
                Height = UIManager.Height - 20
            });

            ButtonControl testButton = new ButtonControl()
            {
                Y = UIManager.Height - 10,
                Width = 15,
                Height = 5,
                Text = "Hi"
            };
            gameScene.Controls.Add(testButton);

            UIManager.ChangeScene(mainMenu);
        }

        static void Update()
        {
            if (Keyboard.KeyDown(Keyboard.VK.ESCAPE) && UIManager.CurrentScene != mainMenu)
            {
                UIManager.ChangeScene(UIManager.CurrentScene == gameScene ? pauseMenu : gameScene);
            }
        }

        static void Main(string[] args)
        {
            // Define console screen sizes to allow easy line wrapping.
            UIManager.Setup(90, 40);
            // Call console input library.
            Input.Setup(false);

            Start();

            // Continue setting up engine
            // Stopwatch to measure game loop time.
            Stopwatch stopwatch = new Stopwatch();

            while (running)
            {
                stopwatch.Restart();

                // Call console input library to update input-logic.
                Input.Update();

                Update();

                Physics.Update();
                UIManager.Draw();
                UIManager.RenderBuffer();

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
