namespace KDTrees;

public readonly record struct Point(long X, long Y)
{
    public double GetDistanceTo(Point p)
    {
        if (p.X == X && p.Y == Y)
            return 0;
        return Math.Sqrt((X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y));
    }
}