using KDTrees.Strategies;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KDTrees.Test
{
    public class DirtyStrategyTest
    {
        [Fact]
        public void FindClosestPoints()
        {
            var points = new List<Point>(){
                new Point( 0,0 ),
                new Point( -10,0 ),
                new Point( 10,0 ),
                new Point( 0,-10 ),
                new Point( 0,10 ),
                new Point( 10,10 ),
                new Point( 12,12 ),
                new Point( -10,-10 ),
                
                new Point( -5,0 ),
                new Point( 0,-5 ),
                new Point( 5,0 ),
                new Point( 0,5 ),
                new Point( 5,5 ),
                new Point( -5,-5 )
            };
            var mapOfPoints = new MapOfPoints(points);
            var strategy = new DirtyStrategy();
            strategy.Init(mapOfPoints);

            Assert.Equal(new Point(0, 0), strategy.FindClosestPoints(new Point(0, 0)).Single());
            Assert.Equal(new Point(0, 0), strategy.FindClosestPoints(new Point(1, 0)).Single());
            Assert.Equal(new Point(0, 0), strategy.FindClosestPoints(new Point(-1, 0)).Single());
            Assert.Equal(new Point(0, 0), strategy.FindClosestPoints(new Point(0, 1)).Single());
            Assert.Equal(new Point(0, 0), strategy.FindClosestPoints(new Point(0, -1)).Single());

            Assert.Equal(new Point(10, 10), strategy.FindClosestPoints(new Point(9, 9)).Single());

            var result = strategy.FindClosestPoints(new Point(11, 11));
            Assert.Contains(new Point(10, 10), result);
            Assert.Contains(new Point(12, 12), result);
            Assert.Equal(2, result.Count);
        }
    }
}