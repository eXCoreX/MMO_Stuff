using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MMO_Stuff
{
    public static class DimensionalOptimization
    {
        public enum GradientMethod
        {
            FastDescent = 0,
            StepDivision
        }

        public enum NonGradientMethod
        {
            CoordinateDescent = 0
        }

        public static (VectorD X, double F) GetMinimumWithGradient(Func<VectorD, double> func, Func<VectorD, VectorD> gradient, int dimensions, double precision = 1e-7, GradientMethod method = GradientMethod.FastDescent)
        {
            if (precision < 1e-10)
            {
                throw new ArgumentException("Precision is too high");
            }
            else
            {
                return method switch
                {
                    GradientMethod.FastDescent => GetMinimumFastDescent(func, gradient, dimensions, precision),
                    GradientMethod.StepDivision => GetMinimumStepDivision(func, gradient, dimensions, precision),
                    _ => throw new ArgumentException("Not valid method"),
                };
            }
        }

        public static (VectorD X, double F) GetMinimumFastDescent(Func<VectorD, double> func, Func<VectorD, VectorD> gradient, int dimensions, double eps)
        {
            if (eps < 0)
            {
                eps = -eps;
            }
            if (eps < 1e-10)
            {
                throw new ArgumentException("Precision is too high");
            }
            VectorD x0 = new VectorD(dimensions);

            VectorD g = gradient(x0);

            if (g.Norm > eps)
            {
                VectorD x;
                do
                {
                    x = x0;
                    double h = FindStepForFastDescent(func, gradient, x, g, eps);
                    x0 = x - h * g;
                    g = gradient(x0);
                } while ((x - x0).Norm >= eps && g.Norm >= eps);
            }

            return (x0, func(x0));
        }

        private static double FindStepForFastDescent(Func<VectorD, double> func, Func<VectorD, VectorD> gradient, VectorD x, VectorD g, double eps)
        {
            var result = LinearOptimization.GetMinimum((double step) =>
            {
                return func(x - g * step);
            }, eps);
            return result.X;
        }

        public static (VectorD X, double F) GetMinimumStepDivision(Func<VectorD, double> func, Func<VectorD, VectorD> gradient, int dimensions, double eps)
        {
            throw new NotImplementedException();
        }
    }
}
