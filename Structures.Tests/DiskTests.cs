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
    
    [TestCaseSource(nameof(_intersectionCases))]
    public void Disk_WhenIntersectsWithRay_TrueReturned
    (
        Disk disk,
        Ray ray,
        Point expectedIntersectionPoint
    )
    {
        var hasIntersection = disk.HasIntersectionWith(ray);

        Assert.That(hasIntersection, Is.True);
    }
    
    [TestCaseSource(nameof(_noIntersectionCases))]
    public void Disk_WhenNoIntersectionWithRay_NullReturned(Disk disk, Ray ray)
    {
        var intersection = disk.GetIntersectionWith(ray);

        Assert.That(intersection, Is.Null);
    }
    
    [TestCaseSource(nameof(_noIntersectionCases))]
    public void Disk_WhenNoIntersectionWithRay_FalseReturned(Disk disk, Ray ray)
    {
        var intersection = disk.HasIntersectionWith(ray);

        Assert.That(intersection, Is.False);
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
    
    private static object[] _noIntersectionCases =
    {
        new object[]
        {
            new Disk(new Point(0, 0, 0), new Vector(0, 0, 1), 5),
            new Ray(new Point(0, 0, 1), new Vector(0, 0, 1))
        },
        new object[]
        {
            new Disk(new Point(0, 0, 0), new Vector(0, 0, 1), 5),
            new Ray(new Point(0, 6, -1), new Vector(0, 0, 1))
        },
    };
}