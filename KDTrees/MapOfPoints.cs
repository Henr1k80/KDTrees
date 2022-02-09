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
        private readonly IEnumerable<Point> _closestPointsEnumerable;
        public HashSet<Point> ClosestPoints => new(_closestPointsEnumerable);

        public double Distance { get; set; }

        public ClosestPointsAndDistance(IEnumerable<Point> closestPoints, double distance)
        {
            _closestPointsEnumerable = closestPoints;
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
