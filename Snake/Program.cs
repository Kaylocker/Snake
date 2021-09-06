using System;
using System.Threading;
using System.Diagnostics;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread inputing = new(Input.Inputing);
            inputing.Start();
            Borders borders = new();
            Menu menu = new();
        }

        public static void Play()
        {
            Console.Clear();

            Borders.DrawBorders();
            Snake snake = Snake.GetInstance();
            Food.SetCurrentType();
            Score score = new();

            if (!Poison.IsThreadGeneratorActive)
            {
                Poison.Generator();
            }

            while (Snake.IsAlive)
            {
                snake.Move();
                snake.Eat();
                snake.PrintSnake();
                snake.OnCollisionEnter();
            }

            Snake.DeleteInstance();
            Food.Destroy();
        }
    }
}
