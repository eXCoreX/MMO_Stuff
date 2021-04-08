using System;

namespace MMO_Stuff
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
            Random rnd = new Random();
            long cnt = 0;
            for (int i = 0; i < 100000; i++)
            {
                int randInt1 = rnd.Next(-100, 100);
                int randInt2 = rnd.Next(-100, 100);
                int randInt3 = rnd.Next(-100, 100);
                int randInt4 = rnd.Next(-100, 100);
                var actual = DimensionalOptimization.GetMinimum(
                    x => 
                    { 
                        cnt++; 
                        return randInt4 + Math.Pow(x[0] - randInt1, 2) + Math.Pow(x[1] - randInt2, 2) + Math.Pow(x[2] - randInt3, 2); 
                    }, 3, 1e-7);
                var expected = (X: new VectorD(new double[] { randInt1, randInt2, randInt3 }), F: randInt4);
                if ((actual.X - expected.X).Norm > 1e-6)
                {
                    Console.WriteLine($"Fail {randInt1} {randInt2} {randInt3} {randInt4}");
                }
            }
            Console.WriteLine(cnt);
        }
    }
}
