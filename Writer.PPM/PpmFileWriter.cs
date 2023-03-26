using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Writer.PPM;

public class PpmFileWriter : IImageWriter
{
    public byte[] WriteToFile(Color[,] picture)
    {
        using var stream = new MemoryStream();
        using var streamWriter = new StreamWriter(stream);
        streamWriter.WriteLine("P3");
        streamWriter.WriteLine($"{picture.GetLength(1)} {picture.GetLength(0)} 255");

        for (int i = 0; i < picture.GetLength(0); i++)
        {
            for (int j = 0; j < picture.GetLength(1); j++)
            {
                Color pix = picture[i, j];
                streamWriter.Write($"{pix.R} {pix.G} {pix.B} ");
            }

            streamWriter.WriteLine();
        }
        
        stream.Flush();
        return stream.GetBuffer();
    }

    public string FileExtension => "ppm";
}