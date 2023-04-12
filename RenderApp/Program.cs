using Core;
using Core.ObjFileReader;
using Core.Scenes;
using ImageFormatConverter.Common;
using RenderApp;
using RenderApp.FileOutput;
using Structures.BaseGeometricalStructures;
using Structures.IntersectableFigures;

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

    SceneTransformator cameraTransformator = new();
    cameraTransformator.RotateDegreeX(-45);
    cameraTransformator.Move(new Vector(0, -0.8f, 0.8f));
    var scene = ScenesSetup.EmptySceneWithLightAndCamera(cameraTransformator);
    scene.Figures.Add(new Plane(new Point(0, 0, 0), new Vector(0, 1, 0)));

    SceneTransformator sceneTransformator = new();
    sceneTransformator.RotateDegreeX(90);
    sceneTransformator.Move(new Vector(0, -0.4f, 0));
    

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