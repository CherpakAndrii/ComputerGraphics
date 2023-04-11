using System.Globalization;
using Structures.BaseGeometricalStructures;
using Structures.Interfaces;
using Structures.IntersectableFigures;

namespace Core.ObjFileReader;

public class ObjFileReader
{
    /// <summary>
    /// Parsing triangles from .obj-file content
    /// </summary>
    /// <param name="fileData">array of lines from file</param>
    /// <returns>the list of figures or null if given file contained some errors</returns>
    public List<IIntersectable>? GetStructuresFromFile(string[] fileData)
    {
        List<string> v = new List<string>();
        List<string> f = new List<string>();
        foreach (var line in fileData)
        {
            (line[..2] switch
            {
                "v " => v,
                "f " => f,
                _ => null
            })?.Add(line);
        }

        Point[] points = new Point[v.Count];
        List<IIntersectable> figures = new List<IIntersectable>();

        for (int i = 0; i < v.Count; i++)
        {
            var splited = v[i].Split();
            if (splited.Length != 4 || !float.TryParse(splited[1], CultureInfo.InvariantCulture, out float x)
                                    || !float.TryParse(splited[2], CultureInfo.InvariantCulture, out float y) 
                                    || !float.TryParse(splited[3], CultureInfo.InvariantCulture, out float z)) return null;
            points[i] = new Point(x, y, z);
        }

        foreach (var figureString in f)
        {
            var splited = figureString.Split();
            if (splited.Length != 4 || !int.TryParse(splited[1].Split('/')[0], out int a)
                                    || !int.TryParse(splited[2].Split('/')[0], out int b) 
                                    || !int.TryParse(splited[3].Split('/')[0], out int c)
                                    || a < 1 || b < 1 || c < 1 || a > points.Length || b > points.Length
                                    || c > points.Length) return null;
            figures.Add(new Triangle(points[a - 1], points[b - 1], points[c - 1]));
        }
        
        return figures;
    }
}