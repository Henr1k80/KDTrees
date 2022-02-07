using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTrees.Strategies
{
    public class TreeStrategy : IClosestPointFindStrategy
    {
        private TreeNode _rootNode = null;
        public ClosestPointsAndDistance FindClosestPoints(Point checkPoint)
        {
            return FindClosesPoint(checkPoint: checkPoint, treeNode: _rootNode, closestSoFar: new ClosestPointsAndDistance(closestPoints: new HashSet<Point>(), distance: double.MaxValue));
        }

        private static ClosestPointsAndDistance GetClosestorMergeIfEqualDistance(ClosestPointsAndDistance closestSoFar, ClosestPointsAndDistance challenger){
            if (closestSoFar.Distance < challenger.Distance)
                return closestSoFar;
            if (closestSoFar.Distance > challenger.Distance)
                return challenger;

            return new ClosestPointsAndDistance(closestPoints: closestSoFar.ClosestPoints.Concat(challenger.ClosestPoints).ToHashSet(), distance: closestSoFar.Distance);
        }

        private ClosestPointsAndDistance FindClosesPoint(Point checkPoint, TreeNode treeNode, ClosestPointsAndDistance closestSoFar)
        {
            if(_rootNode == null)
            {
                throw new InvalidOperationException($"Must call {nameof(BuildIndex)} prior to calling {nameof(FindClosesPoint)}");
            }

            {
                var distanceToMidPoint = checkPoint.GetDistanceTo(treeNode.MidPoint);

                // If node point is closer than closest point so far, decreased the max distance value
                if (distanceToMidPoint <= closestSoFar.Distance)
                    closestSoFar = GetClosestorMergeIfEqualDistance(closestSoFar, new ClosestPointsAndDistance(closestPoints: new HashSet<Point>() { treeNode.MidPoint }, distance: distanceToMidPoint));
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
            _rootNode = BuildNode(points: mapOfPoints.Points.ToList(), axis: Axis.X);
        }

        private TreeNode BuildNode(IReadOnlyList<Point> points, Axis axis)
        {
            if (points.Count == 0)
                throw new ArgumentException("Empty points");

            if (points.Count == 1)
                return new TreeNode(splitOnAxis: axis, nodePoint: points.Single(), less: null, greater: null);

            var reversedAxis = axis == Axis.X ? Axis.Y : Axis.X;
            var ordered = points.OrderBy(p => axis == Axis.X ? p.X : p.Y).ToList();
            var midIndex = (ordered.Count + 1) / 2;

            var less = ordered.Take(midIndex).ToList();
            var greater = ordered.Skip(midIndex + 1).ToList();

            return new TreeNode(
                splitOnAxis: axis,
                nodePoint: ordered[midIndex],
                less: less.Any() ? BuildNode(points: less, axis: reversedAxis) : null,
                greater: greater.Any() ? BuildNode(points: greater, axis: reversedAxis) : null);
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
