using Core;
using Core.ObjFileReader;
using ImageFormatConverter.Common;
using RenderApp;
using RenderApp.FileOutput;

const string sourceFlag = "source";
const string outputFlag = "output";

try
{
    var flagValues = CommandLineArgumentsParser.GetFlagsValues
    (
        args,
        new[] { sourceFlag, outputFlag },
        Array.Empty<string>()
    );

    var source = flagValues[sourceFlag];
    var output = flagValues[outputFlag];

    var objFileData = File.ReadAllLines(source);

    ObjFileReader objFileReader = new();
    var structures = objFileReader.GetStructuresFromFile(objFileData);

    var scene = ScenesSetup.EmptySceneWithLightAndCamera();

    foreach (var figure in structures)
    {
        scene.Figures.Add(figure);
    }

    IRenderOutput renderOutput = new RenderFileOutput(output);

    RayTracer rayTracer = new(scene, renderOutput);
    rayTracer.TraceRays();

}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}
finally
{
    Console.ReadKey();
}