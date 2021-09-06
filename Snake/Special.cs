using System;

namespace Snake
{
    class Special : Food
    {
        private static Special _special;
        private static bool _isActive = false;
        private char _body = '▲';
        public static bool IsActive { get => _isActive; set => _isActive = value; }

        private Special()
        {
            FindPosition();
            SetPosition();
        }

        private void FindPosition()
        {
            bool canSetPosition = true;

            do
            {
                X = random.Next(1, Borders.Width - 1);
                Y = random.Next(1, Borders.Height - 1);

                foreach (Position item in Snake.SnakeBody)
                {
                    if (item.X == X && item.Y == Y)
                    {
                        canSetPosition = false;
                    }
                }
            } while (!canSetPosition);
        }

        public static void GetInstance()
        {
            if (_special == null)
            {
                _special = new();
            }
        }

        public static bool CheckInstance()
        {
            if (_special != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static new void Destroy()
        {
            if (_special != null)
            {
                _special = null;
            }
        }

        private void SetPosition()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(_body);

            Console.ResetColor();
        }
    }
}
