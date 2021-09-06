using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Snake
{
    class Score
    {
        private static int _currentScore;
        private static int _x = Borders.Width / 2 - 4;
        private static int _y = 0;

        public static int CurrentScore
        {
            get
            {
                return _currentScore;
            }
            set
            {
                _currentScore=value;
                ShowCurrentScore();
            }
        }

        public Score()
        {
            _currentScore = 0;
            ShowCurrentScore();
        }

        public static void ShowCurrentScore()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(_x, _y);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"SCORE: {_currentScore}");
        }
    }
}
