using ImageFormatConverter.Console;

const string goalFormatFlag = "goal-format";
const string sourceFlag = "source";
const string outputFlag = "output";

var commandLineArgumentsParser = new CommandLineArgumentsParser();
var flagValues = commandLineArgumentsParser.GetFlagsValues(args, goalFormatFlag, sourceFlag, outputFlag);

var source = flagValues[sourceFlag];
if (!File.Exists(source))
    throw new Exception("Source file doesn't exist");

var fileData = File.ReadAllBytes(source);

var fileFactory = new FileFactory();

var targetReader = fileFactory.GetImageReader(fileData);
var targetWriter = fileFactory.GetImageWriter(flagValues[goalFormatFlag]);

if (targetReader is null)
{
    var supportedReaderFormats = fileFactory.GetSupportedReadersExtensions().Select(format => $".{format}");
    var inputExtension = Path.GetExtension(source);
    if (string.IsNullOrEmpty(inputExtension))
    {
        throw new Exception($"Error: you are trying to open file with unsupported extension. Only {string.Join(" and ", supportedReaderFormats)} files are supported");
    }
    throw new Exception($"Error: you are trying to open {inputExtension} file, but only {string.Join(" and ", supportedReaderFormats)} files are supported");
}

if (targetWriter is null)
    throw new Exception("Appropriate file writer is not found");
    
var image = targetReader.ImageToPixels(fileData);

var targetFileData = targetWriter.WriteToFile(image);

var path = flagValues.TryGetValue(outputFlag, out var output)
    ? output + flagValues[goalFormatFlag]
    : Path.ChangeExtension(source, flagValues[goalFormatFlag]);

File.WriteAllBytes(path, targetFileData);