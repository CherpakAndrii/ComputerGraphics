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
            if (!scene.Figures.CheckForIntersections(toLightRay, figure))
                continue;
            Vector normal = figure!.GetNormalVector(intersection);
            if (figure.IsFlat && normal.FindCos(scene.ProjectionPlane.Camera.Direction) > 0)
                normal *= -1;
            double cosLight = toLightRay.Direction.FindCos(normal);
            if (cosLight > 0)
            {
                pixel.R += (int)(Math.Abs(cosLight) * lightSource.Color.R);
                pixel.G += (int)(Math.Abs(cosLight) * lightSource.Color.G);
                pixel.B += (int)(Math.Abs(cosLight) * lightSource.Color.B);
            }
        }
    }
}
