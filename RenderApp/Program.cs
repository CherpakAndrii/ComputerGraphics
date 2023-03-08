using Core;
using RenderApp;

ConsoleConfigurator configurator = new();
configurator.SetupConsole();

var consoleRenderer = new ConsoleRenderer(false, false);

var scenes = new[]
{
    ScenesSetup.SceneWithAllFigures(),
    ScenesSetup.CheburashkaScene()
};

foreach (var scene in scenes)
{
    RayTracer rayTracer = new(scene);
    consoleRenderer.PrintToConsole(rayTracer.TraceRays());
    Console.ReadKey();
    Console.Clear();
}
