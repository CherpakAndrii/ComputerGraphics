using System.Diagnostics.CodeAnalysis;
using Structures.BaseGeometricalStructures;

namespace Structures.Interfaces;

public interface IIntersectableContainer
{
    public bool FindClosestIntersection(Ray ray, out Point intersection, [NotNullWhen(true)] out IIntersectable? figure);
    public bool CheckForIntersections(Ray ray, IIntersectable? currentFigure);
    public void Add(IIntersectable figure);
}