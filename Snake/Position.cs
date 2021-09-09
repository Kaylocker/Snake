using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public struct Position : IEquatable<Position>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Position))
                return false;

            return Equals((Position)obj);
        }

        public bool Equals(Position other)
        {
            if (X != other.X)
                return false;

            return Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public static bool operator == (Position c1, Position c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Position c1, Position c2)
        {
            return !c1.Equals(c2);
        }
    }
}
