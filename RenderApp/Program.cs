using Core;
using RenderApp;

ConsoleConfigurator configurator = new();
configurator.SetupConsole();

var consoleRendererColourful = new ConsoleRenderer(false, false);
var consoleRendererRetroSymbols = new ConsoleRenderer(true, false);

var scenes = new[]
{
    (consoleRendererColourful, ScenesSetup.PlaneAndSphere()),
    (consoleRendererColourful, ScenesSetup.SceneWithAllFigures()),
    (consoleRendererRetroSymbols, ScenesSetup.CheburashkaScene()),
    (consoleRendererColourful, ScenesSetup.CheburashkaScene()),
    (consoleRendererColourful, ScenesSetup.SceneSphereAndDisk())
};

foreach (var scene in scenes)
{
    RayTracer rayTracer = new(scene.Item2);
    scene.Item1.PrintToConsole(rayTracer.TraceRays());
    Console.ReadKey();
    Console.Clear();
}
