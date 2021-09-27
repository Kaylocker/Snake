using System;
using System.Collections.Generic;

namespace Snake
{
    class Accelerator : Food
    {
        private char _body = '►';
        private const int _score = 3;

        public Accelerator()
        {
            CurrentType = ACCELERATOR;
            Score = _score;
        }

        private void Generate()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(_body);

            Console.ResetColor();
        }

        public override void Instantiate(List<Position> snakeBody)
        {
            _snakeBody = snakeBody;
            FindPosition();
            Generate();
        }
    }
}
