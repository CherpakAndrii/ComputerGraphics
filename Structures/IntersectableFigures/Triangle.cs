using Structures.Interfaces;
using Structures.BaseGeometricalStructures;

namespace Structures.IntersectableFigures;

public struct Triangle : IIntersectable
{
    public bool IsFlat => true;

    public Point A { get; }
    public Point B { get; }
    public Point C { get; }
    
    public Vector Normal { get; }

    public Triangle(Point p1, Point p2, Point p3)
    {
        A = p1;
        B = p2;
        C = p3;
        Normal = CalculateNormalVector(A, B, C);
    }
    
    public Triangle((Point, Point, Point) points)
    {
        (A, B, C) = points;
        Normal = CalculateNormalVector(A, B, C);
    }

    private static Vector CalculateNormalVector(Point p1, Point p2, Point p3)
    {
        return new Vector(p1, p2).CrossProductWith(new Vector(p1, p3)).Normalized();
    }

    public Vector GetNormalVector(Point point) => Normal;
    
    public Point? GetIntersectionWith(Ray ray)
    {
        const double epsilon = 0.000001;
        Vector edge1 = new Vector(A, B);
        Vector edge2 = new Vector(A, C);
        Vector h = ray.Direction.CrossProductWith(edge2);
        double a = edge1.DotProductWith(h);
        if (a > -epsilon && a < epsilon) return null;    // The ray is parallel to the triangle
        float f = (float)(1.0 / a);
        Vector s = new Vector(ray.Origin.X - A.X, ray.Origin.Y - A.Y, ray.Origin.Z - A.Z);
        float u = f * s.DotProductWith(h);
        if (u < 0.0 || u > 1.0) return null;    // The intersection point is outside the triangle
        Vector q = s.CrossProductWith(edge1);
        float v = f * ray.Direction.DotProductWith(q);
        if (v < 0.0 || u + v > 1.0) return null;    // The intersection point is outside the triangle
        float t = f * edge2.DotProductWith(q);
        if (t > epsilon) return ray.Origin + ray.Direction * t;
        
        return null;    // The intersection point is behind the ray's origin
    }
    // implemented by example https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm

    public bool HasIntersectionWith(Ray ray) => GetIntersectionWith(ray) is not null;
}