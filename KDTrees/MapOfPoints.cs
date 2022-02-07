using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTrees
{
    public class MapOfPoints
    {
        public HashSet<Point> Points { get; }
        public MapOfPoints(HashSet<Point> points)
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
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
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
