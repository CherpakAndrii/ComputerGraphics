using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;

namespace ImageFormatConverter.Writers;

public class PpmFileWriter : IImageWriter
{
    public void WriteToFile(string outputFileName, Color[,] picture)
    {
        StreamWriter streamWriter = new StreamWriter(outputFileName);
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
        
        streamWriter.Close();
    }
}