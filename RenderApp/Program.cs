using Core;
using RenderApp;

ConsoleConfigurator configurator = new();
configurator.SetupConsole();

ConsoleRenderer.Retro = false;
ConsoleRenderer.RetroColor = false;

var scenes = new[]
{
    ScenesSetup.SceneWithAllFigures(),
    ScenesSetup.CheburashkaScene()
};

foreach (var scene in scenes)
{
    RayTracer rayTracer = new(scene);
    ConsoleRenderer.PrintToConsole(rayTracer.TraceRays());
    Console.ReadKey();
    Console.Clear();
}
