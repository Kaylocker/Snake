using System;

namespace Snake
{
    abstract class Food
    {
        protected static Random random = new();
        private static int _x;
        private static int _y;
        public static int X { get => _x; protected set => _x = value; }
        public static int Y { get => _y; protected set => _y = value; }

        public static void SetCurrentType()
        {
            double chance = random.NextDouble();

            if (chance < 0.7)
            {
                Classic.GetInstance();

            }
            else if (chance >= 0.7 && chance < 0.85)
            {
                if (Special.IsActive)
                {
                    Special.GetInstance();
                }
                else
                {
                    Classic.GetInstance();
                }

            }
            else if (chance >= 0.85)
            {
                if (Accelerator.IsActive)
                {
                    Accelerator.GetInstance();
                }
                else
                {
                    Classic.GetInstance();
                }
            }
            else if (!Special.IsActive && !Accelerator.IsActive)
            {
                Classic.GetInstance();
            }
        }

        public static void Destroy()
        {
            Classic.Destroy();
            Accelerator.Destroy();
            Special.Destroy();
        }
    }
}
