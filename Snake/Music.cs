using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Snake
{

    class Music
    {
        private static bool _isOn = true;
        public static bool IsOn { get => _isOn; set => _isOn = value; }
        public static void Play()
        {
            if (!_isOn)
            {
                return;
            }
        }
    }
}
