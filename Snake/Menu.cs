using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Snake
{
    class Menu
    {
        GameConfiguration _gameConfigurator;
        private static int _height = 30;
        private static int _width = 50;
        private int _globalPosition;
        private int _localPosition;
        private readonly string _name;
        private int _currentSnakeSpeed;
        

        private static Dictionary<bool, string> _status = new Dictionary<bool, string>();
        private List<Menu> _currentMenu = new();
        private List<Menu> _start = new();
        private List<Menu> _settings = new();
        private List<Menu> _speedSettings = new();
        private List<Menu> _foodTypeSettings = new();
        private List<Menu> _wallSettings = new();
        private List<Menu> _soundSettings = new();

        public GameConfiguration GameConfigurator { get=>_gameConfigurator; init=>_gameConfigurator=value; }
        public List<Menu> Start { get => _start; private set => _start = value; }
        public List<Menu> Settings { get => _settings; private set => _settings = value; }
        public List<Menu> SpeedSettings { get => _speedSettings; private set => _speedSettings = value; }
        public List<Menu> FoodTypeSettings { get => _foodTypeSettings; private set => _foodTypeSettings = value; }
        public List<Menu> WallSetings { get => _wallSettings; private set => _wallSettings = value; }
        public List<Menu> SoundSettings { get => _soundSettings; private set => _soundSettings = value; }
        public List<Menu> CurrentMenu { get => _currentMenu; set => _currentMenu = value; }
        public int GlobalPosition { get => _globalPosition; set => _globalPosition = value; }
        public int LocalPosition { get => _localPosition; set => _localPosition = value; }
        public int CurrentSnakeSpeed { get => _currentSnakeSpeed; set => _currentSnakeSpeed = value; }

        public Menu(GameConfiguration gameConfigurator)
        {
            Console.CursorVisible = false;
            Debug.Assert(OperatingSystem.IsWindows());
            Console.Title = "Snake";
            Console.SetWindowSize(_width, _height);
            Console.SetBufferSize(_width + 1, _height + 1);
            GameConfigurator = gameConfigurator;

            _globalPosition = 0;
            _localPosition = 0;
            _currentMenu = _start;
            _currentSnakeSpeed = _gameConfigurator.Speed;
            _status.Add(false, "OFF");
            _status.Add(true, "ON");

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

        private void SetStartMenu()
        {
            _start.Add(new Menu("Start game"));
            _start.Add(new Menu("Settings"));
            _start.Add(new Menu("Exit"));
        }

        private void SetSettingsMenu()
        {
            _settings.Add(new Menu("Speed"));
            _settings.Add(new Menu("Types of food"));
            _settings.Add(new Menu("Walls settings"));
            _settings.Add(new Menu("Sound"));
            _settings.Add(new Menu("Back "));
        }

        private void SetSpeedSettings()
        {
            _speedSettings.Add(new Menu("50 speed"));
            _speedSettings.Add(new Menu("100 speed"));
            _speedSettings.Add(new Menu("250 speed"));
            _speedSettings.Add(new Menu("400 speed"));
            _speedSettings.Add(new Menu("Back "));
        }

        private void SetFoodTypeSettings()
        {
            _foodTypeSettings.Add(new Menu("Special food"));
            _foodTypeSettings.Add(new Menu("Poison"));
            _foodTypeSettings.Add(new Menu("Accelerator"));
            _foodTypeSettings.Add(new Menu("Back "));
        }

        private void SetWallSettings()
        {
            _wallSettings.Add(new Menu("Empty walls"));
            _wallSettings.Add(new Menu("Back "));
        }

        private void SetSoundSettings()
        {
            _soundSettings.Add(new Menu("Game Sound"));
            _soundSettings.Add(new Menu("Back "));
        }

        public void ShowCurrentMenu()
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

        private void ShowGameInformation()
        {
            ConsoleColor gameInformationTextColor = ConsoleColor.Yellow;
            int counter = 1;
            Console.SetCursorPosition((1), _height / 2);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = gameInformationTextColor;

            Console.Write("Current game settings");
            Console.SetCursorPosition((1), _height / 2 + counter++);
            Console.Write($"Speed: {10000 / CurrentSnakeSpeed }  ");

            string wallMode, standardFoodStatus, specialFoodStatus, poisonFoodStatus, acceleratorFoodStatus, soundStatus;
            {
                _status.TryGetValue(Borders.EmptyWalls, out wallMode);
                _status.TryGetValue(true, out standardFoodStatus);
                _status.TryGetValue(_gameConfigurator.IsSpecialFoodActive, out specialFoodStatus);
                _status.TryGetValue(_gameConfigurator.IsPoisonActive, out poisonFoodStatus);
                _status.TryGetValue(_gameConfigurator.IsAcceleratorFoodActive, out acceleratorFoodStatus);
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

        public void DrawBackgroundMenu()
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

