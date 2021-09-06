using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Snake
{
    class Menu
    {
        private static int _height = 30;
        private static int _width = 50;
        private static int _globalPosition;
        private static int _localPosition;
        private readonly string _name;
        private const int ON = 1, OFF = 0;

        private static Dictionary<bool, string> _status = new Dictionary<bool, string>();
        private static List<Menu> _currentMenu = new();
        private static List<Menu> _start = new();
        private static List<Menu> _settings = new();
        private static List<Menu> _speedSettings = new();
        private static List<Menu> _foodTypeSettings = new();
        private static List<Menu> _wallSettings = new();
        private static List<Menu> _soundSettings = new();

        public static List<Menu> Start { get => _start; private set => _start = value; }
        public static List<Menu> Settings { get => _settings; private set => _settings = value; }
        public static List<Menu> SpeedSnake { get => _speedSettings; private set => _speedSettings = value; }
        public static List<Menu> FoodType { get => _foodTypeSettings; private set => _foodTypeSettings = value; }
        public static List<Menu> WallSetings { get => _wallSettings; private set => _wallSettings = value; }
        public static List<Menu> Sound { get => _soundSettings; private set => _soundSettings = value; }
        public static int GlobalCounter { get => _globalPosition; set => _globalPosition = value; }
        public static int LocalCounter { get => _localPosition; set => _localPosition = value; }

        public Menu()
        {
            Console.CursorVisible = false;
            Debug.Assert(OperatingSystem.IsWindows());
            Console.Title = "Snake";
            Console.SetWindowSize(_width, _height);
            Console.SetBufferSize(_width + 1, _height + 1);

            _globalPosition = 0;
            _localPosition = 0;
            _currentMenu = _start;
            _status.Add(false, "OFF");
            _status.Add(true, "ON");

            Thread controller = new(ControllerPosition);
            controller.Start();

            SetStartMenu();
            SetSettingsMenu();
            SetSpeedSettings();
            SetFoodTypeSettings();
            SetWallSettings();
            SetSoundSettings();
            DrawBackgroundMenu();
            ShowCurrentMenu();
        }

        public Menu(string name, Dictionary<bool, string> status = null)
        {
            _name = name;
        }

        private static void SetStartMenu()
        {
            _start.Add(new Menu("Start game"));
            _start.Add(new Menu("Settings"));
            _start.Add(new Menu("Exit"));
        }

        private static void SetSettingsMenu()
        {
            _settings.Add(new Menu("Speed"));
            _settings.Add(new Menu("Types of food"));
            _settings.Add(new Menu("Walls settings"));
            _settings.Add(new Menu("Sound"));
            _settings.Add(new Menu("Back "));
        }

        private static void SetSpeedSettings()
        {
            _speedSettings.Add(new Menu("50 speed"));
            _speedSettings.Add(new Menu("100 speed"));
            _speedSettings.Add(new Menu("250 speed"));
            _speedSettings.Add(new Menu("400 speed"));
            _speedSettings.Add(new Menu("Back "));
        }

        private static void SetFoodTypeSettings()
        {
            _foodTypeSettings.Add(new Menu("Special food"));
            _foodTypeSettings.Add(new Menu("Poison"));
            _foodTypeSettings.Add(new Menu("Accelerator"));
            _foodTypeSettings.Add(new Menu("Back "));
        }

        private static void SetWallSettings()
        {
            _wallSettings.Add(new Menu("Empty walls"));
            _wallSettings.Add(new Menu("Back "));
        }

        private static void SetSoundSettings()
        {
            _soundSettings.Add(new Menu("Game Sound"));
            _soundSettings.Add(new Menu("Back "));
        }

        public static void ControllerPosition()
        {
            while (true)
            {
                if (Snake.IsAlive)
                {
                    continue;
                }

                switch (Input.InputKey)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        {
                            _localPosition--;

                            if (_localPosition < 0 && !Snake.IsAlive)
                            {
                                _localPosition = _currentMenu.Count - 1;
                            }
                            else if(_localPosition < 0 && Snake.IsAlive)
                            {
                                _localPosition = 0;
                            }

                            Input.InputKey = default;
                            ShowCurrentMenu();

                            break;
                        }
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        {
                            _localPosition++;

                            if (_localPosition > _currentMenu.Count - 1)
                            {
                                _localPosition = 0;
                            }

                            Input.InputKey = default;
                            ShowCurrentMenu();

                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            Input.InputKey = default;
                            MainController();

                            break;
                        }
                }
            }
        }

        private static void MainController()
        {
            switch (_globalPosition)
            {
                case (int)MenuPosition.Start:
                    {
                        if (_localPosition == 0)
                        {
                            Program.Play();
                            Console.SetCursorPosition(0, 0);
                            _globalPosition = (int)MenuPosition.Start;
                            _localPosition = (int)MenuPosition.Zero;
                            DrawBackgroundMenu();
                            
                        }
                        else if (_localPosition == 1)
                        {
                            _globalPosition = (int)MenuPosition.Settings;
                            _localPosition = (int)MenuPosition.Zero;
                            _currentMenu = _settings;
                        }
                        else if (_localPosition == 2)
                        {
                            System.Environment.Exit(1);
                        }

                        ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.Settings:
                    {
                        if (_localPosition == (int)MenuPosition.Zero)
                        {
                            _globalPosition = (int)MenuPosition.SpeedSettings;
                            _currentMenu = _speedSettings;
                        }
                        else if (_localPosition == (int)MenuPosition.First)
                        {
                            _globalPosition = (int)MenuPosition.FoodTypeSettings;
                            _currentMenu = _foodTypeSettings;
                        }
                        else if (_localPosition == (int)MenuPosition.Second)
                        {
                            _globalPosition = (int)MenuPosition.WallSettings;
                            _currentMenu = _wallSettings;
                        }
                        else if (_localPosition == (int)MenuPosition.Third)
                        {
                            _globalPosition = (int)MenuPosition.SoundsSettings;
                            _currentMenu = _soundSettings;
                        }
                        else if (_localPosition == (int)MenuPosition.Fourth)
                        {
                            _globalPosition = (int)MenuPosition.Start;
                            _currentMenu = _start;
                        }

                        _localPosition = (int)MenuPosition.Zero;
                        ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.SpeedSettings:
                    {
                        if (_localPosition == (int)MenuPosition.Zero)
                        {
                            Snake.Speed = 200;
                            // set speed 100
                        }
                        else if (_localPosition == (int)MenuPosition.First)
                        {
                            Snake.Speed = 100;
                            // set speed 50
                        }
                        else if (_localPosition == (int)MenuPosition.Second)
                        {
                            Snake.Speed = 50;
                            // set speed 25
                        }
                        else if (_localPosition == (int)MenuPosition.Third)
                        {
                            Snake.Speed = 25;
                            // set speed 10
                        }
                        else if (_localPosition == (int)MenuPosition.Fourth)
                        {
                            _globalPosition = (int)MenuPosition.Settings;
                            _localPosition = 0;
                            _currentMenu = _settings;
                            // back
                        }

                        ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.FoodTypeSettings:
                    {
                        if (_localPosition == (int)MenuPosition.Zero)
                        {
                            if (!Special.IsActive)
                            {
                                Special.IsActive = true;
                            }
                            else
                            {
                                Special.IsActive = false;
                            }
                        }
                        else if (_localPosition == (int)MenuPosition.First)
                        {
                            if (!Poison.IsActive)
                            {
                                Poison.IsActive = true;
                            }
                            else
                            {
                                Poison.IsActive = false;
                            }
                        }
                        else if (_localPosition == (int)MenuPosition.Second)
                        {
                            if (!Accelerator.IsActive)
                            {
                                Accelerator.IsActive = true;
                            }
                            else
                            {
                                Accelerator.IsActive = false;
                            }
                        }
                        else if (_localPosition == (int)MenuPosition.Third)
                        {
                            _globalPosition = (int)MenuPosition.Settings;
                            _localPosition = 0;
                            _currentMenu = _settings;
                        }

                        ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.WallSettings:
                    {
                        if (_localPosition == (int)MenuPosition.Zero)
                        {
                            if (!Borders.EmptyWalls)
                            {
                                Borders.EmptyWalls = true;
                            }
                            else
                            {
                                Borders.EmptyWalls = false;
                            }
                            // set wall empty mode on off
                        }
                        else if (_localPosition == (int)MenuPosition.First)
                        {
                            _globalPosition = (int)MenuPosition.Settings;
                            _localPosition = 0;
                            _currentMenu = _settings;
                            // back
                        }

                        ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.SoundsSettings:
                    {
                        if (_localPosition == (int)MenuPosition.Zero)
                        {
                            // sounds on off 
                        }
                        else if (_localPosition == (int)MenuPosition.First)
                        {
                            _globalPosition = (int)MenuPosition.Settings;
                            _localPosition = 0;
                            _currentMenu = _settings;
                            // back
                        }

                        ShowCurrentMenu();
                        break;
                    }
            }
        }
        public static void ShowCurrentMenu()
        {
            int counter = 0;

            Clear();

            foreach (Menu item in _currentMenu)
            {
                Console.SetCursorPosition((_width / 2) - item._name.Length / 2, _height / 2 + counter - 7);

                if (counter == _localPosition)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                Console.WriteLine(item._name);
                Console.ResetColor();

                ShowGameInformation();

                counter++;
            }
        }
        private static void ShowGameInformation()
        {
            ConsoleColor gameInformationTextColor = ConsoleColor.Yellow;
            int counter = 1;
            Console.SetCursorPosition((1), _height / 2);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = gameInformationTextColor;

            Console.Write("Current game settings");
            Console.SetCursorPosition((1), _height / 2 + counter++);
            Console.Write($"Speed: {10000 / Snake.Speed}");

            string wallMode, standardFoodStatus, specialFoodStatus, poisonFoodStatus, acceleratorFoodStatus, soundStatus;
            {
                _status.TryGetValue(Borders.EmptyWalls, out wallMode);
                _status.TryGetValue(Classic.IsActive, out standardFoodStatus);
                _status.TryGetValue(Special.IsActive, out specialFoodStatus);
                _status.TryGetValue(Poison.IsActive, out poisonFoodStatus);
                _status.TryGetValue(Accelerator.IsActive, out acceleratorFoodStatus);
                _status.TryGetValue(Music.IsOn, out soundStatus);
            }

            Console.SetCursorPosition((1), _height / 2 + counter++);
            Console.Write($"Empty walls:  {wallMode} ");
            Console.SetCursorPosition((1), _height / 2 + counter++);
            {
                Console.Write($"Standart food: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("♥ ");
                Console.ForegroundColor = gameInformationTextColor;
                Console.Write(standardFoodStatus + " ");
            }
            Console.SetCursorPosition((1), _height / 2 + counter++);
            {
                Console.Write($"Special food: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("▲ ");
                Console.ForegroundColor = gameInformationTextColor;
                Console.Write(specialFoodStatus + " ");
            }
            Console.SetCursorPosition((1), _height / 2 + counter++);
            {
                Console.Write($"Poison: ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("† ");
                Console.ForegroundColor = gameInformationTextColor;
                Console.Write(poisonFoodStatus + " ");
            }
            Console.SetCursorPosition((1), _height / 2 + counter++);
            {
                Console.Write($"Accelerator: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("► ");
                Console.ForegroundColor = gameInformationTextColor;
                Console.Write(acceleratorFoodStatus + " ");
            }
            Console.SetCursorPosition((1), _height / 2 + counter++);
            Console.Write($"Sound: {soundStatus} ");

            Console.SetCursorPosition((int)MenuPosition.Zero, (int)MenuPosition.Zero);
        }

        public static void DrawBackgroundMenu()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if (i == 0 || i == _height - 1)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write(" ");
                    }
                    else
                    {
                        if (j == 0 || j == _width - 1)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(" ");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.Write(" ");
                        }
                    }

                    Console.ResetColor();
                }

                Console.WriteLine(" ");
            }
        }

        private static void Clear()
        {
            for (int i = 0; i < 9; i++)
            {
                Console.SetCursorPosition(15, 6 + i);

                for (int j = 0; j < 17; j++)
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" ");
                }
                Console.WriteLine();

            }

            Console.ResetColor();
        }
    }
}

