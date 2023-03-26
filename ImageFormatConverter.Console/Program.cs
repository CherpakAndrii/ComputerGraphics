﻿using ImageFormatConverter.Console;

const string goalFormatFlag = "goal-format";
const string sourceFlag = "source";
const string outputFlag = "output";

var flagValues = CommandLineArgumentsParser.GetFlagsValues(args, new []{ goalFormatFlag, sourceFlag }, new []{ outputFlag });

var source = flagValues[sourceFlag];

var fileData = FileReader.ReadFile(source);

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
{
    var supportedWriterFormats = fileFactory.GetSupportedWritersExtensions().Select(format => $".{format}");
    var goalFormat = flagValues[goalFormatFlag];

    throw new Exception($"Error: you are trying to write .{goalFormat} file, but only {string.Join(" and ", supportedWriterFormats)} files are supported");
}

var image = targetReader.ImageToPixels(fileData);

var targetFileData = targetWriter.WriteToFile(image);

var path = flagValues.TryGetValue(outputFlag, out var output)
    ? output + flagValues[goalFormatFlag]
    : Path.ChangeExtension(source, flagValues[goalFormatFlag]);

File.WriteAllBytes(path, targetFileData);