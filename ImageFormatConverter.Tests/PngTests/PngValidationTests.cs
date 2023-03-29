// using Reader.PNG;
// using Writer.PNG;
//
// namespace ImageFormatConverter.Tests.PngTests;
//
// [TestFixture]
// public class PngValidationTests : ImageValidationTests
// {
//     [SetUp]
//     public void Setup()
//     {
//         SourceDir = TestPicturesDir + "png/";
//         _imgFileReader = new PngFileReader();
//         _imgWriter = new PngFileWriter();
//     }
//
//
//     [TestCaseSource(nameof(_validFiles))]
//     public void BMP_ValidateCorrectSample_TrueReturned(string filename)
//     {
//         ValidateCorrectSample_TrueReturned(filename);
//     }
//
//     [TestCaseSource(nameof(_invalidFiles))]
//     public void BMP_ValidateIncorrectSample_FalseReturned(string filename)
//     {
//         ValidateIncorrectSample_FalseReturned(filename);
//     }
//
//     protected static object[] _validFiles = new object[]
//     {
//         /*new object[] { "correct/correct_sample.bmp" }*/
//     };
//
//     protected static object[] _invalidFiles = new object[]
//     {
//         /*new object[] { "incorrect/empty.bmp" }*/
//     };
// }