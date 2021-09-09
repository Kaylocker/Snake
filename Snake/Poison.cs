using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Snake
{
    class Poison
    {
        object findPosLocker = new();
        private static Thread _generator;
        private static Poison _poison;
        private static Random _random = new();
        private static bool _isActive = false;
        private static int _x;
        private static int _y;
        private const int DELAY = 1000;
        private char _body = '†';
        public static int X { get => _x; }
        public static int Y { get => _y; }
        public static bool IsActive { get => _isActive; set => _isActive = value; }
        public static bool IsThreadGeneratorActive
        {
            get
            {
                if (_generator == null || _generator.IsAlive == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private Poison()
        {
            lock (findPosLocker)
            {
                FindPosition();

                SetPosition();
            }
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(DELAY);
                }

                _poison = null;

            lock (findPosLocker)
            {
                ClearBody();

                GetInstance();
            }
           
            
        }

        public static void GetInstance()
        {
            bool isGenerated = false;

            do
            {
                if (_poison == null && /*Snake.IsAlive*/ IsActive)
                {
                    isGenerated = true;
                    _poison = new();
                }
            } while (!isGenerated);

        }

        public static void Generator()
        {
            ThreadStart poisonGenerator = new(GetInstance);
            _generator = new Thread(poisonGenerator);
            _generator.Start();
        }
        private void FindPosition()
        {
            lock (findPosLocker)
            {
                bool canSetPosition = true;

                do
                {
                    _x = _random.Next(1, Borders.Width - 1);
                    _y = _random.Next(1, Borders.Height - 1);

                    //foreach (Position item in Snake.SnakeBody)
                    //{
                    //    if (item.X == _x && item.Y == _y)
                    //    {
                    //        canSetPosition = false;
                    //    }
                    //}

                    //if (Food.X == _x && Food.Y == _y)
                    //{
                    //    canSetPosition = false;
                    //}
                } while (!canSetPosition);
            }
           
        }

        private void SetPosition()
        {
            lock (findPosLocker)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(_x, _y);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.Write(_body);

                Console.ResetColor();
            }
        }

        public static void Destroy()
        {
            _poison = null;
        }

        private void ClearBody()
        {
            lock (findPosLocker)
            {
                Console.SetCursorPosition(_x, _y);

                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(" ");
            }
           
        }
    }
}
