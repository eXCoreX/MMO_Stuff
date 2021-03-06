using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MMO_Stuff;

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

    #region Gradient Methods

    /// <summary>
    /// Get minimum of unimodal function func using given gradient function
    /// </summary>
    /// <param name="func"> Unimodal function </param>
    /// <param name="gradient"> Gradient function of func </param>
    /// <param name="dimensions"> Number of dimensions in argument </param>
    /// <param name="initialPoint"> Point from which the method search the minimum. Defaults to vector of zeroes. </param>
    /// <param name="precision"> Epsilon for fp methods </param>
    /// <param name="method"> Method of computing minumum </param>
    /// <returns> VectorD of argument coordinates and value of func at this point </returns>
    public static (VectorD X, double F) GetMinimumWithGradient(
        Func<VectorD, double> func,
        Func<VectorD, VectorD> gradient,
        int dimensions,
        VectorD initialPoint = null,
        double precision = 1e-7,
        GradientMethod method = GradientMethod.FastDescent)
    {
        if (initialPoint is null)
        {
            initialPoint = new(dimensions);
        }

        if (precision < 1e-10)
        {
            throw new ArgumentException("Precision is too high");
        }
        else
        {
            return method switch
            {
                GradientMethod.FastDescent => GetMinimumFastDescent(func, gradient, dimensions, initialPoint, precision),
                GradientMethod.StepDivision => GetMinimumStepDivision(func, gradient, dimensions, initialPoint, 0.5, precision),
                _ => throw new ArgumentException("Not valid method"),
            };
        }
    }

    public static (VectorD X, double F) GetMinimumFastDescent(
        Func<VectorD, double> func,
        Func<VectorD, VectorD> gradient,
        int dimensions,
        VectorD initialPoint,
        double eps)
    {
        if (eps < 0)
        {
            eps = -eps;
        }
        if (eps < 1e-10)
        {
            throw new ArgumentException("Precision is too high");
        }
        if (initialPoint.N != dimensions)
        {
            throw new ArgumentException("Initial point's dimensions should be the same as func's and dimension's params.");
        }

        VectorD x0 = initialPoint;

        VectorD g = gradient(x0);

        if (g.Norm > eps)
        {
            VectorD x;
            do
            {
                x = (VectorD)x0.Clone();
                double h = FindStepForFastDescent(func, gradient, x, g, eps);
                x0 = x - h * g;
                g = gradient(x0);
            } while ((x - x0).Norm >= eps && g.Norm >= eps);
        }

        return (x0, func(x0));
    }

    private static double FindStepForFastDescent(
        Func<VectorD, double> func,
        Func<VectorD, VectorD> gradient,
        VectorD x,
        VectorD g,
        double eps)
    {
        var result = LinearOptimization.GetMinimum((double step) =>
        {
            return func(x - g * step);
        }, eps);
        return result.X;
    }

    public static (VectorD X, double F) GetMinimumStepDivision(
        Func<VectorD, double> func,
        Func<VectorD, VectorD> gradient,
        int dimensions,
        VectorD initialPoint,
        double lambda,
        double eps)
    {
        if (eps < 0)
        {
            eps = -eps;
        }
        if (eps < 1e-10)
        {
            throw new ArgumentException("Precision is too high");
        }
        if (lambda <= 0 || lambda >= 1)
        {
            throw new ArgumentException("lambda should be in range (0, 1)");
        }
        if (initialPoint.N != dimensions)
        {
            throw new ArgumentException("Initial point's dimensions should be the same as func's and dimension's params.");
        }

        VectorD x0 = initialPoint;

        VectorD g = gradient(x0);
        double h = 1;

        if (g.Norm > eps)
        {
            VectorD x;
            do
            {
                x = (VectorD)x0.Clone();
                var fx = func(x);
                double f0;
                do
                {
                    x0 = x - h * g;
                    f0 = func(x0);
                    if (f0 - fx > -lambda * h * Math.Pow(g.Norm, 2))
                    {
                        h = lambda * h;
                    }
                } while (f0 - fx > -lambda * h * Math.Pow(g.Norm, 2) && h > eps / 2);
                g = gradient(x0);
            } while ((x - x0).Norm >= eps && g.Norm >= eps);
        }

        return (x0, func(x0));
    }
    #endregion

    #region Non Gradient Methods

    /// <summary>
    /// Get minimum of unimodal function func without gradient function
    /// </summary>
    /// <param name="func"> Unimodal function </param>
    /// <param name="dimensions"> Number of dimensions in the argument </param>
    /// <param name="initialPoint"> Point from which the method search the minimum. Defaults to vector of zeroes. </param>
    /// <param name="precision"> Epsilon for fp methods </param>
    /// <param name="method"> Method of computing minumum </param>
    /// <returns> VectorD of argument coordinates and value of func at this point </returns>
    public static (VectorD X, double F) GetMinimum(
        Func<VectorD, double> func,
        int dimensions,
        VectorD initialPoint = null,
        double precision = 1e-7,
        NonGradientMethod method = NonGradientMethod.CoordinateDescent)
    {
        if (initialPoint is null)
        {
            initialPoint = new(dimensions);
        }

        if (precision < 1e-10)
        {
            throw new ArgumentException("Precision is too high");
        }
        else
        {
            return method switch
            {
                NonGradientMethod.CoordinateDescent => GetMinimumCoordinateDescent(func, dimensions, initialPoint, 0.8, precision),
                _ => throw new ArgumentException("Not valid method"),
            };
        }
    }

    public static (VectorD X, double F) GetMinimumCoordinateDescent(
        Func<VectorD, double> func,
        int dimensions,
        VectorD initialPoint,
        double lambda,
        double eps)
    {
        if (eps < 0)
        {
            eps = -eps;
        }
        if (eps < 1e-10)
        {
            throw new ArgumentException("Precision is too high");
        }
        if (initialPoint.N != dimensions)
        {
            throw new ArgumentException("Initial point's dimensions should be the same as func's and dimension's params.");
        }

        VectorD x0 = initialPoint;
        VectorD h = new VectorD(dimensions, 8.8005553535);

        VectorD x_int = (VectorD)x0.Clone(), x_ext;

        do
        {
            x_ext = (VectorD)x_int.Clone();
            for (int i = 0; i < dimensions; i++)
            {
                double xi = x_int[i];
                double fx = func(x_int);
                VectorD y1 = (VectorD)x_int.Clone();
                y1[i] += 3 * eps;
                VectorD y2 = (VectorD)x_int.Clone();
                y2[i] -= 3 * eps;
                double f1 = func(y1);
                double f2 = func(y2);
                int sign = Math.Sign(f2 - f1);
                double fx1;
                do
                {
                    x_int[i] = xi + h[i] * sign;
                    fx1 = func(x_int);
                    if (fx1 >= fx)
                    {
                        h[i] *= lambda;
                    }
                } while (fx1 >= fx && h[i] >= eps / 2);
            }
        } while ((x_int - x_ext).Norm >= eps / 2);

        return (x_int, func(x_int));
    }

    #endregion
}
