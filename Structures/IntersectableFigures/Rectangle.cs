using Structures.BaseGeometricalStructures;
using Structures.Interfaces;

namespace Structures.IntersectableFigures;

public class Rectangle : IIntersectable
{
    public Point A { get; protected set; }
    public Point B { get; protected set; }
    public Point C { get; protected set; }
    public Point D { get; protected set; }
    public Vector Normal { get; }

    public Rectangle(Point a, Point b, Point c)
    {
        if (new Vector(b, a).DotProductWith(new Vector(b, c)) != 0)
            throw new ArgumentException("This points can't define the rectangle!");
        A = a;
        B = b;
        C = c;
        D = a + new Vector(b, c);
        Normal = CalculateNormalVector(A, B, C);
    }
    
    public Rectangle(Point a, Point b, Point c, Point d)
    {
        Vector ab = new Vector(a, b);
        Vector bc = new Vector(b, c);
        Vector dc = new Vector(d, c);
        
        if (!ab.Equals(dc) || ab.DotProductWith(bc) != 0)
            throw new ArgumentException("This points can't define the rectangle!");
        
        A = a;
        B = b;
        C = c;
        D = d;
        Normal = CalculateNormalVector(A, B, C);
    }

    public Rectangle(Rectangle original)
    {
        A = original.A;
        B = original.B;
        C = original.C;
        D = original.D;
        Normal = original.Normal;
    }

    public Vector GetNormalVector(Point point) => Normal;
    
    private static Vector CalculateNormalVector(Point p1, Point p2, Point p3)
    {
        return new Vector(p1, p2).CrossProductWith(new Vector(p1, p3)).Normalized();
    }

    public bool HasIntersectionWith(Ray ray, out Point intersectionPoint)
    {
        throw new NotImplementedException();
    }
}