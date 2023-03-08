namespace Structures.BaseGeometricalStructures;

public readonly struct Ray
{
    public Point Origin { get; }
    public Vector Direction { get; }

    public Ray(Point start, Vector direction)
    {
        Origin = start;
        Direction = direction;
    }
    
    public Ray(Point start, Point direction)
    {
        Origin = start;
        Direction = new Vector(start, direction);
    }
    
    public Ray(Ray original)
    {
        Origin = original.Origin;
        Direction = original.Direction;
    }

    public Point GetNearestPointTo(Point another)
    {
        var vectorToPoint = new Vector(Origin, another);
        float coef = vectorToPoint.DotProductWith(Direction) / (float)Math.Pow(Direction.GetModule(), 2);
        if (coef <= 0) return Origin;
        
        var nearest = Origin + Direction * coef;
        return nearest;
    }
}