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
                {
                    for (int k = 0; j < Scene.LightSources.Count; k++)
                    {
                        Ray toLightRay = new(intersectionPoint, Scene.LightSources[k].GetVector(intersectionPoint) * -1);
                        if (IsOnLight(toLightRay))
                        {
                            Vector normal = figure!.GetNormalVector(intersectionPoint);
                            if (normal.FindCos(Scene.Camera.Direction) > 0) normal *= -1;
                            double cosLight = toLightRay.Direction.FindCos(normal);
                            if (cosLight > 0)
                            {
                                pixels[i, j].R += (int)(Math.Abs(cosLight) * 255);
                                pixels[i, j].G += (int)(Math.Abs(cosLight) * 255);
                                pixels[i, j].B += (int)(Math.Abs(cosLight) * 255);
                            }
                        }
                    }
                }
            }
        }
        return pixels;
    }
 
    private bool FindClosestIntersection(Ray ray, out Point intersection, out IIntersectable? figure)
    {
        bool intersected = false;
        intersection = new();
        figure = default;
        double minDistance = double.MaxValue;
        for (int i = 0; i < Scene.Figures.Count; i++)
        {
            if (Scene.Figures[i].HasIntersectionWith(ray, out Point currIntersection))
            {
                intersected = true;
                double currDistance = new Vector(ray.Origin, currIntersection).GetModule();
                if (currDistance < minDistance)
                {
                    intersection = currIntersection;
                    figure = Scene.Figures[i];
                    minDistance = currDistance;
                }
            }
        }
        return intersected;
    }

    private bool IsOnLight(Ray ray)
    {
        for (int i = 0; i < Scene.Figures.Count; i++)
        {
            if (Scene.Figures[i].HasIntersectionWith(ray, out _)) return false;
        }
        return true;
    }
}