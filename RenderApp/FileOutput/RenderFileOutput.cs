using Core;
using Core.Lights;
using ImageFormatConverter.Console;

namespace RenderApp.FileOutput;

public class RenderFileOutput : IRenderOutput
{
    private string _output;
    
    public RenderFileOutput(string output)
    {
        _output = output;
    }

    public void CreateRenderResult(Color[,] pixels)
    {
        var fileFactory = new FileFactory();
        var imageWriter = fileFactory.GetImageWriter("png");

        var fileData = imageWriter.WriteToFile(pixels);

        var fileWriter = new FileWriter(_output);
        fileWriter.Write(fileData, $".{imageWriter.FileExtension}", _output, "picture");
    }
}