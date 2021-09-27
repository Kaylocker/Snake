using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class Snake
    {
        public event Action<int> OnEat;
        public event Action OnDied;

        private Food _food;
        private Poison _poison;
        private Borders _borders;
        private ConsoleKey _input;
        private List<Position> _snakeBody;
        private char _direction;
        private char _body = '♦';
        private int _speed = 100, _acceleratorSpeed;
        private int _x, _y;
        private const char _leftDirection = 'l', _rightDirection = 'r', _upDirection = 'u', _downDirection = 'd';
        private const int _accelerator = 1;
        private bool _isAlive = false, _isEatedAccelerator = false, _isWallsEmpty, _isGoneThroughWall = false;
        private bool _isPaused = false;

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
        public Poison DangerousFood { get => _poison; set => _poison = value; }
        public Borders Borders { get => _borders; }
        public bool IsAlive { get => _isAlive; set => _isAlive = value; }
        public bool IsPaused { get => _isPaused; set => _isPaused = value; }
        public List<Position> SnakeBody { get => _snakeBody; private set => _snakeBody = value; }
        public int Speed { get => _speed; private set => _speed = value; }
        public Snake(GameConfiguration gameconfigurator, Borders borders, Score score)
        {
            _isAlive = true;
            _speed = gameconfigurator.Speed;
            _isWallsEmpty = gameconfigurator.IsWallsEmpty;
            _borders = borders;
            _x = _borders.Width / 2;
            _y = _borders.Height / 2;
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
                case ConsoleKey.P:
                    {
                        _isPaused = true;

                        break;
                    }
                case ConsoleKey.Escape:
                    {
                        _isAlive = false;
                        _poison = null;

                        break;
                    }
            }
        }

        public void Move()
        {
            GetDirection();

            if (!_isGoneThroughWall)
            {
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
            }
            else
            {
                _isGoneThroughWall = false;
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

            OnCollisionEnter();
        }

        public void PrintBody()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;

            foreach (Position item in _snakeBody)
            {
                if (item == _snakeBody[^1])
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.SetCursorPosition(item.X, item.Y);
                Console.Write(_body);
            }

            if (_snakeBody[0].X == 0 || _snakeBody[0].X == _borders.Width - 1 || _snakeBody[0].Y == 0 || _snakeBody[0].Y == _borders.Height - 1)
            {
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
            }

            Console.SetCursorPosition(_snakeBody[0].X, _snakeBody[0].Y);
            Console.Write(" ");

            _snakeBody.RemoveAt(0);

            Console.ResetColor();
        }

        public void Eat()
        {
            Position headSnake = new Position(_snakeBody[^1].X, _snakeBody[^1].Y);

            if (headSnake == _food.Position)
            {
                if(_food.CurrentType == _accelerator)
                {
                    Thread speedReset = new(SpeetIncrease);
                    speedReset.Start();
                }

                OnEat?.Invoke(_food.Score);

                _snakeBody.Add(new Position(_snakeBody[^1].X, _snakeBody[^1].Y));
                _food = null;
            }
            else if (_poison != null)
            {
                foreach (Position item in _poison.Position)
                {
                    if (item == headSnake)
                    {
                        OnDied?.Invoke();
                        _isAlive = false;
                        return;
                    }
                }
            }
        }

        public void OnCollisionEnter()
        {
            OnCollisionWithBorder();
            OnCollisionWithOneself();
        }

        private void OnCollisionWithBorder()
        {
            int xHeadSnakePosition = _snakeBody[^1].X, yHeadSnakePosition = _snakeBody[^1].Y;

            if (!_isWallsEmpty)
            {
                if (xHeadSnakePosition <= _borders.LeftLimit || xHeadSnakePosition >= _borders.Width - 1 || yHeadSnakePosition <= _borders.TopLimit
                    || yHeadSnakePosition >= _borders.Height - 1)
                {
                    _isAlive = false;

                    OnDied?.Invoke();

                    Console.SetCursorPosition(xHeadSnakePosition, yHeadSnakePosition);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(" ");

                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(_borders.Width / 2, _borders.Height / 2);
                    Console.WriteLine("YOUR LOSE");

                    Thread.Sleep(1000);
                }
            }
            else
            {
                const int CORRECTION_VALUE_ZERO = 0, CORRECTION_VALUE_ONE = 1, CORRECTION_VALUE_TWO = 2;

                int x = xHeadSnakePosition, y = yHeadSnakePosition;

                if (xHeadSnakePosition == CORRECTION_VALUE_ZERO)
                {
                    _x = _borders.Width - CORRECTION_VALUE_TWO;
                }
                else if (xHeadSnakePosition == _borders.Width - CORRECTION_VALUE_ONE)
                {
                    _x = CORRECTION_VALUE_ONE;
                }
                else if (yHeadSnakePosition == CORRECTION_VALUE_ZERO)
                {
                    _y = _borders.Height - CORRECTION_VALUE_TWO;
                }
                else if (yHeadSnakePosition == _borders.Height - CORRECTION_VALUE_ONE)
                {
                    _y = CORRECTION_VALUE_ONE;
                }
                else
                {
                    return;
                }

                _isGoneThroughWall = true;

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
                        Console.SetCursorPosition(_borders.Width / 2 - 5, _borders.Height / 2);
                        Console.WriteLine("GG");
                        Thread.Sleep(500);
                        OnDied?.Invoke();
                        _isAlive = false;
                    }

                    counter++;
                }
            }
        }

        public void SpeetIncrease()
        {
            _acceleratorSpeed = _speed / 2;
            _isEatedAccelerator = true;

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
            }

            _isEatedAccelerator = false;
        }
    }
}
