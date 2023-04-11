using Core;
using Core.Lights;
using ImageFormatConverter.Common;
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
        var targetExtension = _output.Split('.').Last();
        
        var fileFactory = new FileFactory();
        var imageWriter = fileFactory.GetImageWriter(targetExtension);
        
        var fileData = imageWriter.WriteToFile(pixels);

        File.WriteAllBytes(_output, fileData);
    }
}