using System;
using Xunit;
using MMO_Stuff;
using System.Drawing;

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
    }
}
