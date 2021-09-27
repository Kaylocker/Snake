using System;

namespace Snake
{
    class Score
    {
        Borders _borders;
        private const int CORRECTION_VALUE = 4;
        private int _currentScore;
        private int _x, _y = 0;

        public int CurrentScore
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

        public Score(Borders borders)
        {
            _currentScore = 0;
            _borders = borders;
            _x = _borders.Width / 2 - CORRECTION_VALUE;

            ShowCurrentScore();
        }

        public void CollectScore(int score)
        {
            CurrentScore += score;
        }

        public void ShowCurrentScore()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(_x, _y);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"SCORE: {_currentScore}");
        }
    }
}
