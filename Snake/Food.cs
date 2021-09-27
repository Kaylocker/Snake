using System;
using System.Collections.Generic;

namespace Snake
{
    abstract class Food
    {
        private Borders _borders;
        private Position _position;
        protected List<Position> _snakeBody;
        protected const int CLASSIC = 0, ACCELERATOR = 1, SPECIAL = 2;
        private int _currentType, _score;

        public int CurrentType { get => _currentType; protected set => _currentType = value; }
        public int Score { get => _score; protected set => _score = value; }
        public Position Position { get => _position; protected set => _position = value; }
        public Borders Borders 
        { 
            get=>_borders;

            set
            {
                if (_borders == null)
                {
                    _borders = value;
                }
            }
        }

        public static Food GetCurrentType(bool isSpecialActive, bool isAcceleratorActive)
        {
            Random random = new();
            double chance = random.NextDouble();

            if (chance < 0.8)
            {
                Food classicFood = new Classic();
                return classicFood;
            }
            else if (chance >= 0.8 && chance < 0.9)
            {
                if (isSpecialActive)
                {
                    Food specialFood = new Special();
                    return specialFood;
                }
                else
                {
                    Food classicFood = new Classic();
                    return classicFood;
                }

            }
            else if (chance >= 0.9)
            {
                if (isAcceleratorActive)
                {
                    Food accelerator = new Accelerator();
                    return accelerator;
                }
                else
                {
                    Food classicFood = new Classic();
                    return classicFood;
                }
            }

            return null;
        }

        protected virtual void FindPosition()
        {
            bool canSetPosition;

            Random random = new();

            do
            {
                canSetPosition = true;

                int x = random.Next(1, _borders.Width - 1);
                int y = random.Next(1, _borders.Height - 1);

                _position = new Position(x, y);

                foreach (Position item in _snakeBody)
                {
                    if (item == _position)
                    {
                        canSetPosition = false;
                    }
                }
            } while (!canSetPosition);
        }

        public virtual void Instantiate(List<Position> snakeBody) { }
    }
}
