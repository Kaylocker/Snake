using System;
using System.Threading;

namespace Snake
{
    class SoundManager
    {
        private object _locker = 1;
        private bool _isOn = false;

        public bool IsOn { get => _isOn; set => _isOn = value; }

        public void PlayEatSnake(int tone)
        {
            if (!_isOn)
            {
                return;
            }

            Thread eatSound = new Thread(delegate ()
            {
                lock(_locker)
                {
                    Console.Beep(349 * tone, 250);
                    Console.Beep(293 * tone, 200);
                    Console.Beep(392 * tone, 150);
                }
            });

            eatSound.Start();
        }

        public void PlayDeathSnake()
        {
            if (!_isOn)
            {
                return;
            }

            Thread deathSound = new Thread(delegate ()
            {
                lock (_locker)
                {
                    Console.Beep(147, 300);
                    Console.Beep(123, 300);
                    Console.Beep(110, 300);
                }
            });

            deathSound.Start();
        }

        public void PlaySwitchLocalMenuPosition()
        {
            if (!_isOn)
            {
                return;
            }

            Thread menuSwitch = new Thread(delegate ()
            {
                lock (_locker)
                {
                    Console.Beep(4698, 250);
                }
            });

            menuSwitch.Start();
        }

        public void PlaySwitchGlobalMenuPosition()
        {
            if (!_isOn)
            {
                return;
            }

            Thread menuSwitch = new Thread(delegate ()
            {
                lock (_locker)
                {
                    Console.Beep(3440, 250);
                }
            });

            menuSwitch.Start();
        }
    }
}
