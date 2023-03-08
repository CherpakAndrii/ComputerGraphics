using Core.Lights;
using Core.Scenes;
using Structures.Interfaces;
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
                if (FindClosestIntersection(ray, out Point intersectionPoint, out IIntersectable? figure))
                    CalculateLight(ref pixels[i, j], intersectionPoint, figure);
            }
        }
        return pixels;
    }

    private void CalculateLight(ref Color pixel, Point intersection, IIntersectable? figure)
    {
        foreach (ILightSource lightSource in Scene.LightSources)
        {
            Ray toLightRay = new(intersection, lightSource.GetVector(intersection));
            if (!IsOnLight(toLightRay, intersection))
                continue;
            Vector normal = figure!.GetNormalVector(intersection);
            if (figure.IsFlat && normal.FindCos(Scene.Camera.Direction) > 0) normal *= -1;
            double cosLight = toLightRay.Direction.FindCos(normal);
            if (cosLight > 0)
            {
                pixel.R += (int)(Math.Abs(cosLight) * lightSource.Color.R);
                pixel.G += (int)(Math.Abs(cosLight) * lightSource.Color.G);
                pixel.B += (int)(Math.Abs(cosLight) * lightSource.Color.B);
            }
        }
    }
 
    private bool FindClosestIntersection(Ray ray, out Point intersection, out IIntersectable? figure)
    {
        bool intersected = false;
        intersection = new();
        figure = default;
        double minDistance = double.MaxValue;
        foreach (var f in Scene.Figures)
        {
            if (f.GetIntersectionWith(ray) is { } currIntersection)
            {
                intersected = true;
                double currDistance = new Vector(ray.Origin, currIntersection).GetModule();
                if (currDistance < minDistance)
                {
                    intersection = currIntersection;
                    figure = f;
                    minDistance = currDistance;
                }
            }
        }
        return intersected;
    }

    private bool IsOnLight(Ray ray, Point currentIntersection)
    {
        foreach (var figure in Scene.Figures)
        {
            if (figure.GetIntersectionWith(ray) is { } point && !point.Equals(currentIntersection))
                return false;
        }

        return true;
    }
}