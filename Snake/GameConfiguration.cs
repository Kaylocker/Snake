using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class GameConfiguration
    {
        private static int _speed=100;

        public static int Speed { get => _speed; set => _speed = value; }
    }
}
