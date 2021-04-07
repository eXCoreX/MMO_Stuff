using System;
using Xunit;

namespace MMO_Stuff.Tests
{
    public class UnitTestMMO
    {
        [Fact]
        public void TestAbsAt0Auto()
        {
            var actual = LinearOptimization.GetMinimum(x => Math.Abs(x));

            var expected = new PointD(0, 0);

            Assert.Equal(expected.X, actual.X, 9);
            Assert.Equal(expected.Y, actual.Y, 9);
        }

        [Fact]
        public void TestAbsAt1Auto()
        {
            var actual = LinearOptimization.GetMinimum(x => Math.Abs(x - 1), 1e-10);

            var expected = new PointD(1, 0);

            Assert.Equal(expected.X, actual.X, 9);
            Assert.Equal(expected.Y, actual.Y, 9);
        }

        [Fact]
        public void TestAbsAt1GoldenRatio()
        {
            var actual = LinearOptimization.GetMinimum(x => Math.Abs(x - 1), 1e-10, LinearOptimization.LinearMethod.GoldenRatio);

            var expected = new PointD(1, 0);

            Assert.Equal(expected.X, actual.X, 9);
            Assert.Equal(expected.Y, actual.Y, 9);
        }

        [Fact]
        public void TestParabolaAt0Auto()
        {
            var actual = LinearOptimization.GetMinimum(x => Math.Pow(x, 2));

            var expected = new PointD(0, 0);

            Assert.Equal(expected.X, actual.X, 7);
            Assert.Equal(expected.Y, actual.Y, 7);
        }

        [Fact]
        public void TestParabolaAt1Auto()
        {
            var actual = LinearOptimization.GetMinimum(x => Math.Pow(x - 1, 2));

            var expected = new PointD(1, 0);

            Assert.Equal(expected.X, actual.X, 7);
            Assert.Equal(expected.Y, actual.Y, 7);
        }

        [Fact]
        public void TestParabolaAt1_1Auto()
        {
            var actual = LinearOptimization.GetMinimum(x => 1 + Math.Pow(x - 1, 2));

            var expected = new PointD(1, 1);

            Assert.Equal(expected.X, actual.X, 7);
            Assert.Equal(expected.Y, actual.Y, 7);
        }

        [Fact]
        [Trait("X =", "-5, 5")]
        public void TestLinearEquation()
        {
            var actual = LinearOptimization.GetMinimumGoldenRatio(x => 1 + (x - 2) / 2, -5, 5, 1e-7);

            var expected = new PointD(-5, -2.5);

            Assert.Equal(expected.X, actual.X, 7);
            Assert.Equal(expected.Y, actual.Y, 7);
        }

        [Fact]
        [Trait("X =", "-5, 5")]
        public void TestNegativeLinearEquation()
        {
            var actual = LinearOptimization.GetMinimumGoldenRatio(x => 1 - (x - 2) / 2, -5, 5, 1e-7);

            var expected = new PointD(5, -0.5);

            Assert.Equal(expected.X, actual.X, 7);
            Assert.Equal(expected.Y, actual.Y, 7);
        }

        [Fact]
        public void Test3DParaboloidAt0Auto()
        {
            var actual = DimensionalOptimization.GetMinimumWithGradient(x =>
            {
                return x[0] * x[0] + x[1] * x[1];
            }, x =>
            {
                double[] coords = { 2 * x[0], 2 * x[1] };
                return new VectorD(coords);
            }, 2);

            var expected = (X: new VectorD(2), F: 0);

            for (int i = 0; i < actual.X.N; i++)
            {
                Assert.Equal(expected.X[i], actual.X[i], 7);
            }
            Assert.Equal(expected.F, actual.F, 7);
        }

        [Fact]
        public void Test3DParaboloidAt1_1Auto()
        {
            var actual = DimensionalOptimization.GetMinimumWithGradient(x =>
            {
                return (x[0] - 1) * (x[0] - 1) + (x[1] - 1) * (x[1] - 1);
            }, x =>
            {
                double[] coords = { 2 * (x[0] - 1), 2 * (x[1] - 1) };
                return new VectorD(coords);
            }, 2);

            var expected = (X: new VectorD(2, 1), F: 0);

            for (int i = 0; i < actual.X.N; i++)
            {
                Assert.Equal(expected.X[i], actual.X[i], 7);
            }
            Assert.Equal(expected.F, actual.F, 7);
        }

        [Fact]
        public void Test3DParaboloidAt1_1_1Auto()
        {
            var actual = DimensionalOptimization.GetMinimumWithGradient(x =>
            {
                return 1 + (x[0] - 1) * (x[0] - 1) + (x[1] - 1) * (x[1] - 1);
            }, x =>
            {
                double[] coords = { 2 * (x[0] - 1), 2 * (x[1] - 1) };
                return new VectorD(coords);
            }, 2);

            var expected = (X: new VectorD(2, 1), F: 1);

            for (int i = 0; i < actual.X.N; i++)
            {
                Assert.Equal(expected.X[i], actual.X[i], 7);
            }
            Assert.Equal(expected.F, actual.F, 7);
        }

        [Fact]
        public void Test3DParaboloidAt1_1_1StepDivision()
        {
            var actual = DimensionalOptimization.GetMinimumWithGradient(x =>
            {
                return 1 + (x[0] - 1) * (x[0] - 1) + (x[1] - 1) * (x[1] - 1);
            }, x =>
            {
                double[] coords = { 2 * (x[0] - 1), 2 * (x[1] - 1) };
                return new VectorD(coords);
            }, 2, 1e-7, DimensionalOptimization.GradientMethod.StepDivision);

            var expected = (X: new VectorD(2, 1), F: 1);

            for (int i = 0; i < actual.X.N; i++)
            {
                Assert.Equal(expected.X[i], actual.X[i], 7);
            }
            Assert.Equal(expected.F, actual.F, 7);
        }

