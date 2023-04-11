using ImageFormatConverter.Common;
using ImageFormatConverter.Console;

const string goalFormatFlag = "goal-format";
const string sourceFlag = "source";
const string outputFlag = "output";

var flagValues = CommandLineArgumentsParser.GetFlagsValues
(
    args,
    new []{ goalFormatFlag, sourceFlag },
    new []{ outputFlag }
);

var source = flagValues[sourceFlag];
var goalFormat = flagValues[goalFormatFlag];
flagValues.TryGetValue(outputFlag, out var output);

var converterApp = new ConverterApp(source, goalFormat, output);
converterApp.Run();