using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class Snake
    {
        private static Snake _snake;
        private char _direction;
        private static int _speed = 100;
        private int _x;
        private int _y;
        private static bool _isAlive = false;
        private static List<Position> _snakeBody;
        public static bool IsAlive { get => _isAlive; set => _isAlive = value; }
        public static List<Position> SnakeBody { get => _snakeBody; private set => _snakeBody = value; }
        public static int Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                GameConfiguration.Speed = _speed;
            }

        }
        private Snake()
        {
            _isAlive = true;
            _x = Borders.Width / 2;
            _y = Borders.Height / 2;
            _snakeBody = new();
            _snakeBody.Add(new Position(_x, _y));
        }
        public static Snake GetInstance()
        {
            if (_snake == null)
            {
                _isAlive = true;
                _snake = new();
            }

            return _snake;
        }
        public static void DeleteInstance()
        {
            if (_snake != null)
            {
                _snake = null;
            }
        }
        private void GetDirection()
        {
            switch (Input.InputKey)
            {
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    {
                        if (_direction != Input.LeftDirection)
                        {
                            _direction = Input.RightDirection;
                        }

                        break;
                    }
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    {
                        if (_direction != Input.RightDirection)
                        {
                            _direction = Input.LeftDirection;
                        }

                        break;
                    }
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    {
                        if (_direction != Input.DownDirection)
                        {
                            _direction = Input.UpDirection;
                        }

                        break;
                    }
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    {
                        if (_direction != Input.UpDirection)
                        {
                            _direction = Input.DownDirection;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        public void Move()
        {
            GetDirection();

            if (_direction == Input.LeftDirection)
            {
                _x--;
            }
            else if (_direction == Input.RightDirection)
            {
                _x++;
            }
            else if (_direction == Input.UpDirection)
            {
                _y--;
            }
            else if (_direction == Input.DownDirection)
            {
                _y++;
            }

            _snakeBody.Add(new Position(_x, _y));

            Thread.Sleep(_speed);
        }
        public void PrintSnake()
        {
            char body;

            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;

            foreach (Position item in _snakeBody)
            {
                if (item == _snakeBody[^1])
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    body = '♦';
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    body = '♦';
                }

                Console.SetCursorPosition(item.X, item.Y);
                Console.Write(body);
            }

            Console.SetCursorPosition(_snakeBody[0].X, _snakeBody[0].Y);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(" ");

            _snakeBody.RemoveAt(0);

            Console.ResetColor();
        }
        public void Eat()
        {
            int xPosition = _snakeBody[^1].X, yPosition = _snakeBody[^1].Y;

            if (xPosition == Food.X && yPosition == Food.Y && Classic.CheckInstance())
            {
                Score.CurrentScore++;
            }
            else if (xPosition == Food.X && yPosition == Food.Y && Special.CheckInstance())
            {
                Score.CurrentScore += 5;
            }
            else if (xPosition == Food.X && yPosition == Food.Y && Accelerator.CheckInstance())
            {
                Thread speedReset = new(Accelerator.ResetSpeed);
                speedReset.Start();
                _speed = _speed / 2;
                Score.CurrentScore++;
            }
            else if(xPosition== Poison.X && yPosition == Poison.Y)
            {
                _isAlive = false;
            }
            else
            {
                return;
            }

            _snakeBody.Add(new Position(_snakeBody[^1].X, _snakeBody[^1].Y));
            Food.Destroy();
            Food.SetCurrentType();
        }
        public void OnCollisionEnter()
        {
            OnCollisionWithBorder();
            OnCollisionWithOneself();
        }
        private void OnCollisionWithBorder()
        {
            int xHeadSnakePosition = _snakeBody[^1].X, yHeadSnakePosition = _snakeBody[^1].Y;

            if (!Borders.EmptyWalls)
            {
                if (xHeadSnakePosition <= Borders.LeftLimit || xHeadSnakePosition >= Borders.Width - 1 || yHeadSnakePosition <= Borders.TopLimit
                    || yHeadSnakePosition >= Borders.Height - 1)
                {
                    _isAlive = false;

                    Console.SetCursorPosition(xHeadSnakePosition, yHeadSnakePosition);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(" ");

                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(Borders.Width / 2, Borders.Height / 2);
                    Console.WriteLine("YOUR LOSE");
                    Thread.Sleep(5000);
                }
            }
            else
            {

            }
        }
        private void OnCollisionWithOneself()
        {
            int counter = 0;
            bool isEnoughLargeSnake = _snakeBody.Count > 4;
            int xHeadSnakePosition = _snakeBody[^1].X, yHeadSnakePosition = _snakeBody[^1].Y;

            if (isEnoughLargeSnake)
            {
                List<Position> snakeBodyNoHead = new(_snakeBody);

                snakeBodyNoHead.RemoveAt(snakeBodyNoHead.Count - 1);
                snakeBodyNoHead.RemoveAt(snakeBodyNoHead.Count - 1);

                foreach (Position item in snakeBodyNoHead)
                {
                    if (xHeadSnakePosition == item.X && yHeadSnakePosition == item.Y)
                    {
                        Console.SetCursorPosition(Borders.Width / 2 - 5, Borders.Height / 2);
                        Console.WriteLine("GG");
                        Thread.Sleep(500);
                        Snake.IsAlive = false;
                        ClearSnakeBody();
                    }

                    counter++;
                }
            }
        }
        private void ClearSnakeBody()
        {
            _snakeBody.Clear();
        }
    }
}
