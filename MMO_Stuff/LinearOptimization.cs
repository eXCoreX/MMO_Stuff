using System;

namespace MMO_Stuff
{
    public static class LinearOptimization
    {
        public enum LinearMethod
        {
            Dichotomy = 0,
            GoldenRatio,
            //Fibonacci,
            //Parabolic
        }

        /// <summary>
        /// Get minimum of given unimodal function. If function is not unimodal or doesn't have global minimum, behaviour is unexpected.
        /// </summary>
        /// <param name="func"> Unimodal Function</param>
        /// <param name="precision"> Epsilon for fp methods </param>
        /// <param name="method"> Method of computing minimum </param>
        /// <returns> Minimum of the function </returns>
        public static PointD GetMinimum(Func<double, double> func, double precision = 1e-7, LinearMethod method = LinearMethod.Dichotomy)
        {
            if (precision < 1e-10)
            {
                throw new ArgumentException("Precision is too high");
            }
            (double a, double b) = LocalizeMinimum(func, 1e-10);
            if (a == b)
            {
                return new PointD(a, func(a));
            }
            else
            {
                return method switch
                {
                    LinearMethod.Dichotomy => GetMinimumDichotomy(func, a, b, precision),
                    LinearMethod.GoldenRatio => GetMinimumGoldenRatio(func, a, b, precision),
                    //LinearMethod.Fibonacci => GetMinimumFibonacci(func, a, b, (int)precision),
                    //LinearMethod.Parabolic => GetMinimumParabolic(func, a, b, eps),
                    _ => throw new ArgumentException("Not valid method"),
                };
            }
        }

        public static PointD GetMinimumDichotomy(Func<double, double> func, double a, double b, double eps)
        {
            if (a > b)
            {
                double t = a;
                a = b;
                b = t;
            }
            if (eps < 0)
            {
                eps = -eps;
            }
            if (eps < 1e-10)
            {
                throw new ArgumentException("Precision is too high");
            }
            double delta = eps / 3;
            do
            {
                double x1 = (a + b - delta) / 2;
                double x2 = (a + b + delta) / 2;
                double f1 = func(x1);
                double f2 = func(x2);
                if (f1 <= f2)
                {
                    b = x2;
                }
                else
                {
                    a = x1;
                }
            } while (b - a >= eps);
            return new PointD((a + b) / 2, func((a + b) / 2));
        }

        public static PointD GetMinimumGoldenRatio(Func<double, double> func, double a, double b, double eps)
        {
            if (a > b)
            {
                double t = a;
                a = b;
                b = t;
            }
            if (eps < 0)
            {
                eps = -eps;
            }
            if (eps < 1e-10)
            {
                throw new ArgumentException("Precision is too high");
            }
            double u = ((double)(a + (3 - Math.Sqrt(5)) / 2 * (b - a)));
            double v = a + b - u;
            double fu = func(u);
            double fv = func(v);
            do
            {
                if (fu <= fv)
                {
                    b = v;
                    v = u;
                    fv = fu;
                    u = a + b - v;
                    fu = func(u);
                }
                else
                {
                    a = u;
                    u = v;
                    fu = fv;
                    v = a + b - u;
                    fv = func(v);
                }

                if (u > v)
                {
                    u = (double)(a + (3 - Math.Sqrt(5)) / 2 * (b - a));
                    v = a + b - u;
                    fu = func(u);
                    fv = func(v);
                }
            } while (b - a >= eps);

            return new PointD((a + b) / 2, func((a + b) / 2));
        }

        private static PointD GetMinimumFibonacci(Func<double, double> func, double a, double b, int n)
        {
            throw new NotImplementedException();
        }

        private static PointD GetMinimumParabolic(Func<double, double> func, double a, double b, double eps)
        {
            throw new NotImplementedException();
        }

        public static (double a, double b) LocalizeMinimum(Func<double, double> func, double eps)
        {
            double x1 = 0, x2;
            double h = 1e6;
            double f1 = func(x1), f2;
            do
            {
                h /= 2;
                x2 = x1 + h;
                f2 = func(x2);
                if (f1 <= f2)
                {
                    h = -h;
                    x2 = x1 + h;
                    f2 = func(x2);
                }
            } while (f1 <= f2 && Math.Abs(h) >= eps);

            if (Math.Abs(h) > eps)
            {
                do
                {
                    x1 = x2;
                    f1 = f2;
                    x2 = x1 + h;
                    f2 = func(x2);
                } while (f1 >= f2);

                if (h > 0)
                {
                    return (x1 - h, x2);
                }
                else
                {
                    return (x2, x1 - h);
                }
            }
            else
            {
                return (x1, x1);
            }
        }
    }
}
