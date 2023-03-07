using Structures.BaseGeometricalStructures;
using Structures.Interfaces;

namespace Structures.IntersectableFigures;

public class Disk : IIntersectable
{
    public bool IsFlat { get; } = true;

    public Point Position { get; }
    public Vector Normal { get; }
    public float Radius { get; }

    private readonly Plane _plane;

    private readonly float _radiusSquared;

    public Disk(Point position, Vector normal, float radius)
    {
        Position = position;
        Normal = normal.Normalized();
        Radius = radius;
        _plane = new Plane(Position, Normal);
        _radiusSquared = Radius * Radius;
    }
    public Vector GetNormalVector(Point point)
        => Normal;

    public Point? GetIntersectionWith(Ray ray)
    {
        var intersectionPoint = _plane.GetIntersectionWith(ray);
        if (intersectionPoint is null)
            return null;

        var radiusVector = new Vector(Position, intersectionPoint.Value);
        float radiusVectorLengthSquared = radiusVector.DotProductWith(radiusVector);
        return radiusVectorLengthSquared <= _radiusSquared
            ? intersectionPoint
            : null;
    }

    // https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-plane-and-ray-disk-intersection.html
    public bool HasIntersectionWith(Ray ray)
        => GetIntersectionWith(ray) is not null;
}