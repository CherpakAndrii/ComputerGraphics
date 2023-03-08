using Core;
using RenderApp;
using Core.Lights;
using Core.Scenes;
using Core.Cameras;
using Structures.BaseGeometricalStructures;
using Structures.IntersectableFigures;

ConsoleConfigurator configuerer = new();
configuerer.SetupConsole();

Camera camera = new
(
    new Point(0, 0, 0),
    new Vector(1, 0, 0),
    90,
    1
);

var projectionPlane = new ProjectionPlane(camera, 60, 60);

Sphere sphere = new(new Point(7, 0, 0), 4);
var plane = new Plane(new Point(10, 10, 0), new Vector(-1, -1, 0));
var disk = new Disk(new Point(2, 2, 2), new Vector(-1, 0, 0), 1);

LightPoint lightPoint = new(new Point(0, 0, 0), new Color(0, 255, 255));
LightPoint lightPoint2 = new(new Point(-3, 5, 5), new Color(255, 0, 255));

Scene scene = new() { ProjectionPlane = projectionPlane };
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
