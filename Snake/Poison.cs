using System;
using System.Collections.Generic;

namespace Snake
{
    class Poison
    {
        private Food _food;
        private Random _random = new();
        private Snake _snake;
        private Borders _borders;
        private List<Position> _positions;
        private const int _countOfPoisons = 3;
        private char _body = '†';
        private bool _isActive = false;

        public Food Food { init => _food = value; }
        public List<Position> Position { get => _positions; }
        public bool IsActive { get => _isActive; }

        public Poison(Food food, bool isPoisonActivate, Snake snake)
        {
            if (!isPoisonActivate)
            {
                return;
            }

            Food = food;
            _snake = snake;
            _borders = _snake.Borders;
            _isActive = true;

            FindPosition();

        }

        private void FindPosition()
        {
            _positions = new List<Position>();

            for (int i = 0; i < _countOfPoisons; i++)
            {
                Position position;
                bool canSetPosition = true;
                int correctionBorderValue = 1;

                do
                {
                    int x = _random.Next(correctionBorderValue, _borders.Width - correctionBorderValue);
                    int y = _random.Next(correctionBorderValue, _borders.Height - correctionBorderValue);

                    position = new(x, y);

                    foreach (Position item in _snake.SnakeBody)
                    {
                        if (item == position)
                        {
                            canSetPosition = false;
                        }
                    }

                    foreach (Position item in _positions)
                    {
                        if (item == position)
                        {
                            canSetPosition = false;
                        }
                    }

                    if (_food.Position == position)
                    {
                        canSetPosition = false;
                    }

                } while (!canSetPosition);

                _positions.Add(position);
            }
        }

        public void Instantiate()
        {
            for (int i = 0; i < _countOfPoisons; i++)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(_positions[i].X, _positions[i].Y);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.Write(_body);

                Console.ResetColor();
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _countOfPoisons; i++)
            {
                Console.SetCursorPosition(_positions[i].X, _positions[i].Y);
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(" ");
            }
        }
    }
}
