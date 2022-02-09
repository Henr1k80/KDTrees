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

    public readonly record struct Point(long X, long Y)
    {
        public double GetDistanceTo(Point p)
        {
            if (p.X == X && p.Y == Y)
                return 0;
            return Math.Sqrt((X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y));
        }
    }
}
