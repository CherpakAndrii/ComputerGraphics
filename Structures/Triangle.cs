namespace Structures;

public struct Triangle : IIntersectable
{
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

    public bool HasIntersectionWith(Ray ray)
    {
        throw new NotImplementedException();
    }

    public Point? GetIntersectionWith(Ray ray)
    {
        throw new NotImplementedException();
    }
}