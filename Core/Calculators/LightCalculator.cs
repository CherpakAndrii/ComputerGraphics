using Core.Lights;
using Core.Scenes;
using Structures.Interfaces;
using Structures.BaseGeometricalStructures;

namespace Core.Calculators;

public static class LightCalculator
{
    public static void CalculateLight(Scene scene, ref Color pixel, Point intersection, IIntersectable figure)
    {
        foreach (var lightSource in scene.LightSources)
        {
            Ray toLightRay = new(intersection, lightSource.GetVector(intersection));
            if (!IsOnLight(scene, toLightRay, intersection, figure))
                continue;
            Vector normal = figure!.GetNormalVector(intersection);
            if (figure.IsFlat && normal.FindCos(scene.Camera.Direction) > 0) normal *= -1;
            double cosLight = toLightRay.Direction.FindCos(normal);
            if (cosLight > 0)
            {
                pixel.R += (int)(Math.Abs(cosLight) * lightSource.Color.R);
                pixel.G += (int)(Math.Abs(cosLight) * lightSource.Color.G);
                pixel.B += (int)(Math.Abs(cosLight) * lightSource.Color.B);
            }
        }
    }

    private static bool IsOnLight(Scene scene, Ray ray, Point currentIntersection, IIntersectable currentFigure)
    {
        foreach (var figure in scene.Figures)
        {
            if (figure.GetIntersectionWith(ray) is { } point
                && (!point.Equals(currentIntersection) || figure != currentFigure))
                return false;
        }
        return true;
    }
}
