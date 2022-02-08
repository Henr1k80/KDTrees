// See https://aka.ms/new-console-template for more information
using KDTrees;
using System.Text;

Console.WriteLine("Hello, World!");

var service = new PointStorageService();
service.WriteMapPoints(points: GenerateRandomUniquePoints(pointCount: 5_000_000));
service.WriteCheckPoints(points: GenerateRandomUniquePoints(pointCount: 10_000));

static HashSet<Point> GenerateRandomUniquePoints(int pointCount)
{
    int pointMaxValue = 1_000_000;
    var pointsToCheck = new HashSet<Point>();
    var ran = new Random();
    while (pointsToCheck.Count < pointCount)
    {
        pointsToCheck.Add(new Point(x: ran.Next(-pointMaxValue, pointMaxValue), y: ran.Next(-pointMaxValue, pointMaxValue)));
    }
    return pointsToCheck;
}
