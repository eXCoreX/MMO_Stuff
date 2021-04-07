using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;

namespace MMO_Stuff
{
    public struct PointD : IEquatable<PointD>
    {
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public bool Equals(PointD other)
        {
            bool eqX = Math.Abs(X - other.X) <= double.Epsilon;
            bool eqY = Math.Abs(Y - other.Y) <= double.Epsilon;
            return eqX && eqY;
        }

        public override bool Equals(object obj)
        {
            var other = (PointD)obj;
            return obj is PointD && this.Equals(other);
        }

        public double Norm
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ (Y.GetHashCode() * 7);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static bool operator ==(PointD lhs, PointD rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(PointD lhs, PointD rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static PointD operator +(PointD lhs, PointD rhs)
        {
            return new PointD(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static PointD operator -(PointD lhs, PointD rhs)
        {
            return new PointD(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }
    }
}
