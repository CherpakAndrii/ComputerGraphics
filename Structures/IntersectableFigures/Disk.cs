using Structures.BaseGeometricalStructures;
using Structures.Interfaces;

namespace Structures.IntersectableFigures;

public class Disk : IIntersectable
{
    public Point Position { get; set; }
    public Vector Normal { get; set; }
    public float Radius { get; set; }

    public Vector GetNormalVector(Point point)
    {
        throw new NotImplementedException();
    }

    public Point? GetIntersectionWith(Ray ray)
    {
        throw new NotImplementedException();
    }

    public bool HasIntersectionWith(Ray ray)
    {
        throw new NotImplementedException();
    }
}