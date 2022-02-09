namespace KDTrees
{
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
