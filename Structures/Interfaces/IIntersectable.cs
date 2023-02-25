using Structures.BaseGeometricalStructures;

namespace Structures.Interfaces;

public interface IIntersectable
{
    public Vector GetNormalVector(Point point);
    public bool HasIntersectionWith(Ray ray);
    public Point? GetIntersectionWith(Ray ray);
}