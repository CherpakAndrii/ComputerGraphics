using Structures;

namespace Core;

public interface ILightSource
{
    public Vector GetVector(Point targetPoint);
    public Color Color { get; set; }
    
}