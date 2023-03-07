﻿using Core.Lights;
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
                    foreach (var lightSource in Scene.LightSources)
                    {
                        Ray toLightRay = new(intersectionPoint, lightSource.GetVector(intersectionPoint));
                        if (IsOnLight(toLightRay))
                        {
                            Vector normal = figure!.GetNormalVector(intersectionPoint);
                            if (normal.FindCos(Scene.Camera.Direction) > 0) normal *= -1;
                            double cosLight = toLightRay.Direction.FindCos(normal);
                            if (cosLight > 0)
                            {
                                pixels[i, j].R += (int)(Math.Abs(cosLight) * lightSource.Color.R);
                                pixels[i, j].G += (int)(Math.Abs(cosLight) * lightSource.Color.G);
                                pixels[i, j].B += (int)(Math.Abs(cosLight) * lightSource.Color.B);
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

    private bool IsOnLight(Ray ray)
    {
        foreach (var figure in Scene.Figures)
        {
            if (figure.GetIntersectionWith(ray) is not null)
                return false;
        }

        return true;
    }
}