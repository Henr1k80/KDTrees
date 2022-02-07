using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTrees
{
    public class MapOfPoints
    {
        public IReadOnlyList<Point> Points { get; }
        public MapOfPoints(IReadOnlyList<Point> points)
        {
            Points = points;
        }
    }

    public record Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double GetDistanceTo(Point p)
        {
            return Math.Sqrt((X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y));
        }
    }
}
