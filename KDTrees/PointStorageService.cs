using System.Text;

namespace KDTrees
{
    public class PointStorageService
    {
        public void WriteMapPoints(IEnumerable<Point> points)
        {
            WriteToFile(points: points, filePath: FilePaths.MapPointsFilePath);
        }

        public IEnumerable<Point> ReadMapPoints()
        {
            return ReadFromFile(filepath: FilePaths.MapPointsFilePath);
        }

        public void WriteCheckPoints(IEnumerable<Point> points)
        {
            WriteToFile(points: points, filePath: FilePaths.CheckPointsFilePath);
        }

        public IEnumerable<Point> ReadCheckPoints()
        {
            return ReadFromFile(filepath: FilePaths.CheckPointsFilePath);
        }

        private static IEnumerable<Point> ReadFromFile(string filepath)
        {
            using (var reader = new StreamReader(filepath))
            {
                var line = reader.ReadLine();
                while(line != null){
                    var mid = line.IndexOf(" ");
                    var x = int.Parse(line.AsSpan().Slice(0, mid));
                    var y = int.Parse(line.AsSpan().Slice(mid + 1));
                    yield return new Point(x, y);
                    line = reader.ReadLine();
                }
            }
        }

        private static void WriteToFile(IEnumerable<Point> points, string filePath)
        {
            using (var w = new StreamWriter(path: filePath, append: false, encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)))
            {
                foreach (var p in points)
                {
                    w.WriteLine(p.X + " " + p.Y);
                }
            }
        }

    }
}
