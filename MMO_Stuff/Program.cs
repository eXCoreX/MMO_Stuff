using System;

namespace MMO_Stuff
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Random rnd = new Random();
            long cnt = 0;
            for (int i = 0; i < 1000000; i++)
            {
                int randInt = rnd.Next(-100, 100);
                int randInt2 = rnd.Next(-100, 100);
                var actual = LinearOptimization.GetMinimum(
                    x => 
                    { 
                        cnt++; 
                        return randInt2 + Math.Pow(x - randInt, 2); 
                    }, 1e-7, LinearOptimization.LinearMethod.GoldenRatio);
                var expected = new PointD(randInt, randInt2);
                if ((actual - expected).Norm > 1e-6)
                {
                    Console.WriteLine($"Fail {randInt} {randInt2}");
                }
            }
            Console.WriteLine(cnt);
        }
    }
}
