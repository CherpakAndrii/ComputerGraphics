using Structures.BaseGeometricalStructures;
using Structures.Interfaces;

namespace Structures.IntersectableFigures;

public class Sphere : IIntersectable
{
    public Point Center { get; protected set; }
    public float Radius { get; protected set; }

    public Sphere(Point center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public Sphere(Sphere original)
    {
        Center = original.Center;
        Radius = original.Radius;
    }

    public Vector GetNormalVector(Point point)
    {
        return new Vector(Center, point).Normalized();
    }

    public bool HasIntersectionWith(Ray ray, out Point intersectionPoint)
    {
        throw new NotImplementedException();
    }
}