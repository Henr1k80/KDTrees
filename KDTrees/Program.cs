// See https://aka.ms/new-console-template for more information
using KDTrees;
using KDTrees.Strategies;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

int pointMaxValue = 1_000_000;

int pointCountOnMap = 10_000_000;
Console.WriteLine($"{DateTime.Now} Creating a list of {pointCountOnMap} random points");
var pointsOnMap = new List<Point>(capacity: pointCountOnMap);
var ran = new Random();
while (pointsOnMap.Count < pointCountOnMap)
{
    pointsOnMap.Add(new Point(x: ran.Next(-pointMaxValue, pointMaxValue), y: ran.Next(-pointMaxValue, pointMaxValue)));
}
var map = new MapOfPoints(pointsOnMap.ToHashSet());

var pointCountToCheck = 10_000;
Console.WriteLine($"{DateTime.Now} Create a random list of {pointCountOnMap} points to check");
var pointsToCheck = new List<Point>();
for (var i = 0; i < pointCountToCheck; i++)
{
    pointsToCheck.Add(new Point(x: ran.Next(-pointMaxValue, pointMaxValue), y: ran.Next(-pointMaxValue, pointMaxValue)));
}

Console.WriteLine($"{DateTime.Now} Building index");
var strategies = new List<IClosestPointFindStrategy>();
//strategies.Add(new DirtyStrategy());
strategies.Add(new TreeStrategy());
var stopwatch = new Stopwatch();
foreach (var strategy in strategies)
{
    stopwatch.Restart();
    strategy.BuildIndex(map);
    Console.WriteLine($"{DateTime.Now} Strategy {strategy.GetType().Name} initiated in {stopwatch.ElapsedMilliseconds}ms");
}

// VALIDATE A given strategy compared to the dirty strategy
//Console.WriteLine("Starting to check same value as dirty");
//while(true){
//    var checkPoint = new Point(x: ran.Next(-pointMaxValue, pointMaxValue), y: ran.Next(-pointMaxValue, pointMaxValue));
//    var result1 = strategies[0].FindClosestPoints(checkPoint);
//    var result2 = strategies[1].FindClosestPoints(checkPoint);
//    if (result1.Distance != result2.Distance)
//        throw new Exception("Not the same distance!");

//    if (result1.ClosestPoints.Count != result2.ClosestPoints.Count)
//        throw new Exception("Not the same point count!");
//    Console.Write(".");
//}

Console.WriteLine($"{DateTime.Now} Starting comparison");
foreach (var strategy in strategies)
{
    Console.WriteLine($"Testing strategy {strategy.GetType().Name}...");
    stopwatch.Restart();
    foreach (var p in pointsToCheck)
    {
        strategy.FindClosestPoints(p);
    }
    Console.WriteLine($"Strategy {strategy.GetType().Name} checked all {pointsToCheck.Count} points in {stopwatch.ElapsedMilliseconds}ms");
}

Console.ReadLine();