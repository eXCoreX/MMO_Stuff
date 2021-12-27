using System;
using System.Collections.Generic;
using System.Linq;

namespace MMO_Stuff;

#nullable enable

public static class LinearOptimization
{
    public enum LinearMethod
    {
        Dichotomy = 0,
        GoldenRatio,
        //Fibonacci,
        //Parabolic
    }

    public enum GlobalMethod
    {
        Uniform = 0,
        Sequential,
        Polyline
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
        throw new Exception();
        if (a > b)
        {
            double t = a;
            a = b;
            b = t;
        }
        if (n < 0)
        {
            throw new Exception();
        }
        if (n > 85)
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
        } while (b - a >= n);

        return new PointD((a + b) / 2, func((a + b) / 2));
    }

    private static PointD GetMinimumParabolic(Func<double, double> func, double a, double b, double eps)
    {
        throw new NotImplementedException();
    }

    public static (double a, double b) LocalizeMinimum(Func<double, double> func, double eps)
    {
        double x1 = 0, x2;
        double h = 1;
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

    public static PointD GetGlobalMinimum(
        Func<double, double> func,
        double precision = 1e-7,
        double a = -1e3,
        double b = 1e3,
        double lipschitzK = 1e3,
        GlobalMethod method = GlobalMethod.Polyline)
    {
        if (precision < 1e-10)
        {
            throw new ArgumentException("Precision is too high");
        }

        return method switch
        {
            GlobalMethod.Uniform => GetGlobalMinimumUniform(func, a, b, precision, lipschitzK),
            GlobalMethod.Sequential => GetGlobalMinimumSequential(func, a, b, precision, lipschitzK),
            GlobalMethod.Polyline => GetGlobalMinimumPolyline(func, a, b, precision, lipschitzK),
            _ => throw new ArgumentException("Not valid method"),
        };
    }

    public static PointD GetGlobalMinimumUniform(
        Func<double, double> func,
        double a,
        double b,
        double precision,
        double lipschitzK)
    {
        var h = 2 * precision / lipschitzK;
        var x0 = a + h / 2;
        var f0 = func(x0);
        var x_ans = x0;
        var f_ans = f0;

        do
        {
            x0 += h;
            if (b < x0)
            {
                x0 = b;
            }
            f0 = func(x0);
            if (f0 < f_ans)
            {
                f_ans = f0;
                x_ans = x0;
            }
        } while (x0 != b);

        return new PointD(x_ans, f_ans);
    }

    public static PointD GetGlobalMinimumSequential(
        Func<double, double> func,
        double a,
        double b,
        double precision,
        double lipschitzK)
    {
        var h = 2 * precision / lipschitzK;
        var x0 = a + h / 2;
        var f0 = func(x0);
        var x_ans = x0;
        var f_ans = f0;

        do
        {
            x0 += h + (f0 - f_ans) / lipschitzK;
            if (b < x0)
            {
                x0 = b;
            }
            f0 = func(x0);
            if (f0 < f_ans)
            {
                f_ans = f0;
                x_ans = x0;
            }
        } while (x0 != b);

        return new PointD(x_ans, f_ans);
    }

    public static PointD GetGlobalMinimumPolyline(
        Func<double, double> func,
        double a,
        double b,
        double precision,
        double lipschitzK)
    {
        List<double> y = new();
        y.Add(a);
        y.Add(b);

        var MAX_Y = 100_000;

        Random r = new();

        double z0(double x, double y)
        {
            double a, b;
            if (x < y)
            {
                a = x;
                b = y;
            }
            else
            {
                a = y;
                b = x;
            }
            //return (x + y) / 2;
            //return GetMinimumDichotomy(func, a, b, 0.1).X;

            return a + (b - a) * r.NextDouble();

        }

        double fz0(double x, double y)
        {
            return func(y) - lipschitzK * Math.Abs(x - y);
        }

        double[] z = new double[MAX_Y];
        z[0] = z0(y[0], y[1]);

        double[] fz = new double[MAX_Y];
        fz[0] = fz0(z[0], y[0]);

        var bestIdx = 0;
        var min = 0.0;

        do
        {
            min = fz.First();
            var iMin = 0;

            for (int i = 1; i < y.Count - 1; i++)
            {
                if (fz[i] < min)
                {
                    min = fz[i];
                    iMin = i;
                }
            }

            bestIdx = 0;

            while (y[bestIdx] < z[iMin])
            {
                bestIdx++;
            }

            y.Add(0);

            for (int i = y.Count - 2; i >= bestIdx; i--)
            {
                y[i + 1] = y[i];
            }

            y[bestIdx] = z[iMin];

            z[iMin] = z0(y[bestIdx - 1], y[bestIdx]);
            fz[iMin] = fz0(z[iMin], y[bestIdx - 1]);
            z[y.Count - 2] = z0(y[bestIdx], y[bestIdx + 1]);
            fz[y.Count - 2] = fz0(z[y.Count - 2], y[bestIdx]);

        } while (func(y[bestIdx]) - min >= precision && y.Count != MAX_Y);

        return new(y[bestIdx], func(y[bestIdx]));
    }
}
