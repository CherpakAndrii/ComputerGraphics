namespace Structures.BaseGeometricalStructures;

public struct Ray
{
    public Point Origin { get; private set; }
    public Vector Direction { get; private set; }

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
        Vector vectorToPoint = new Vector(Origin, another);
        float coef = vectorToPoint.DotProductWith(Direction) / (float)Math.Pow(Direction.GetModule(), 2);
        Point nearest = Origin + Direction * coef;

        return nearest;
    }
}