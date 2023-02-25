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

    public bool HasIntersectionWith(Ray ray)
    {
        return (new Vector(Center, ray.GetNearestPointTo(Center)).GetModule() <= Radius);
    }

    public Point? GetIntersectionWith(Ray ray)
    {
        Vector vectorFromRayOriginToSphereCenter = new Vector(ray.Origin, Center);

        var rayDirectionSquared = ray.Direction.DotProductWith(ray.Direction);
        var radiusSquared = Radius * Radius;
        var vectorFromRayOriginToSphereCenterSquared =
            vectorFromRayOriginToSphereCenter.DotProductWith(vectorFromRayOriginToSphereCenter);

        var a = rayDirectionSquared;
        var b = 2 * ray.Direction.DotProductWith(vectorFromRayOriginToSphereCenter);
        var c = vectorFromRayOriginToSphereCenterSquared - radiusSquared;

        var D = b * b - 4 * a * c;
        if (D < 0)
            return null;

        var distance1 = (-b - Math.Sqrt(D)) / 2;
        var distance2 = (-b + Math.Sqrt(D)) / 2;
        var distance = (float)Math.Min(distance1, distance2);

        var intersectionPoint = ray.Origin + ray.Direction.Normalized() * distance;
        return intersectionPoint;
    }
}