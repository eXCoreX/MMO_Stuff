using System;

namespace MMO_Stuff
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lab 6:");
            TestRosenbrock(1e-4);
            TestRosenbrock(1e-8);
            TestIndividual(1e-4);
            TestIndividual(1e-8);

            Console.WriteLine("Lab 7:");
            TestGlobalMin(1e-2);
            TestGlobalMin(1e-4);
        }

        static void TestRosenbrock(double eps)
        {
            var result = DimensionalOptimization.GetMinimumWithGradient(
                x =>
                {
                    return 100 * (x[1] - x[0] * x[0]) * (x[1] - x[0] * x[0]) + (1 - x[0]) * (1 - x[0]);
                },
                x =>
                {
                    double[] coords = new double[]
                    {
                        2.0 * x[0] - 2.0 - 400.0 * x[0] * (x[1] - x[0] * x[0]),
                        200.0 * (x[1] - x[0] * x[0])
                    };
                    return new VectorD(coords);
                },
                2, null, eps, DimensionalOptimization.GradientMethod.FastDescent);

            Console.WriteLine($"rosenbrock fast descent, eps = {eps}, result:\n{result}");
        }

        static void TestIndividual(double eps)
        {
            var initialPoint = new VectorD(new[] { 1.0, 1.0 });

            int functionCalls = 0;

            Func<VectorD, double> func = x =>
            {
                functionCalls++;
                return x[0] * x[0] - 2 * x[0] * x[1] + 3 * x[1] * x[1] + x[0] - 4 * x[1];
            };

            Func<VectorD, VectorD> gradientFunc = x =>
            {
                double[] coords = new double[]
                {
                        2.0 * x[0] - 2.0 * x[1] + 1.0,
                        6.0 * x[1] - 2.0 * x[0] - 4.0
                };
                return new VectorD(coords);
            };

            var resultStep = DimensionalOptimization.GetMinimumWithGradient(
                func,
                gradientFunc,
                2,
                initialPoint,
                eps,
                DimensionalOptimization.GradientMethod.StepDivision);

            Console.WriteLine($"individual, step division, eps = {eps}, result:\n{resultStep}, functionCalls: {functionCalls}");

            functionCalls = 0;

            var resultFast = DimensionalOptimization.GetMinimumWithGradient(
                func,
                gradientFunc,
                2,
                initialPoint,
                eps,
                DimensionalOptimization.GradientMethod.FastDescent);

            Console.WriteLine($"individual, fast descent, eps = {eps}, result:\n{resultFast}, functionCalls: {functionCalls}");

            functionCalls = 0;

            var resultCoord = DimensionalOptimization.GetMinimum(
                func,
                2,
                initialPoint,
                eps,
                DimensionalOptimization.NonGradientMethod.CoordinateDescent);

            Console.WriteLine($"individual, coordinate descent, eps = {eps}, result:\n{resultCoord}, functionCalls: {functionCalls}");
        }

        static void TestGlobalMin(double eps)
        {
            int callCount = 0;

            double func(double x)
            {
                callCount++;
                return Math.Sin(x) / x;
            }

            var a = 2.0;
            var b = 13.0;

            var lK = 50;

            var globalUniform = LinearOptimization.GetGlobalMinimum(
                func,
                eps,
                a,
                b,
                lK,
                LinearOptimization.GlobalMethod.Uniform);

            Console.WriteLine($"\n\nGlobal sin(x)/x, uniform, precision {eps}, result:\n {globalUniform}, calls: {callCount}");

            callCount = 0;

            var globalSequential = LinearOptimization.GetGlobalMinimum(
                func,
                eps,
                a,
                b,
                lK,
                LinearOptimization.GlobalMethod.Sequential);

            Console.WriteLine($"\n\nGlobal sin(x)/x, sequential, precision {eps}, result:\n {globalSequential},calls: {callCount}");

            callCount = 0;

            var globalPolyline = LinearOptimization.GetGlobalMinimum(
                func,
                eps,
                a,
                b,
                lK,
                LinearOptimization.GlobalMethod.Polyline);

            Console.WriteLine($"\n\nGlobal sin(x)/x, polyline, precision {eps}, result:\n {globalPolyline},calls: {callCount}");
        }
    }
}
