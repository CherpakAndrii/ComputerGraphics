using Core.Lights;
using Core.Scenes;
using Structures.BaseGeometricalStructures;
using Structures.Interfaces;
using Structures.IntersectableFigures;

namespace Core;
   
public class RayTracer
{
    public required Scene scene { get; set; }

    public RayTracer(Scene scene) { this.scene = scene; }

    public Color[,] TraceRays()
    {
        Point[,] projectionPlane = scene.Camera.ProjectionPlane;
        Color[,] pixels = new Color[projectionPlane.GetLength(0), projectionPlane.GetLength(1)];
        for (int i = 0; i < projectionPlane.GetLength(0); i++)
        {
            for (int j = 0; j < projectionPlane.GetLength(1); j++)
            {
                pixels[i, j] = new();
                Ray ray = new(scene.Camera.Position, projectionPlane[i, j]);
                if (FindClosestIntersection(ray, out Point intersectionPoint, out IIntersectable figure))
                {
                    for (int k = 0; j < scene.LightSources.Count; k++)
                    {
                        Ray toLightRay = new(intersectionPoint, scene.LightSources[k].GetVector(intersectionPoint) * -1);
                        if (IsOnLight(toLightRay))
                        {
                            double cosLight = toLightRay.Direction.FindCos(figure.GetNormalVector(intersectionPoint));
                            pixels[i, j].R += (int)(Math.Abs(cosLight) * 255);
                            pixels[i, j].G += (int)(Math.Abs(cosLight) * 255);
                            pixels[i, j].B += (int)(Math.Abs(cosLight) * 255);
                        }
                    }
                }
            }
        }
        return pixels;
    }
 
    private bool FindClosestIntersection(Ray ray, out Point intersection, out IIntersectable figure)
    {
        bool intersected = false;
        intersection = new();
        figure = new Triangle();
        double minDistance = double.MaxValue;
        for (int i = 0; i < scene.Figures.Count; i++)
        {
            if (scene.Figures[i].HasIntersectionWith(ray, out Point currIntersection))
            {
                intersected = true;
                double currDistance = new Vector(ray.Origin, currIntersection).GetModule();
                if (currDistance < minDistance)
                {
                    intersection = currIntersection;
                    figure = scene.Figures[i];
                    minDistance = currDistance;
                }
            }
        }
        return intersected;
    }

    private bool IsOnLight(Ray ray)
    {
        for (int i = 0; i < scene.Figures.Count; i++)
        {
            if (scene.Figures[i].HasIntersectionWith(ray, out _)) return false;
        }
        return true;
    }
}