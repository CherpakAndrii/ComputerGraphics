using Structures.BaseGeometricalStructures;

namespace Structures.Interfaces;

public interface IIntersectable
{
    public Vector GetNormalVector(Point point);
    public Point? GetIntersectionWith(Ray ray);
    public bool HasIntersectionWith(Ray ray);
}