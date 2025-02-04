namespace KDTrees.Strategies
{
    public class TreeStrategy : IClosestPointFindStrategy
    {
        private TreeNode _rootNode = null;
        private static readonly Comparison<Point> SortByX = (p1, p2) => p1.X > p2.X ? 1 : p1.X < p2.X ? -1 : 0;
        private static readonly Comparison<Point> SortByY = (p1, p2) => p1.Y > p2.Y ? 1 : p1.Y < p2.Y ? -1 : 0;

        public ClosestPointsAndDistance FindClosestPoints(Point checkPoint)
        {
            if(_rootNode == null)
            {
                throw new InvalidOperationException($"Must call {nameof(BuildIndex)} prior to calling {nameof(FindClosesPoint)}");
            }
            return FindClosesPoint(checkPoint: checkPoint, treeNode: _rootNode, closestSoFar: new ClosestPointsAndDistance(closestPoints: ArraySegment<Point>.Empty, distance: double.MaxValue));
        }

        private static ClosestPointsAndDistance GetClosestorMergeIfEqualDistance(ClosestPointsAndDistance closestSoFar, ClosestPointsAndDistance challenger){
            if (closestSoFar.Distance < challenger.Distance)
                return closestSoFar;
            if (closestSoFar.Distance > challenger.Distance)
                return challenger;

            return new ClosestPointsAndDistance(closestPoints: closestSoFar.ClosestPoints.Concat(challenger.ClosestPoints), distance: closestSoFar.Distance);
        }

        private static ClosestPointsAndDistance FindClosesPoint(Point checkPoint, TreeNode treeNode, ClosestPointsAndDistance closestSoFar)
        {
            {
                var distanceToMidPoint = checkPoint.GetDistanceTo(treeNode.MidPoint);

                // If node point is closer than closest point so far, decreased the max distance value
                if (distanceToMidPoint <= closestSoFar.Distance)
                    closestSoFar = GetClosestorMergeIfEqualDistance(closestSoFar, new ClosestPointsAndDistance(closestPoints: new [] { treeNode.MidPoint }, distance: distanceToMidPoint));
            }

            if (treeNode.SplitOnAxis == Axis.X)
            {
                if (checkPoint.X <= treeNode.MidPoint.X)
                {
                    // X less than midPoint X
                    if (treeNode.Less != null)
                    {
                        closestSoFar = GetClosestorMergeIfEqualDistance(closestSoFar, FindClosesPoint(checkPoint: checkPoint, treeNode: treeNode.Less, closestSoFar: closestSoFar));
                    }

                    if (checkPoint.X + closestSoFar.Distance >= treeNode.MidPoint.X)
                    {
                        // There is a chance that the closest point os on the greater side
                        if (treeNode.Greater != null)
                        {
                            closestSoFar = GetClosestorMergeIfEqualDistance(closestSoFar, FindClosesPoint(checkPoint: checkPoint, treeNode: treeNode.Greater, closestSoFar: closestSoFar));
                        }
                    }
                }

                if (checkPoint.X >= treeNode.MidPoint.X)
                {
                    // X greater than midPoint X
                    // TODO: Optimize branch
                    if (treeNode.Greater != null)
                    {
                        closestSoFar = GetClosestorMergeIfEqualDistance(closestSoFar, FindClosesPoint(checkPoint: checkPoint, treeNode: treeNode.Greater, closestSoFar: closestSoFar));
                    }

                    if (checkPoint.X - closestSoFar.Distance <= treeNode.MidPoint.X)
                    {
                        // There is a chance that the closest point os on the less side
                        if (treeNode.Less != null)
                        {
                            closestSoFar = GetClosestorMergeIfEqualDistance(closestSoFar, FindClosesPoint(checkPoint: checkPoint, treeNode: treeNode.Less, closestSoFar: closestSoFar));
                        }
                    }
                }
            }
            else
            {
                if (checkPoint.Y <= treeNode.MidPoint.Y)
                {
                    // Y less than midPoint Y
                    if (treeNode.Less != null)
                    {
                        closestSoFar = GetClosestorMergeIfEqualDistance(closestSoFar, FindClosesPoint(checkPoint: checkPoint, treeNode: treeNode.Less, closestSoFar: closestSoFar));
                    }

                    if (checkPoint.Y + closestSoFar.Distance >= treeNode.MidPoint.Y)
                    {
                        // There is a chance that the closest point os on the greater side
                        if (treeNode.Greater != null)
                        {
                            closestSoFar = GetClosestorMergeIfEqualDistance(closestSoFar, FindClosesPoint(checkPoint: checkPoint, treeNode: treeNode.Greater, closestSoFar: closestSoFar));
                        }
                    }
                }

                if (checkPoint.Y >= treeNode.MidPoint.Y)
                {
                    // Y greater than midPoint Y
                    if (treeNode.Greater != null)
                    {
                        closestSoFar = GetClosestorMergeIfEqualDistance(closestSoFar, FindClosesPoint(checkPoint: checkPoint, treeNode: treeNode.Greater, closestSoFar: closestSoFar));
                    }

                    if (checkPoint.Y - closestSoFar.Distance <= treeNode.MidPoint.Y)
                    {
                        // There is a chance that the closest point os on the less side
                        if (treeNode.Less != null)
                        {
                            closestSoFar = FindClosesPoint(checkPoint: checkPoint, treeNode: treeNode.Less, closestSoFar: closestSoFar);
                        }
                    }
                }

            }

            return closestSoFar;
        }

        public void BuildIndex(MapOfPoints mapOfPoints)
        {
            _rootNode = BuildNode(mapOfPoints.Points, axis: Axis.X);
        }

        private static TreeNode BuildNode(Span<Point> points, Axis axis)
        {
            if (points.Length == 0)
                throw new ArgumentException("Empty points");

            if (points.Length == 1)
                return new TreeNode(splitOnAxis: axis, nodePoint: points[0], less: null, greater: null);

            Comparison<Point> sortBy;
            Axis oppositeAxis;
            if (axis == Axis.X)
            {
                sortBy = SortByX;
                oppositeAxis = Axis.Y;
            }
            else
            {
                sortBy = SortByY;
                oppositeAxis = Axis.X;
            }
            // this is single threaded, so we can reorder parts of the same array and avoid a lot of allocations
            points.Sort(sortBy);
            var midIndex = (points.Length + 1) / 2;
            var less = points[..midIndex];
            var greater = points[(midIndex + 1)..];

            return new TreeNode(
                splitOnAxis: axis,
                nodePoint: points[midIndex],
                less: less.Length > 0 ? BuildNode(points: less, axis: oppositeAxis) : null,
                greater: greater.Length > 0 ? BuildNode(points: greater, axis: oppositeAxis) : null);
        }

        public enum Axis
        {
            X, Y
        }

        public class TreeNode
        {
            public Axis SplitOnAxis { get; set; }
            public Point MidPoint { get; set; }
            public TreeNode? Less { get; set; }
            public TreeNode? Greater { get; set; }

            public TreeNode(Axis splitOnAxis, Point nodePoint, TreeNode? less, TreeNode? greater)
            {
                SplitOnAxis = splitOnAxis;
                MidPoint = nodePoint;
                Less = less;
                Greater = greater;
            }
        }
    }
}