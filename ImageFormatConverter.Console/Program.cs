using ImageFormatConverter.Console;

const string goalFormatFlag = "goal-format";
const string sourceFlag = "source";
const string outputFlag = "output";

var flagValues = CommandLineArgumentsParser.GetFlagsValues(args, new []{ goalFormatFlag, sourceFlag }, new []{ outputFlag });

var source = flagValues[sourceFlag];

var fileData = FileReader.ReadFile(source);

var fileFactory = new FileFactory();
var targetReader = fileFactory.GetImageReader(fileData, Path.GetExtension(source));
var targetWriter = fileFactory.GetImageWriter(flagValues[goalFormatFlag]);

var image = targetReader.ImageToPixels(fileData);

var targetFileData = targetWriter.WriteToFile(image);

var path = flagValues.TryGetValue(outputFlag, out var output)
    ? output + flagValues[goalFormatFlag]
    : Path.ChangeExtension(source, flagValues[goalFormatFlag]);

File.WriteAllBytes(path, targetFileData);