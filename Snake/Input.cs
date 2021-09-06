using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Snake
{
    class Input
    {
        private static ConsoleKeyInfo _key = new ConsoleKeyInfo();
        private static ConsoleKey _input;
        private const char _leftDirection = 'l', _rightDirection = 'r', _upDirection = 'u', _downDirection = 'd';
        public static char LeftDirection { get => _leftDirection; }
        public static char RightDirection { get => _rightDirection; }
        public static char UpDirection { get => _upDirection; }
        public static char DownDirection { get => _downDirection; }
        public static ConsoleKey InputKey { get => _input; set => _input = value; }

        public Input()
        {
            Thread inputing = new(Input.Inputing);
            inputing.Start();
        }
        public static void Inputing()
        {
            while (true)
            {
                _key = Console.ReadKey(true);
                InputKey = _key.Key;
            }
        }

    }
}
