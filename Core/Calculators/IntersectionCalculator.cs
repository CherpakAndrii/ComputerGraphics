using Core.Scenes;
using Structures.Interfaces;
using System.Diagnostics.CodeAnalysis;
using Structures.BaseGeometricalStructures;

namespace Core.Calculators;

public static class IntersectionCalculator
{
    public static bool FindClosestIntersection(Scene scene, Ray ray, out Point intersection, [NotNullWhen(true)] out IIntersectable? figure)
    {
        bool intersected = false;
        intersection = new();
        figure = default;
        double minDistance = double.MaxValue;
        foreach (var f in scene.Figures)
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
}
