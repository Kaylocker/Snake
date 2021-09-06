using System;
using System.Threading;

namespace Snake
{
    class Accelerator : Food
    {
        private static Accelerator _accelerator;
        private static bool _isActive = false;
        private char _body = '►';
        public static bool IsActive { get => _isActive; set => _isActive = value; }

        private Accelerator()
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
            if (_accelerator == null)
            {
                _accelerator = new();
            }
        }

        public static bool CheckInstance()
        {
            if (_accelerator != null)
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
            if (_accelerator != null)
            {
                _accelerator = null;
            }
        }

        private void SetPosition()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(_body);

            Console.ResetColor();
        }

        public static void ResetSpeed()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
            }

            Snake.Speed = GameConfiguration.Speed;
        }
    }
}
