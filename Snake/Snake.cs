using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class Snake
    {
        private Food _food;
        private Poison _poison;
        private ConsoleKey _input;
        private List<Position> _snakeBody;
        private char _direction;
        private int _speed = 100;
        private int _acceleratorSpeed;
        private int _x;
        private int _y;
        private const char _leftDirection = 'l', _rightDirection = 'r', _upDirection = 'u', _downDirection = 'd';
        private const int _classicFood = 0, _acceleratorFood = 1, _specialFood = 2;
        private bool _isAlive = false;
        private bool _isEatedAccelerator = false;


        public ConsoleKey Input { set => _input = value; }
        public Food SearchingFood
        {
            get => _food;

            set
            {
                if (_food == null)
                {
                    _food = value;
                }
            }
        }
        public Poison DangerousFood
        {
            get => _poison;

            set
            {
                if (_poison == null)
                {
                    _poison = value;
                }
            }
        }
        public bool IsAlive { get => _isAlive; }
        public List<Position> SnakeBody { get => _snakeBody; private set => _snakeBody = value; }
        public int Speed { get => _speed; private set => _speed = value; }

        public Snake(int speed)
        {
            _isAlive = true;
            _speed = speed;
            _x = Borders.Width / 2;
            _y = Borders.Height / 2;
            _snakeBody = new();
            _snakeBody.Add(new Position(_x, _y));
        }

        private void GetDirection()
        {
            switch (_input)
            {
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    {
                        if (_direction != _leftDirection)
                        {
                            _direction = _rightDirection;
                        }

                        break;
                    }
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    {
                        if (_direction != _rightDirection)
                        {
                            _direction = _leftDirection;
                        }

                        break;
                    }
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    {
                        if (_direction != _downDirection)
                        {
                            _direction = _upDirection;
                        }

                        break;
                    }
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    {
                        if (_direction != _upDirection)
                        {
                            _direction = _downDirection;
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

            if (_direction == _leftDirection)
            {
                _x--;
            }
            else if (_direction == _rightDirection)
            {
                _x++;
            }
            else if (_direction == _upDirection)
            {
                _y--;
            }
            else if (_direction == _downDirection)
            {
                _y++;
            }

            _snakeBody.Add(new Position(_x, _y));

            if (_isEatedAccelerator)
            {
                Thread.Sleep(_acceleratorSpeed);
            }
            else
            {
                Thread.Sleep(_speed);
            }
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
            Position headSnake = new Position(_snakeBody[^1].X, _snakeBody[^1].Y);
            Position foodConcrete = _food.Position;

            if (headSnake == _food.Position && _food.CurrentType == _classicFood)
            {
                Score.CurrentScore++;
            }
            else if (headSnake == _food.Position && _food.CurrentType == _specialFood)
            {
                Score.CurrentScore += 5;
            }
            else if (headSnake == _food.Position && _food.CurrentType == _acceleratorFood)
            {
                Score.CurrentScore += 2;
                Thread speedReset = new(ResetSpeed);
                speedReset.Start();
            }
            else if (headSnake == _poison.Position)
            {
                _isAlive = false;
                _poison.StopThread();
            }
            else
            {
                return;
            }

            _snakeBody.Add(new Position(_snakeBody[^1].X, _snakeBody[^1].Y));
            _food = null;
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
                        _isAlive = false;
                        ClearSnakeBody();
                    }

                    counter++;
                }
            }
        }

        public void ResetSpeed()
        {
            _acceleratorSpeed = _speed / 2;
            _isEatedAccelerator = true;

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
            }

            _isEatedAccelerator = false;
        }

        private void ClearSnakeBody()
        {
            _snakeBody.Clear();
        }
    }
}
