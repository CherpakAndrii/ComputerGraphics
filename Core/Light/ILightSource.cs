using Core.Light;
using Structures;
using Structures.BaseGeometricalStructures;

namespace Core;

public interface ILightSource
{
    public Vector GetVector(Point targetPoint);
    public Color Color { get; set; }
}