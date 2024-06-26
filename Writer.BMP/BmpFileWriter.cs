﻿using Core.Lights;
using ImageFormatConverter.Abstractions.Interfaces;

namespace Writer.BMP;

public class BmpFileWriter : IImageWriter
{
    public byte[] WriteToFile(Color[,] pixels)
    {
        int height = pixels.GetLength(0);
        int width = pixels.GetLength(1);
        int rowLength = width * 3;
        int numberOfZeroBytes = 0;

        while (rowLength%4 != 0)
        {
            rowLength++;
            numberOfZeroBytes++;
        }
        
        int fileSize = 54 + height * rowLength;

        byte[] bmpHeader = GetBmpHeader(fileSize);
        byte[] bmpInfoHeader = GetInfoHeader(width, height);
        byte[] pixelData = GetPixelData(pixels, numberOfZeroBytes);

        using var stream = new MemoryStream();
        using var binaryWriter = new BinaryWriter(stream);
        binaryWriter.Write(bmpHeader);
        binaryWriter.Write(bmpInfoHeader);
        binaryWriter.Write(pixelData);
        stream.Flush();
        return stream.GetBuffer();
    }

    public string FileExtension => "bmp";

    private static byte[] GetBmpHeader(int fileSize)
    {
        byte[] header = new byte[14];

        header[0] = (byte)'B';
        header[1] = (byte)'M';
        BitConverter.GetBytes(fileSize).CopyTo(header, 2);
        BitConverter.GetBytes(54).CopyTo(header, 10);

        return header;
    }
    
    private static byte[] GetInfoHeader(int width, int height)
    {
        byte[] infoHeader = new byte[40];

        infoHeader[0] = 40;
        BitConverter.GetBytes(width).CopyTo(infoHeader, 4);
        BitConverter.GetBytes(height).CopyTo(infoHeader, 8);
        infoHeader[12] = 1;
        infoHeader[14] = 24;

        return infoHeader;
    }

    private static byte[] GetPixelData(Color[,] pixels, int zeros)
    {
        int height = pixels.GetLength(0);
        int width = pixels.GetLength(1);
        int size = height * (width * 3 + zeros);
        int counter = 0;
        
        byte[] pixelData = new byte[size];

        for (int i = height - 1; i >= 0; i--)
        {
            for (int j = 0; j < width; j++)
            {
                pixelData[counter++] = (byte)pixels[i, j].B;
                pixelData[counter++] = (byte)pixels[i, j].G;
                pixelData[counter++] = (byte)pixels[i, j].R;
            }

            counter += zeros;
        }

        return pixelData;
    }
}