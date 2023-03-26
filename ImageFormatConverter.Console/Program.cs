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
    throw new Exception("Appropriate file reader is not found");
    
if (targetWriter is null)
    throw new Exception("Appropriate file writer is not found");
    
var image = targetReader.ImageToPixels(fileData);

var targetFileData = targetWriter.WriteToFile(image);

File.WriteAllBytes(Path.ChangeExtension(source, flagValues[goalFormatFlag]), targetFileData);