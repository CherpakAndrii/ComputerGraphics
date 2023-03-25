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

foreach (var arg in args)
{
    Console.WriteLine(arg);
}
    