namespace ImageFormatConverter.Interfaces;

public interface IImageFileStructureValidator
{
    public bool ValidateFileStructure(byte[] fileData);
}