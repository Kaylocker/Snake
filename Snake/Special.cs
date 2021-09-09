using System;
using System.Collections.Generic;

namespace Snake
{
    class Special : Food
    {
        private char _body = '▲';

        public Special()
        {
            CurrentType = SPECIAL;
        }

        private void Generate()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.ForegroundColor = ConsoleColor.Yellow;
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
