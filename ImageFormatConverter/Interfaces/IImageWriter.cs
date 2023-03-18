﻿using Core.Lights;

namespace ImageFormatConverter.Interfaces;

public interface IImageWriter
{
    public void WriteToFile(string outputFileName, Color[,] pixels);
}