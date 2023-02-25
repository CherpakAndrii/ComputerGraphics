namespace Structures.BaseGeometricalStructures;

public struct Ray
{
    public Point StartPoint { get; private set; }
    public Vector Direction { get; private set; }

    public Ray(Point start, Vector direction)
    {
        StartPoint = start;
        Direction = direction;
    }
    
    public Ray(Point start, Point direction)
    {
        StartPoint = start;
        Direction = new Vector(start, direction);
    }
    
    public Ray(Ray original)
    {
        StartPoint = original.StartPoint;
        Direction = original.Direction;
    }

    public Point GetNearestPointTo(Point another)
    {
        Vector vectorToPoint = new Vector(StartPoint, another);
        float coef = vectorToPoint.DotProductWith(Direction) / (float)Math.Pow(Direction.GetModule(), 2);
        Point nearest = StartPoint + Direction * coef;

        return nearest;
    }
}