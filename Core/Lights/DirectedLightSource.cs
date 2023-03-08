using Structures.BaseGeometricalStructures;

namespace Core.Lights;

public readonly struct DirectedLightSource : ILightSource
{
    public Vector Direction { get; }
    public Color Color { get; }

    public DirectedLightSource(Vector direction, Color color)
    {
        Direction = direction;
        Color = color;
    }
    
    public DirectedLightSource(Vector direction)
    {
        Direction = direction;
        Color = new Color(255, 255, 255);
    }
    
    public DirectedLightSource(DirectedLightSource original)
    {
        Direction = original.Direction;
        Color = original.Color;
    }
    
    public Vector GetVector(Point targetPoint) => Direction;
}