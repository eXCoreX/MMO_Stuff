using System;
using Xunit;
using MMO_Stuff;
using System.Drawing;

namespace AssertExtensions
{
    public static class EqualExtension
    {
        public static void Equal(this string assert, VectorD expected, VectorD actual, int precision)
        {

        }
    }
}

namespace MMO_Stuff.Tests
{
    using AssertExtensions;
    using static AssertExtensions.EqualExtension;
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
        public void TestTooHighPrecisionException()
        {
            Assert.ThrowsAny<Exception>(() => LinearOptimization.GetMinimum(x => Math.Abs(x), 9e-11));
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
    }
}
