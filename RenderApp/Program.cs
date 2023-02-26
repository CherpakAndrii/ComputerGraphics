using Core;
using Core.Scenes;
using Core.Cameras;
using Structures.IntersectableFigures;

Camera camera = new()
{
    ProjectionPlaneHeightInPixels = 30,
    ProjectionPlaneWidthInPixels = 30,
    DistanceToProjectionPlane = 1,
    FieldOfView = 90,
    Direction = new(1, 0, 0),
    Position = new(0, 0, 0)
};

Sphere sphere = new(new(5, 0, 0), 4);

LightPoint lightPoint = new(new(0, 5, 0), new(0, 255, 0));

Scene scene = new() { Camera = camera };
scene.LightSources.Add(lightPoint);
scene.Figures.Add(sphere);

RayTracer rayTracer = new(scene);
ConsoleRenderer.PrintToConsole(rayTracer.TraceRays());