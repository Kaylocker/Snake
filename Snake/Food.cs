﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    abstract class Food
    {
        private int _currentType;
        private Position _position;
        protected List<Position> _snakeBody;
        protected const int CLASSIC = 0, ACCELERATOR = 1, SPECIAL = 2;
        public int CurrentType { get => _currentType; protected set => _currentType = value; }
        public Position Position { get => _position; protected set => _position = value; }

        public static Food GetCurrentType(bool isSpecialActive, bool isAcceleratorActive)
        {
            Random random = new();
            double chance = random.NextDouble();

            if (chance < 0.7)
            {
                Food classicFood = new Classic();
                return classicFood;
            }
            else if (chance >= 0.7 && chance < 0.85)
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
            else if (chance >= 0.85)
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
            else if (!isSpecialActive && !isAcceleratorActive)
            {
                Food classicFood = new Classic();
                return classicFood;
            }

            return null;
        }

        protected void FindPosition()
        {
            bool canSetPosition = true;

            Random random = new();

            do
            {
                int x = random.Next(1, Borders.Width - 1);
                int y = random.Next(1, Borders.Height - 1);

                _position = new Position(x, y);

                foreach (Position item in _snakeBody)
                {
                    if (item == _position)
                    {
                        continue;
                    }
                }

            } while (!canSetPosition);
        }

        public virtual void Instantiate(List<Position> snakeBody) { }
    }
}
