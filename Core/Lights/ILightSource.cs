using Structures.BaseGeometricalStructures;

namespace Core.Lights;

public interface ILightSource
{
    public Vector GetVector(Point targetPoint);
    public Color Color { get; set; }
}