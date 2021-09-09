using System;
using System.Collections.Generic;

namespace Snake
{
    class Classic : Food
    {
        private char _body = '♥';

        public Classic()
        {
            CurrentType = CLASSIC;
        }

        private void Generate()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.ForegroundColor = ConsoleColor.DarkRed;
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
