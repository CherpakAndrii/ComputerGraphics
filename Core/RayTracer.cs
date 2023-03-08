using Core.Lights;
using Core.Scenes;
using Core.Calculators;
using Structures.BaseGeometricalStructures;

namespace Core;

public class RayTracer
{
    public Scene Scene { get; set; }

    public RayTracer(Scene scene) { Scene = scene; }

    public Color[,] TraceRays()
    {
        Point[,] projectionPlane = Scene.Camera.ProjectionPlane;
        Color[,] pixels = new Color[projectionPlane.GetLength(0), projectionPlane.GetLength(1)];
        for (int i = 0; i < projectionPlane.GetLength(0); i++)
        {
            for (int j = 0; j < projectionPlane.GetLength(1); j++)
            {
                pixels[i, j] = new();
                Ray ray = new(Scene.Camera.Position, projectionPlane[i, j]);
                if (IntersectionCalculator.FindClosestIntersection(Scene, ray, out var intersectionPoint, out var figure))
                    LightCalculator.CalculateLight(Scene, ref pixels[i, j], intersectionPoint, figure);
            }
        }
        return pixels;
    }
}