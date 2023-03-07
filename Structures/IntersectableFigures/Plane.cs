using Structures.BaseGeometricalStructures;
using Structures.Interfaces;
using Vector = Structures.BaseGeometricalStructures.Vector;
using Point = Structures.BaseGeometricalStructures.Point;

namespace Structures.IntersectableFigures;

public class Plane : IIntersectable
{
    public Point Point { get; }
    public Vector Normal { get; }

    public Plane(Point point, Vector normal)
    {
        Point = point;
        Normal = normal.Normalized();
    }

    public Vector GetNormalVector(Point point)
    {
        return Normal;
    }

    public Point? GetIntersectionWith(Ray ray)
    {
        var distance = GetDistanceFromRayOriginToPlane(ray);
        if (distance is null or < 0)
            return null;

        return ray.Origin + ray.Direction.Normalized() * distance;
    }

    public bool HasIntersectionWith(Ray ray)
    {
        return GetDistanceFromRayOriginToPlane(ray) is >= 0;
    }

    // https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-plane-and-ray-disk-intersection.html
    private float? GetDistanceFromRayOriginToPlane(Ray ray)
    {
        float denominator = Normal.DotProductWith(ray.Direction);
        if (denominator < 1e-6)
            return null;
        
        Vector difference = new(ray.Origin, Point);
        return difference.DotProductWith(Normal) / denominator;
    }
}