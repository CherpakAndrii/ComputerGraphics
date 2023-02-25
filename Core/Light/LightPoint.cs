using Core.Light;
using Structures;
using Structures.BaseGeometricalStructures;

namespace Core;

public class LightPoint : ILightSource
{
    public Point Location { get; protected set; }
    public Color Color { get; set; }

    public LightPoint(Point location, Color color)
    {
        Location = location;
        Color = color;
    }
    
    public LightPoint(Point location)
    {
        Location = location;
        Color = new Color(255, 255, 255);
    }
    
    public LightPoint(LightPoint original)
    {
        Location = original.Location;
        Color = original.Color;
    }
    
    public Vector GetVector(Point targetPoint)
    {
        return new Vector(targetPoint, Location).Normalized();
    }
}