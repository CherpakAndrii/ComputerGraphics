using Core;
using Core.Cameras;
using Core.Lights;
using Core.ObjFileReader;
using Core.Scenes;
using ImageFormatConverter.Common;
using RenderApp.FileOutput;
using Structures.BaseGeometricalStructures;

const string sourceFlag = "source";
const string outputFlag = "output";

var flagValues = CommandLineArgumentsParser.GetFlagsValues
(
    args,
    new []{ sourceFlag, outputFlag },
    Array.Empty<string>()
);

var source = flagValues[sourceFlag];
var output = flagValues[outputFlag];

var objFileData = File.ReadAllLines(source);

ObjFileReader objFileReader = new();
var structures = objFileReader.GetStructuresFromFile(objFileData);

Camera camera = new
(
    new Point(0, -1, 0),
    new Vector(0, 1, 0),
    90,
    1
);

var projectionPlane = new ProjectionPlane(camera, 100, 100);

LightPoint lightPoint = new(new Point(0, -1, 0), new Color(100, 255, 255));

Scene scene = new() { ProjectionPlane = projectionPlane };
scene.LightSources.Add(lightPoint);
foreach (var figure in structures)
{
    scene.Figures.Add(figure);
}

IRenderOutput renderOutput = new RenderFileOutput(output);

RayTracer rayTracer = new(scene, renderOutput);
rayTracer.TraceRays();
