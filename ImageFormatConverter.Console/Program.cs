// See https://aka.ms/new-console-template for more information


using System.Reflection;
using ImageFormatConverter.Abstractions.Interfaces;

var allAssemblies = new List<Assembly>();
const string path = @"C:\Users\Acer\Documents\computerGraphics\ComputerGraphics\ImagePlugins\net7.0";

foreach (var dll in Directory.GetFiles(path, "*.dll"))
{
    allAssemblies.Add(Assembly.LoadFile(dll));
}

var readers = allAssemblies
    .SelectMany(s => s.GetTypes())
    .Where(type => typeof(IImageReader) .IsAssignableFrom(type))
    .ToArray();

var writers = allAssemblies
    .SelectMany(s => s.GetTypes())
    .Where(type => typeof(IImageWriter) .IsAssignableFrom(type))
    .ToArray();

string outputPath, goalFormat, source;

const string goalFormatFlag = "goal-format";
const string sourceFlag = "source";
const string outputFlag = "output";

var flagValues = new Dictionary<string, string>();

var flags = new [] { goalFormatFlag, sourceFlag, outputFlag };
foreach (var arg in args)
{
    foreach (var flag in flags)
    {
        if (arg.StartsWith($"--{arg}="))
            flagValues.Add(flag, arg[arg.IndexOf('=')..]);
    }
}
    