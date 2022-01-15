using System.Drawing;
using Xunit;

namespace Contemplation.Tests
{
    public class ProportionsHelperTests
    {
        // Contain

        [Fact]
        public void ContainSameSize()
        {
            var source = new Point(10, 20);
            var destination = new Point(10, 20);

            var result = ProportionsHelper.Contain(source, destination);

            Assert.Equal(new Point(10, 20), result);
        }

        [Fact]
        public void ContainEqualScaleEnlarge()
        {
            var source = new Point(10, 20);
            var destination = new Point(20, 40);

            var result = ProportionsHelper.Contain(source, destination);

            Assert.Equal(new Point(20, 40), result);
        }

        [Fact]
        public void ContainEqualScaleShrink()
        {
            var source = new Point(10, 20);
            var destination = new Point(5, 10);

            var result = ProportionsHelper.Contain(source, destination);

            Assert.Equal(new Point(5, 10), result);
        }

        [Fact]
        public void ContainScaleX()
        {
            var source = new Point(10, 20);
            var destination = new Point(20, 50);

            var result = ProportionsHelper.Contain(source, destination);

            Assert.Equal(new Point(20, 40), result);
        }

        [Fact]
        public void ContainScaleY()
        {
            var source = new Point(10, 20);
            var destination = new Point(30, 40);

            var result = ProportionsHelper.Contain(source, destination);

            Assert.Equal(new Point(20, 40), result);
        }

        // Cover

        [Fact]
        public void CoverSameSize()
        {
            var source = new Point(10, 20);
            var destination = new Point(10, 20);

            var result = ProportionsHelper.Cover(source, destination);

            Assert.Equal(new Point(10, 20), result);
        }

        [Fact]
        public void CoverEqualScaleEnlarge()
        {
            var source = new Point(10, 20);
            var destination = new Point(20, 40);

            var result = ProportionsHelper.Cover(source, destination);

            Assert.Equal(new Point(20, 40), result);
        }

        [Fact]
        public void CoverEqualScaleShrink()
        {
            var source = new Point(10, 20);
            var destination = new Point(5, 10);

            var result = ProportionsHelper.Cover(source, destination);

            Assert.Equal(new Point(5, 10), result);
        }

        [Fact]
        public void CoverScaleX()
        {
            var source = new Point(10, 20);
            var destination = new Point(15, 40);

            var result = ProportionsHelper.Cover(source, destination);

            Assert.Equal(new Point(20, 40), result);
        }

        [Fact]
        public void CoverScaleY()
        {
            var source = new Point(10, 20);
            var destination = new Point(20, 35);

            var result = ProportionsHelper.Cover(source, destination);

            Assert.Equal(new Point(20, 40), result);
        }
    }
}
