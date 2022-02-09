namespace KDTrees
{
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
}
