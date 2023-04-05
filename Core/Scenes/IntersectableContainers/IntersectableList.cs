using System.Diagnostics.CodeAnalysis;
using Structures.BaseGeometricalStructures;
using Structures.Interfaces;

namespace Core.Scenes.IntersectableContainers;

public class IntersectableList : IIntersectableContainer
{
    private List<IIntersectable> _figures;

    public IntersectableList(List<IIntersectable> figures) { _figures = figures; }
    
    public IntersectableList() { _figures = new List<IIntersectable>(); }

    public bool FindClosestIntersection(Ray ray, out Point intersection, [NotNullWhen(true)] out IIntersectable? figure)
    {
        bool intersected = false;
        intersection = new();
        figure = default;
        double minDistance = double.MaxValue;
        foreach (var f in _figures)
        {
            if (f.GetIntersectionWith(ray) is not { } currIntersection)
                continue;

            intersected = true;
            double currDistance = new Vector(ray.Origin, currIntersection).GetModule();
            if (currDistance < minDistance)
            {
                intersection = currIntersection;
                figure = f;
                minDistance = currDistance;
            }
        }
        return intersected;
    }

    public bool CheckForIntersections(Ray ray, IIntersectable? currentFigure)
    {
        foreach (var figure in _figures)
        {
            if (figure.GetIntersectionWith(ray) is { } point
                && (!point.Equals(ray.Origin) || figure != currentFigure))
                return false;
        }
        return true;
    }

    public void Add(IIntersectable figure) => _figures.Add(figure);
}