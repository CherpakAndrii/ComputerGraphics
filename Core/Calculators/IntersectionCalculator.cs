using Core.Scenes;
using Structures.Interfaces;
using System.Diagnostics.CodeAnalysis;
using Structures.BaseGeometricalStructures;

namespace Core.Calculators;

public static class IntersectionCalculator
{
    public static bool FindClosestIntersection(Scene scene, Ray ray, out Point intersection, [NotNullWhen(true)] out IIntersectable? figure)
    {
        return scene.Figures.FindClosestIntersection(ray, out intersection, out figure);
    }
}
