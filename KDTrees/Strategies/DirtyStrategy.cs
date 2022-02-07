using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTrees.Strategies
{
    /// <summary>
    /// This strategy serves as a verification for other strategies to be right and not as a serious solution to the closest-point-problem.
    /// </summary>
    public class DirtyStrategy : IClosestPointFindStrategy
    {
        private MapOfPoints _mapOfPoints;
        public void Init(MapOfPoints mapOfPoints)
        {
            // This strategy does not prepare anything as it will loop through all points
            _mapOfPoints = mapOfPoints ?? throw new ArgumentNullException(nameof(mapOfPoints));
        }

        public List<Point> FindClosestPoints(Point point)
        {
            return _mapOfPoints.Points
                    .GroupBy(p => p.GetDistanceTo(point))
                    .OrderBy(group => group.Key)
                    .First()
                    .ToList();
        }
    }
}
