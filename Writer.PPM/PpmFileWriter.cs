using System.Text;
using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Writer.PPM;

public class PpmFileWriter : IImageWriter
{
    public byte[] WriteToFile(Color[,] picture)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("P3\n");
        stringBuilder.Append($"{picture.GetLength(1)} {picture.GetLength(0)} 255\n");

        for (int i = 0; i < picture.GetLength(0); i++)
        {
            for (int j = 0; j < picture.GetLength(1); j++)
            {
                Color pix = picture[i, j];
                stringBuilder.Append($"{pix.R} {pix.G} {pix.B} ");
            }

            stringBuilder.Append('\n');
        }

        return Encoding.ASCII.GetBytes(stringBuilder.ToString());
    }

    public string FileExtension => "ppm";
}