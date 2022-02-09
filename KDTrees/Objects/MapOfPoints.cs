using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
