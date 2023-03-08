﻿/* Чебурашка
using Core;
using RenderApp;
using Core.Lights;
using Core.Scenes;
using Core.Cameras;
using Structures.IntersectableFigures;

Camera camera = new()
{
    ProjectionPlaneHeightInPixels = 80,
    ProjectionPlaneWidthInPixels = 80,
    DistanceToProjectionPlane = 1,
    FieldOfView = 30,
    Direction = new(1, 0, 0),
    Position = new(0, 0, 0)
};

Sphere sphere = new(new(6, -0.2f, -1f), 0.5f);
Sphere sphere2 = new(new(6, -0.2f, 1f), 0.5f);
Sphere sphere3 = new(new(6, 0, 0), 1f);
Sphere sphere4 = new(new(5, -0.2f, -0.3f), 0.15f);
Sphere sphere5 = new(new(5, -0.2f, 0.3f), 0.15f);

LightPoint lightPoint = new(new(0, 0, 0), new(139, 69, 19));
LightPoint lightPoint2 = new(new(-5, 0, 5), new(123, 45, 125));

Scene scene = new() { Camera = camera };
scene.LightSources.Add(lightPoint);
scene.LightSources.Add(lightPoint2);

scene.Figures.Add(sphere);
scene.Figures.Add(sphere2);
scene.Figures.Add(sphere3);
scene.Figures.Add(sphere4);
scene.Figures.Add(sphere5);

RayTracer rayTracer = new(scene);
ConsoleRenderer.PrintToConsole(rayTracer.TraceRays());
*/

using Core;
using RenderApp;
using Core.Lights;
using Core.Scenes;
using Core.Cameras;
using Structures.BaseGeometricalStructures;
using Structures.IntersectableFigures;

ConsoleConfigurator configuerer = new();
configuerer.SetupConsole();

Camera camera = new()
{
    ProjectionPlaneHeightInPixels = 60,
    ProjectionPlaneWidthInPixels = 60,
    DistanceToProjectionPlane = 1,
    FieldOfView = 90,
    Direction = new(1, 0, 0),
    Position = new(0, 0, 0)
};

Sphere sphere = new(new(7, 0, 0), 4);
Plane plane = new Plane(new Point(10, 0, 0), new Vector(-1, 0, 0));
Disk disk = new Disk(new Point(2, 2, 2), new Vector(-1, 0, 0), 1);

LightPoint lightPoint = new(new(0, 0, 0), new(0, 255, 255));
LightPoint lightPoint2 = new(new(-3, 5, 5), new(255, 0, 255));

Scene scene = new() { Camera = camera };
scene.LightSources.Add(lightPoint);
scene.LightSources.Add(lightPoint2);
scene.Figures.Add(sphere);
scene.Figures.Add(plane);
scene.Figures.Add(disk);

RayTracer rayTracer = new(scene);

ConsoleRenderer.Retro = false;
ConsoleRenderer.RetroColor = false;
ConsoleRenderer.PrintToConsole(rayTracer.TraceRays());
Console.ReadKey();
