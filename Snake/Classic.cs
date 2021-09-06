using System;

namespace Snake
{
    class Classic : Food
    {
        private static Classic _classic;
        private static bool _isActive = true;
        private char _body = '♥';
        public static bool IsActive { get => _isActive; set => _isActive = value; }

        private Classic()
        {
            FindPosition();
            SetPosition();
        }

        protected void FindPosition()
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
            if (_classic == null)
            {
                _classic = new();
            }
        }

        public static bool CheckInstance()
        {
            if (_classic != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetPosition()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(_body);

            Console.ResetColor();
        }

        public static new void Destroy()
        {
            if (_classic != null)
            {
                _classic = null;
            }
        }
    }

}
