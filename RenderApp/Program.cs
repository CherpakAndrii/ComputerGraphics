using Core;
using Core.ObjFileReader;
using Core.Scenes;
using ImageFormatConverter.Common;
using RenderApp;
using RenderApp.FileOutput;
using Structures.BaseGeometricalStructures;

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
    var triangles = objFileReader.GetTrianglesFromFile(objFileData);

    var scene = ScenesSetup.EmptySceneWithLightAndCamera();

    SceneTransformator sceneTransformator = new();
    sceneTransformator.RotateDegreeX(170);
    sceneTransformator.RotateDegreeZ(45);
    sceneTransformator.ToScale(new Vector(0.7f, 0.7f, 0.7f));
    sceneTransformator.Move(new Vector(-0.2f, 0, 0.2f));
    

    var transformedTriangles = triangles.Select(triangle => sceneTransformator.Apply(triangle)).ToArray();

    foreach (var figure in transformedTriangles)
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