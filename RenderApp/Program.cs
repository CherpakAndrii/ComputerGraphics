using Core.ObjFileReader;
using ImageFormatConverter.Common;

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
objFileReader.GetStructuresFromFile(objFileData);