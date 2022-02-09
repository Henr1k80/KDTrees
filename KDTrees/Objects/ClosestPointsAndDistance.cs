namespace KDTrees;

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