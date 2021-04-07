using System;
using System.Collections.Generic;
using System.Text;

namespace MMO_Stuff
{
    public class VectorD
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

        public int N { get; private set; }

        public double[] Coords { get; private set; }

        public double this[int index]
        {
            get { return Coords[index]; }
            set { Coords[index] = value; }
        }

    }
}
