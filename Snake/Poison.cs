using System;
using System.Threading;

namespace Snake
{
    class Poison
    {
        object _poisonLocker = new();
        Thread _poisonGenerator;
        private Food _food;
        private Random _random = new();
        private Snake _snake;
        private Borders _borders;
        private Position _position;
        private int DELAY = 300;
        private char _body = '†';
        private bool _isSnakeAlive;

        public Food Food { init => _food = value; }
        public bool IsSnakeAlive { get => _isSnakeAlive; set => _isSnakeAlive = value; }
        public Position Position { get => _position; }

        public Poison(Food food, bool isPoisonActivate, Snake snake)
        {
            if (!isPoisonActivate)
            {
                return;
            }

            Food = food;
            _snake = snake;
            _borders = _snake.Borders;

            _poisonGenerator = new Thread(new ThreadStart(Generator));
            _poisonGenerator.Start();

        }
        private void Generator()
        {
            do
            {
                FindPosition();
                SetPosition();


                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(DELAY);
                }

                ClearBody();

            } while (_snake.IsAlive);

            _poisonGenerator.Join();

        }

        public void StopThread()
        {
            _poisonGenerator.Join();
        }

        private void FindPosition()
        {
            lock (_poisonLocker)
            {
                bool canSetPosition = true;

                do
                {
                    int x = _random.Next(1, _borders.Width - 1);
                    int y = _random.Next(1, _borders.Height - 1);

                    _position = new(x, y);

                    foreach (Position item in _snake.SnakeBody)
                    {
                        if (item == _position)
                        {
                            canSetPosition = false;
                        }
                    }

                    if (_food.Position == _position)
                    {
                        canSetPosition = false;
                    }
                } while (!canSetPosition);
            }

        }
         
        private void SetPosition()
        {
            lock (_poisonLocker)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(_position.X, _position.Y);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.Write(_body);

                Console.ResetColor();
            }
        }

        private void ClearBody()
        {
            //lock (_poisonLocker)
            //{
            Console.SetCursorPosition(_position.X, _position.Y);

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(" ");
            //}

        }
    }
}
