using ImageFormatConverter.Console;

const string goalFormatFlag = "goal-format";
const string sourceFlag = "source";
const string outputFlag = "output";

var flagValues = CommandLineArgumentsParser.GetFlagsValues(args, new []{ goalFormatFlag, sourceFlag }, new []{ outputFlag });

var source = flagValues[sourceFlag];
var fileData = FileReader.ReadFile(source);

var fileFactory = new FileFactory();

var fileConverter = fileFactory.CreateFileConverter(fileData, Path.GetExtension(source), flagValues[goalFormatFlag]);
var targetFileData = fileConverter.ConvertImage(fileData);

var fileWriter = new FileWriter(source);
fileWriter.Write(targetFileData, flagValues[goalFormatFlag], flagValues[outputFlag]);