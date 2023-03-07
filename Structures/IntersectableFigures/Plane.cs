using Structures.BaseGeometricalStructures;
using Structures.Interfaces;
using Vector = Structures.BaseGeometricalStructures.Vector;

namespace Structures.IntersectableFigures;

public class Plane : IIntersectable
{
    private Point Point { get; set; }
    private Vector Normal { get; set; }
    
    public Vector GetNormalVector(Point point)
    {
        return Normal.Normalized();
    }

    public Point? GetIntersectionWith(Ray ray)
    {
        throw new NotImplementedException();
    }

    public bool HasIntersectionWith(Ray ray)
    {
        return GetDistanceFromRayOriginToPlane(ray) is >= 0;
    }

    // https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-plane-and-ray-disk-intersection.html
    private float? GetDistanceFromRayOriginToPlane(Ray ray)
    {
        Vector normal = Normal.Normalized();
        float denominator = normal.DotProductWith(ray.Direction);
        if (denominator < 1e-6)
            return null;
        
        Vector difference = new(ray.Origin, Point);
        float t = difference.DotProductWith(normal) / denominator;
        return t;
    }
}