        [Fact]
        public void Test3DParaboloidAt3_m1_2StepDivision()
        {
            var actual = DimensionalOptimization.GetMinimumWithGradient(x =>
            {
                return 2 + (x[0] - 3) * (x[0] - 3) + (x[1] + 1) * (x[1] + 1);
            }, x =>
            {
                double[] coords = { 2 * (x[0] - 3), 2 * (x[1] + 1) };
                return new VectorD(coords);
            }, 2, 1e-7, DimensionalOptimization.GradientMethod.StepDivision);

            var expected = (X: new VectorD(new double[] { 3, -1 }), F: 2);

            for (int i = 0; i < actual.X.N; i++)
            {
                Assert.Equal(expected.X[i], actual.X[i], 7);
            }
            Assert.Equal(expected.F, actual.F, 7);
        }

        [Fact]
        public void Test3DParaboloidAt1_m1_2WithoutGradient()
        {
            var actual = DimensionalOptimization.GetMinimum(x =>
            {
                return 2 + (x[0] - 1) * (x[0] - 1) + (x[1] + 1) * (x[1] + 1);
            }, 2);

            var expected = (X: new VectorD(new double[] { 1, -1 }), F: 2);

            for (int i = 0; i < actual.X.N; i++)
            {
                Assert.Equal(expected.X[i], actual.X[i], 7);
            }
            Assert.Equal(expected.F, actual.F, 7);
        }

        [Theory]
        [InlineData(-1, -2, -1, 3)]
        [InlineData(0, 1, 3, 1)]
        [InlineData(0, 0, 0, 0)]
        [InlineData(-1.4, 1.1, 0, -20.2)]
        [InlineData(2, 1.41, 3, 11.1)]
        public void Test4DParaboloidWithoutGradient(double x1, double x2, double x3, double y)
        {
            var actual = DimensionalOptimization.GetMinimum(x =>
            {
                return y + (x[0] - x1) * (x[0] - x1) + (x[1] - x2) * (x[1] - x2) + (x[2] - x3) * (x[2] - x3);
            }, 3);

            var expected = (X: new VectorD(new double[] { x1, x2, x3 }), F: y);

            for (int i = 0; i < actual.X.N; i++)
            {
                Assert.Equal(expected.X[i], actual.X[i], 7);
            }
            Assert.Equal(expected.F, actual.F, 7);
        }

        [Theory]
        [InlineData(-1, 3)]
        [InlineData(0, 1)]
        [InlineData(0, 0)]
        [InlineData(-1.4, -20.2)]
        [InlineData(2, 11.1)]
        public void Test2DParabolaWithoutGradient(double x1, double y)
        {
            var actual = DimensionalOptimization.GetMinimum(x =>
            {
                return y + (x[0] - x1) * (x[0] - x1);
            }, 1);

            var expected = (X: new VectorD(new double[] { x1 }), F: y);

            for (int i = 0; i < actual.X.N; i++)
            {
                Assert.Equal(expected.X[i], actual.X[i], 7);
            }
            Assert.Equal(expected.F, actual.F, 7);
        }
    }

    public class UnitTestExceptionsMMO
    {
        [Fact]
        public void TestTooHighLinearPrecisionException()
        {
            Assert.ThrowsAny<Exception>(() => LinearOptimization.GetMinimum(x => x, 9e-11));
            Assert.ThrowsAny<Exception>(() => LinearOptimization.GetMinimumDichotomy(x => x, 0, 0, 9e-11));
            Assert.ThrowsAny<Exception>(() => LinearOptimization.GetMinimumGoldenRatio(x => x, 0, 0, 9e-11));
        }

        [Fact]
        public void TestTooHighDimensionalPrecisionException()
        {
            Assert.ThrowsAny<Exception>(() => DimensionalOptimization.GetMinimumWithGradient(x => x[0], x => x, 1, 9e-11));
            Assert.ThrowsAny<Exception>(() => DimensionalOptimization.GetMinimumFastDescent(x => x[0], x => x, 1, 9e-11));
            Assert.ThrowsAny<Exception>(() => DimensionalOptimization.GetMinimumStepDivision(x => x[0], x => x, 1, 0.5, 9e-11));
            Assert.ThrowsAny<Exception>(() => DimensionalOptimization.GetMinimum(x => x[0], 1, 9e-11));
            Assert.ThrowsAny<Exception>(() => DimensionalOptimization.GetMinimumCoordinateDescent(x => x[0], 1, 0.5, 9e-11));
        }

        [Fact]
        public void TestBadLambdaException()
        {
            Assert.ThrowsAny<Exception>(() => DimensionalOptimization.GetMinimumStepDivision(x => x[0], x => x, 1, -1, 9e-11));
            Assert.ThrowsAny<Exception>(() => DimensionalOptimization.GetMinimumStepDivision(x => x[0], x => x, 1, 1, 9e-11));
            Assert.ThrowsAny<Exception>(() => DimensionalOptimization.GetMinimumCoordinateDescent(x => x[0], 1, -1, 9e-11));
            Assert.ThrowsAny<Exception>(() => DimensionalOptimization.GetMinimumCoordinateDescent(x => x[0], 1, 1, 9e-11));
        }
    }
}
