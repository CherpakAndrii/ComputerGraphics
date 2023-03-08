namespace StructTest;

public class DiskTests
{
    [Test]
    public void Disk_WhenCallNormalMethod_NormalVectorReceived()
    {
        Disk plane = new(new Point(0, 0, 0), new Vector(0, 0, 1), 5);
        Vector expectedNormal = new(0, 0, 1);
        Point pointForNormal = new(0, 0, 0);

        var actualNormal = plane.GetNormalVector(pointForNormal);
        
        Assert.That(actualNormal, Is.EqualTo(expectedNormal));
    }
    
    [TestCaseSource(nameof(_intersectionCases))]
    public void Disk_WhenIntersectsWithRay_CorrectIntersectionPointReturned
    (
        Disk disk,
        Ray ray,
        Point expectedIntersectionPoint
    )
    {
        var actualIntersectionPoint = disk.GetIntersectionWith(ray);

        Assert.Multiple(() =>
        {
            Assert.That(actualIntersectionPoint is not null, Is.True);
            Assert.That(actualIntersectionPoint, Is.EqualTo(expectedIntersectionPoint));
        });
    }
    
    private static object[] _intersectionCases =
    {
        new object[]
        {
            new Disk(new Point(0, 0, 0), new Vector(0, 0, 1), 5),
            new Ray(new Point(0, 0, 1), new Vector(0, 0, -1)),
            new Point(0, 0, 0)
        },
        new object[]
        {
            new Disk(new Point(0, 0, 0), new Vector(0, 0, 1), 5),
            new Ray(new Point(0, 0, -1), new Vector(0, 0, 1)),
            new Point(0, 0, 0)
        },
        new object[]
        {
            new Disk(new Point(0, 0, 0), new Vector(0, 0, 1), 5),
            new Ray(new Point(0, 4, -1), new Vector(0, 0, 1)),
            new Point(0, 4, 0)
        }
    };
}