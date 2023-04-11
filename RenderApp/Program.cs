using Core;
using Core.Cameras;
using Core.Lights;
using Core.ObjFileReader;
using Core.Scenes;
using ImageFormatConverter.Common;
using ImageFormatConverter.Console;
using RenderApp;
using Structures.BaseGeometricalStructures;
using Structures.IntersectableFigures;

ConsoleConfigurator configurator = new();
configurator.SetupConsole();

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

ObjFileReader objFileReader = new();
var objFileData = File.ReadAllLines(source);
var structures = objFileReader.GetStructuresFromFile(objFileData);

Camera camera = new
(
    new Point(0, -1, 0),
    new Vector(0, 1, 0),
    90,
    1
);

var projectionPlane = new ProjectionPlane(camera, 90, 90);

LightPoint lightPoint = new(new Point(-10, -10, -10), new Color(255, 255, 255));

Scene scene = new() { ProjectionPlane = projectionPlane };
scene.LightSources.Add(lightPoint);
foreach (var figure in structures)
{
    scene.Figures.Add(figure);
}

RayTracer rayTracer = new(scene);

var fileFactory = new FileFactory();
var imageWriter = fileFactory.GetImageWriter("png");

var fileData = imageWriter.WriteToFile(rayTracer.TraceRays());

var fileWriter = new FileWriter(output);
fileWriter.Write(fileData, $".{imageWriter.FileExtension}", output, "picture");