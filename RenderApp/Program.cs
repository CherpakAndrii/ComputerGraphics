using Core;
using RenderApp;

ConsoleConfigurator configurator = new();
configurator.SetupConsole();

var scene = ScenesSetup.SceneWithAllFigures();

RayTracer rayTracer = new(scene);

ConsoleRenderer.Retro = false;
ConsoleRenderer.RetroColor = false;
ConsoleRenderer.PrintToConsole(rayTracer.TraceRays());
Console.ReadKey();
