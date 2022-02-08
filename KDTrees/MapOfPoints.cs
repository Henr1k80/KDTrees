using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTrees
{
    public class MapOfPoints
    {
        public Point[] Points { get; }
        public MapOfPoints(Point[] points)
        {
            Points = points;
        }
    }

    public class ClosestPointsAndDistance
    {
        public HashSet<Point> ClosestPoints { get; set; }
        public double Distance { get; set; }

        public ClosestPointsAndDistance(HashSet<Point> closestPoints, double distance)
        {
            ClosestPoints = closestPoints;
            Distance = distance;
        }
    }

    public record struct Point
    {
        public long X { get; set; }
        public long Y { get; set; }

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }

        public double GetDistanceTo(Point p)
        {
            if (p.X == X && p.Y == Y)
                return 0;
            return Math.Sqrt((X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y));
        }
    }
}
