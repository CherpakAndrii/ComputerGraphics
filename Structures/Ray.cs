namespace Structures;

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
}