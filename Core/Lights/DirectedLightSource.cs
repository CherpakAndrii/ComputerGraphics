using Core.Lights;
using Structures;
using Structures.BaseGeometricalStructures;

namespace Core;

public class DirectedLightSource : ILightSource
{
    public Vector Direction { get; protected set; }
    public Color Color { get; set; }

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