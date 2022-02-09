namespace KDTrees
{
    public interface IClosestPointFindStrategy
    {
        void BuildIndex(MapOfPoints mapOfPoints);
        ClosestPointsAndDistance FindClosestPoints(Point checkPoint);
    }
}
