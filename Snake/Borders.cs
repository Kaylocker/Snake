using System;
using System.Diagnostics;

namespace Snake
{
    class Borders
    {

        private static int _height = 30;
        private static int _width = 50;
        private static int _leftLimit = 0;
        private static int _topLImit = 0;
        private static bool _emptyWalls = false;
        public static bool EmptyWalls { get => _emptyWalls; set => _emptyWalls = value; }

        public static int Height { get => _height; }
        public static int Width { get => _width; }
        public static int LeftLimit { get => _leftLimit; }
        public static int TopLimit { get => _topLImit; }
        public Borders()
        {
            Debug.Assert(OperatingSystem.IsWindows());
            Console.Title = "Snake";
            Console.SetWindowSize(_width, _height);
            Console.SetBufferSize(_width+1, _height + 1);
        }

        public static void DrawBorders()
        {
            Console.Clear();

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if(i==0 || i == _height - 1)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write(" ");
                    }
                    else
                    {
                        if(j==0 || j==_width-1)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(" ");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.Write(" ");
                        }
                    }

                    Console.ResetColor();
                }

                Console.WriteLine(" ");
            }
        }
    }
}
