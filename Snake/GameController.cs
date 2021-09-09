using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    class GameController
    {
        private Snake _snake;
        private Food _food;
        private Menu _menu;
        private GameConfiguration _gameConfigurator = new GameConfiguration();

        private ConsoleKeyInfo _key = new ConsoleKeyInfo();
        private ConsoleKey _input;

        public GameController()
        {
            Thread inputing = new(Inputing);
            inputing.Start();

            Borders borders = new();

            _menu = new(_gameConfigurator);
            Thread controller = new(ControllerLocalPosition);
            controller.Start();
        }

        private void Play()
        {
            Console.Clear();
            Borders.DrawBorders();
            Score score = new();

            _snake = new Snake(_gameConfigurator.Speed);
            GenerateFood();



            //if (!Poison.IsThreadGeneratorActive)
            //{
            //    Poison.Generator();
            //}

            while (_snake.IsAlive)
            {
                _snake.Move();
                _snake.Eat();

                if (_snake.SearchingFood == null)
                {
                    GenerateFood();
                }

                _snake.PrintSnake();
                _snake.OnCollisionEnter();
            }

            _snake = null;
        }

        private void GenerateFood()
        {
            _food = Food.GetCurrentType(_gameConfigurator.IsSpecialFoodActive, _gameConfigurator.IsAcceleratorFoodActive);
            _snake.SearchingFood = _food;;
            _food.Instantiate(_snake.SnakeBody);
        }

        private void Inputing()
        {
            while (true)
            {
                _key = Console.ReadKey(true);
                _input = _key.Key;

                if (_snake!=null)
                {
                    _snake.Input = _input;
                }
            }
        }

        private void ControllerLocalPosition()
        {
            while (true)
            {
                if (_snake!=null)
                {
                    continue;
                }

                switch (_input)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        {
                            _menu.LocalPosition--;

                            if (_menu.LocalPosition < 0)
                            {
                                _menu.LocalPosition = _menu.CurrentMenu.Count - 1;
                            }
                            else if (_menu.LocalPosition < 0)
                            {
                                _menu.LocalPosition = 0;
                            }

                            _input = default;
                            _menu.ShowCurrentMenu();

                            break;
                        }
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        {
                            _menu.LocalPosition++;

                            if (_menu.LocalPosition > _menu.CurrentMenu.Count - 1)
                            {
                                _menu.LocalPosition = 0;
                            }

                            _input = default;
                            _menu.ShowCurrentMenu();

                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            _input = default;
                            MainController();

                            break;
                        }
                }
            }
        }

        private void MainController()
        {
            switch (_menu.GlobalPosition)
            {
                case (int)MenuPosition.Start:
                    {
                        if (_menu.LocalPosition == 0)
                        {
                            Play();
                            Console.SetCursorPosition(0, 0);
                            _menu.GlobalPosition = (int)MenuPosition.Start;
                            _menu.LocalPosition = (int)MenuPosition.Zero;
                            _menu.DrawBackgroundMenu();

                        }
                        else if (_menu.LocalPosition == 1)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.Settings;
                            _menu.LocalPosition = (int)MenuPosition.Zero;
                            _menu.CurrentMenu = _menu.Settings;
                        }
                        else if (_menu.LocalPosition == 2)
                        {
                            System.Environment.Exit(1);
                        }

                        _menu.ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.Settings:
                    {
                        if (_menu.LocalPosition == (int)MenuPosition.Zero)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.SpeedSettings;
                            _menu.CurrentMenu = _menu.SpeedSettings;
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.First)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.FoodTypeSettings;
                            _menu.CurrentMenu = _menu.FoodTypeSettings;
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.Second)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.WallSettings;
                            _menu.CurrentMenu = _menu.WallSetings;
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.Third)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.SoundsSettings;
                            _menu.CurrentMenu = _menu.SoundSettings;
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.Fourth)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.Start;
                            _menu.CurrentMenu = _menu.Start;
                        }

                        _menu.LocalPosition = (int)MenuPosition.Zero;
                        _menu.ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.SpeedSettings:
                    {
                        if (_menu.LocalPosition == (int)MenuPosition.Zero)
                        {
                            _gameConfigurator.Speed = _gameConfigurator.LowSpeed;
                            _menu.CurrentSnakeSpeed = _gameConfigurator.LowSpeed;
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.First)
                        {
                            _gameConfigurator.Speed = _gameConfigurator.MediumSpeed;
                            _menu.CurrentSnakeSpeed = _gameConfigurator.MediumSpeed;
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.Second)
                        {
                            _gameConfigurator.Speed = _gameConfigurator.HighSpeed;
                            _menu.CurrentSnakeSpeed = _gameConfigurator.HighSpeed;
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.Third)
                        {
                            _gameConfigurator.Speed = _gameConfigurator.SuperSpeed;
                            _menu.CurrentSnakeSpeed = _gameConfigurator.SuperSpeed;
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.Fourth)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.Settings;
                            _menu.LocalPosition = 0;
                            _menu.CurrentMenu = _menu.Settings;
                        }

                        _menu.ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.FoodTypeSettings:
                    {
                        if (_menu.LocalPosition == (int)MenuPosition.Zero)
                        {
                            if (!_gameConfigurator.IsSpecialFoodActive)
                            {
                                _gameConfigurator.IsSpecialFoodActive = true;
                            }
                            else
                            {
                                _gameConfigurator.IsSpecialFoodActive = false;
                            }
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.First)
                        {
                            if (!_gameConfigurator.IsPoisonActive)
                            {
                                _gameConfigurator.IsPoisonActive = true;
                            }
                            else
                            {
                                _gameConfigurator.IsPoisonActive = false;
                            }
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.Second)
                        {
                            if (!_gameConfigurator.IsAcceleratorFoodActive)
                            {
                                _gameConfigurator.IsAcceleratorFoodActive = true;
                            }
                            else
                            {
                                _gameConfigurator.IsAcceleratorFoodActive = false;
                            }
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.Third)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.Settings;
                            _menu.LocalPosition = 0;
                            _menu.CurrentMenu = _menu.Settings;
                        }

                        _menu.ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.WallSettings:
                    {
                        if (_menu.LocalPosition == (int)MenuPosition.Zero)
                        {
                            if (!Borders.EmptyWalls)
                            {
                                Borders.EmptyWalls = true;
                            }
                            else
                            {
                                Borders.EmptyWalls = false;
                            }
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.First)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.Settings;
                            _menu.LocalPosition = 0;
                            _menu.CurrentMenu = _menu.Settings;
                        }

                        _menu.ShowCurrentMenu();
                        break;
                    }
                case (int)MenuPosition.SoundsSettings:
                    {
                        if (_menu.LocalPosition == (int)MenuPosition.Zero)
                        {
                            // sounds on off 
                        }
                        else if (_menu.LocalPosition == (int)MenuPosition.First)
                        {
                            _menu.GlobalPosition = (int)MenuPosition.Settings;
                            _menu.LocalPosition = 0;
                            _menu.CurrentMenu = _menu.Settings;
                        }

                        _menu.ShowCurrentMenu();
                        break;
                    }
            }
        }
    }
}
