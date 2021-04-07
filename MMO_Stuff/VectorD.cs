using System;
using System.Collections.Generic;
using System.Text;

namespace MMO_Stuff
{
    public class VectorD : ICloneable
    {
        public VectorD(int n, double initVal = 0)
        {
            N = n;
            Coords = new double[N];
            for (int i = 0; i < N; i++)
            {
                Coords[i] = initVal;
            }
        }

        public VectorD(double[] coords)
        {
            if (coords == null || coords.Length < 1)
            {
                throw new ArgumentNullException("coords can't be null");
            }
            else if (coords.Length < 1)
            {
                throw new ArgumentException("Dimensions can't be less than 1");
            }
            N = coords.Length;
            Coords = coords;
        }

        public int N { get; private set; }

        public double[] Coords { get; private set; }

        public double Norm
        {
            get
            {
                double result = 0;
                for (int i = 0; i < N; i++)
                {
                    result += Coords[i] * Coords[i];
                }
                return Math.Sqrt(result);
            }
        }

        public double this[int index]
        {
            get { return Coords[index]; }
            set { Coords[index] = value; }
        }

        public override string ToString()
        {
            return "(" + string.Join(", ", Coords) + ")";
        }

        public object Clone()
        {
            return new VectorD((double[])Coords.Clone());
        }

        public static VectorD operator +(VectorD lhs, VectorD rhs)
        {
            if (lhs.N != rhs.N)
            {
                throw new ArgumentException("Dimensions are not equal");
            }

            double[] coords = new double[lhs.N];
            for (int i = 0; i < coords.Length; i++)
            {
                coords[i] = lhs[i] + rhs[i];
            }
            return new VectorD(coords);
        }

        public static VectorD operator -(VectorD lhs, VectorD rhs)
        {
            if (lhs.N != rhs.N)
            {
                throw new ArgumentException("Dimensions are not equal");
            }

            double[] coords = new double[lhs.N];
            for (int i = 0; i < coords.Length; i++)
            {
                coords[i] = lhs[i] - rhs[i];
            }
            return new VectorD(coords);
        }

        public static VectorD operator -(VectorD vec)
        {
            double[] coords = (double[])vec.Coords.Clone();
            for (int i = 0; i < coords.Length; i++)
            {
                coords[i] = -coords[i];
            }
            return new VectorD(coords);
        }

        public static VectorD operator *(VectorD lhs, double rhs)
        {
            double[] coords = new double[lhs.N];
            for (int i = 0; i < coords.Length; i++)
            {
                coords[i] = lhs[i] * rhs;
            }
            return new VectorD(coords);
        }

        public static VectorD operator *(double lhs, VectorD rhs)
        {
            double[] coords = new double[rhs.N];
            for (int i = 0; i < coords.Length; i++)
            {
                coords[i] = rhs[i] * lhs;
            }
            return new VectorD(coords);
        }

    }
}
