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
    .Select(type => (IImageReader)Activator.CreateInstance(type)!)
    .ToArray();

var writers = allAssemblies
    .SelectMany(s => s.GetTypes())
    .Where(type => typeof(IImageWriter) .IsAssignableFrom(type))
    .Select(type => (IImageWriter)Activator.CreateInstance(type)!)
    .ToArray();

const string goalFormatFlag = "goal-format";
const string sourceFlag = "source";
const string outputFlag = "output";

var flagValues = new Dictionary<string, string>();

var flags = new [] { goalFormatFlag, sourceFlag, outputFlag };
foreach (var arg in args)
{
    foreach (var flag in flags)
    {
        if (arg.StartsWith($"--{flag}="))
        {
            var flagValue = arg[(arg.IndexOf('=') + 1)..];
            if (string.IsNullOrWhiteSpace(flagValue))
            {
                throw new Exception("Program arg is empty");
            }
            flagValues.Add(flag, flagValue);
        }
    }
}

var source = flagValues[sourceFlag];
if (!File.Exists(source))
    throw new Exception("Source file doesn't exist");

var fileData = File.ReadAllBytes(source);

var targetReader = readers.FirstOrDefault(reader => reader.ValidateFileStructure(fileData));

var targetWriter = writers.FirstOrDefault(writer => writer.FileExtension == flagValues[goalFormatFlag]);

foreach (var flagValue in flagValues.Values)
{
    Console.WriteLine(flagValue);
}