using System;
using System.Drawing;

namespace Contemplation
{
    public static class ProportionsHelper
    {
        public static Point Contain(Point source, Point destination)
        {
            var scaleX = (double)destination.X / source.X;
            var scaleY = (double)destination.Y / source.Y;
            var scale = Math.Min(scaleX, scaleY);
            var result = new Point(
               Convert.ToInt32(Math.Round(source.X * scale, MidpointRounding.AwayFromZero)),
               Convert.ToInt32(Math.Round(source.Y * scale, MidpointRounding.AwayFromZero))
                );
            return result;
        }

        public static Point Cover(Point source, Point destination)
        {
            var scaleX = (double)destination.X / source.X;
            var scaleY = (double)destination.Y / source.Y;
            var scale = Math.Max(scaleX, scaleY);
            var result = new Point(
               Convert.ToInt32(Math.Round(source.X * scale, MidpointRounding.AwayFromZero)),
               Convert.ToInt32(Math.Round(source.Y * scale, MidpointRounding.AwayFromZero))
                );
            return result;
        }
    }
}
