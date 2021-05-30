using ConsoleInput;
using ConsoleInteraction;
using ConsoleInteraction.UI;
using ConsoleInteraction.UI.Controls;
using ParticleEngine;
using ParticleEngine.Particles;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace GameImplementation
{
    class Program
    {
        const int FramesPerSecondCap = 60;
        const int ControlHorizontalMargin = 2;
        const int ButtonVerticalMargin = 2;
        const int ButtonWidth = 10;
        const int ButtonHeight = 3;
        static Scene mainMenu;
        static Scene gameScene;
        static Scene pauseMenu;
        static Scene creditsScene;
        static GameControl gameControl;
        static bool running = true;

        /// <summary>
        /// Kan bl.a. bruges til at opsætte scener.
        /// </summary>
        static void SetupScenes()
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
            gameControl = new GameControl(true, UIManager.Width, UIManager.Height - 10);
            gameScene.Controls.Add(gameControl);
            gameScene.Controls.Add(new LabelControl("Use left mouse button to place 1 particle, and right to place many.")
            {
                X = ControlHorizontalMargin,
                Y = gameControl.Height
            });
            PixelColor pressedColor = new PixelColor(ConsoleColor.Gray);
            int buttonCount = 0;
            // - Sand
            gameScene.Controls.Add(new ButtonControl(new PixelColor(ConsoleColor.Yellow), new PixelColor(ConsoleColor.DarkYellow), pressedColor)
            {
                OnClick = () => gameControl.SelectParticleType<Sand>(),
                Text = "Sand",
                Width = ButtonWidth,
                Height = ButtonHeight,
                X = ControlHorizontalMargin + ControlHorizontalMargin * buttonCount * 2 + ButtonWidth * buttonCount,
                Y = gameControl.Y + gameControl.Height + ButtonVerticalMargin
            });
            // - Water
            buttonCount++;
            gameScene.Controls.Add(new ButtonControl(new PixelColor(ConsoleColor.Blue), new PixelColor(ConsoleColor.DarkBlue), pressedColor)
            {
                OnClick = () => gameControl.SelectParticleType<Water>(),
                Text = "Water",
                Width = ButtonWidth,
                Height = ButtonHeight,
                X = ControlHorizontalMargin + ControlHorizontalMargin * buttonCount * 2 + ButtonWidth * buttonCount,
                Y = gameControl.Y + gameControl.Height + ButtonVerticalMargin
            });
            // - Block
            buttonCount++;
            gameScene.Controls.Add(new ButtonControl(new PixelColor(ConsoleColor.Cyan), new PixelColor(ConsoleColor.DarkCyan), pressedColor)
            {
                OnClick = () => gameControl.SelectParticleType<Block>(),
                Text = "Block",
                Width = ButtonWidth,
                Height = ButtonHeight,
                X = ControlHorizontalMargin + ControlHorizontalMargin * buttonCount * 2 + ButtonWidth * buttonCount,
                Y = gameControl.Y + gameControl.Height + ButtonVerticalMargin
            });
            // - Acid
            buttonCount++;
            gameScene.Controls.Add(new ButtonControl(new PixelColor(ConsoleColor.Green), new PixelColor(ConsoleColor.DarkGreen), pressedColor)
            {
                OnClick = () => gameControl.SelectParticleType<Acid>(),
                Text = "Acid",
                Width = ButtonWidth,
                Height = ButtonHeight,
                X = ControlHorizontalMargin + ControlHorizontalMargin * buttonCount * 2 + ButtonWidth * buttonCount,
                Y = gameControl.Y + gameControl.Height + ButtonVerticalMargin
            });
            // - Virus
            buttonCount++;
            gameScene.Controls.Add(new ButtonControl(new PixelColor(ConsoleColor.Magenta), new PixelColor(ConsoleColor.DarkMagenta), pressedColor)
            {
                OnClick = () => gameControl.SelectParticleType<Virus>(),
                Text = "Virus",
                Width = ButtonWidth,
                Height = ButtonHeight,
                X = ControlHorizontalMargin + ControlHorizontalMargin * buttonCount * 2 + ButtonWidth * buttonCount,
                Y = gameControl.Y + gameControl.Height + ButtonVerticalMargin
            });
            buttonCount++;
            gameScene.Controls.Add(new ButtonControl(new PixelColor(ConsoleColor.Red), new PixelColor(ConsoleColor.Yellow), pressedColor)
            {
                OnClick = () => 
                {
                    Physics.ResetPhysics();
                    gameControl.InstantiateBorders();
                },
                Text = "Reset",
                Width = ButtonWidth,
                Height = ButtonHeight,
                X = ControlHorizontalMargin + ControlHorizontalMargin * buttonCount * 2 + ButtonWidth * buttonCount,
                Y = gameControl.Y + gameControl.Height + ButtonVerticalMargin
            });

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
            UIManager.Setup(90, 45);
            // Call console input library.
            Input.Setup(false);

            SetupScenes();

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

                // Udregn antallet af partikler.
                int particleCount = Physics.ParticleGroups.Sum(p => p.Particles.Count);

                // Sat før timeout for at måle den potentielle FPS.
                Console.Title = $"Particles: {particleCount}, FPS: {Math.Floor(1 / stopwatch.Elapsed.TotalSeconds)}";

                // Når man sætter Thread.Sleep's parameter til 0 sætter den tråden i en kø og så vender den tilbage når den "har tid".
                // Det vil give en mere nøjagtig tid end hvis man bare indtastede det antal milisekunder man ville vente, 
                // så ville det være: ventetid + abitrer timeout før der vendes tilbage til tråden.
                while (stopwatch.ElapsedMilliseconds < 1000 / FramesPerSecondCap)
                    Thread.Sleep(0);
            }
        }
    }
}
