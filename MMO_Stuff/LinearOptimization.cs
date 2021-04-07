using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MMO_Stuff
{
    public static class LinearOptimization
    {
        public enum LinearMethod
        {
            Dichotomy = 0,
            GoldenRatio,
            Fibonacci,
            Parabolic
        }
        public static PointF GetMinimum(Func<float, float> func, float eps = 1e-6f, LinearMethod method = LinearMethod.Dichotomy)
        {
            (float a, float b) = LocalizeMinimum(func, eps);
            if (a == b)
            {
                return new PointF(a, func(a));
            }
            else
            {
                return method switch
                {
                    LinearMethod.Dichotomy => GetMinimumDichotomy(func, a, b, eps),
                    LinearMethod.GoldenRatio => GetMinimumGoldenRatio(func, a, b, eps),
                    LinearMethod.Fibonacci => GetMinimumFibonacci(func, a, b, eps),
                    LinearMethod.Parabolic => GetMinimumParabolic(func, a, b, eps),
                    _ => throw new ArgumentException("Not valid method"),
                };
            }
        }

        public static PointF GetMinimumDichotomy(Func<float, float> func, float a, float b, float eps)
        {
            float delta = eps / 3;
            do
            {
                float x1 = (a + b - delta) / 2;
                float x2 = (a + b + delta) / 2;
                float f1 = func(x1);
                float f2 = func(x2);
                if (f1 <= f2)
                {
                    b = x2;
                }
                else
                {
                    a = x1;
                }
            } while (b - a >= eps);
            return new PointF((a + b) / 2, func((a + b) / 2));
        }

        public static PointF GetMinimumGoldenRatio(Func<float, float> func, float a, float b, float eps)
        {
            float u = (a + (3 - MathF.Sqrt(5)) / 2 * (b - a));
            float v = a + b - u;
            float fu = func(u);
            float fv = func(v);
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
                    u = a + (3 - MathF.Sqrt(5)) / 2 * (b - a);
                    v = a + b - u;
                    fu = func(u);
                    fv = func(v);
                }
            } while (b - a >= eps);

            return new PointF((a + b) / 2, func((a + b) / 2));
        }
        public static PointF GetMinimumFibonacci(Func<float, float> func, float a, float b, float eps)
        {
            throw new NotImplementedException();
        }

        public static PointF GetMinimumParabolic(Func<float, float> func, float a, float b, float eps)
        {
            throw new NotImplementedException();
        }

        public static (float a, float b) LocalizeMinimum(Func<float, float> func, float eps)
        {
            float x1 = 0f, x2;
            float h = 1e6f;
            float f1 = func(x1), f2;
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
