using Core;
using Core.Scenes;
using Core.Cameras;
using Structures.IntersectableFigures;

Camera camera = new()
{
    ProjectionPlaneHeightInPixels = 80,
    ProjectionPlaneWidthInPixels = 80,
    DistanceToProjectionPlane = 1,
    FieldOfView = 90,
    Direction = new(1, 0, 0),
    Position = new(0, 0, 0)
};

Sphere sphere = new(new(5, 0, 0), 4);

LightPoint lightPoint = new(new(-2, -5, 5), new(0, 255, 255));
LightPoint lightPoint2 = new(new(-3, 5, 5), new(255, 0, 255));

Scene scene = new() { Camera = camera };
scene.LightSources.Add(lightPoint);
scene.LightSources.Add(lightPoint2);
scene.Figures.Add(sphere);

RayTracer rayTracer = new(scene);
ConsoleRenderer.PrintToConsole(rayTracer.TraceRays());