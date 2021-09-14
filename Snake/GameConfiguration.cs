namespace Snake
{
    class GameConfiguration 
    {
        private int _speed;
        private const int LOW_SPEED = 200, MEDIUM_SPEED = 100, HIGH_SPEED = 50, SUPER_SPEED = 25;
        private bool _isSpecialFoodActive = false, _isAcceleratorFoodActive = false, _isPoisonActive = false, _isWallsEmpty = false;

        public int LowSpeed { get => LOW_SPEED; }
        public int MediumSpeed { get => MEDIUM_SPEED; }
        public int HighSpeed { get => HIGH_SPEED; }
        public int SuperSpeed { get => SUPER_SPEED; }
        public bool IsSpecialFoodActive { get=>_isSpecialFoodActive; set=>_isSpecialFoodActive=value; }
        public bool IsAcceleratorFoodActive { get=> _isAcceleratorFoodActive; set=> _isAcceleratorFoodActive = value; }
        public bool IsPoisonActive { get=> _isPoisonActive; set=> _isPoisonActive = value; }
        public bool IsWallsEmpty { get=> _isWallsEmpty; set=> _isWallsEmpty = value; }
        public int Speed { get => _speed; set => _speed = value; }

        public GameConfiguration()
        {
            _speed = MEDIUM_SPEED;
        }
    }
}
