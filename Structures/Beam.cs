namespace Structures;

public struct Beam
{
    public Point StartPoint { get; private set; }
    public Vector Direction { get; private set; }

    public Beam(Point start, Vector direction)
    {
        StartPoint = start;
        Direction = direction;
    }
    
    public Beam(Point start, Point direction)
    {
        StartPoint = start;
        Direction = new Vector(start, direction);
    }
    
    public Beam(Beam original)
    {
        StartPoint = original.StartPoint;
        Direction = original.Direction;
    }
}