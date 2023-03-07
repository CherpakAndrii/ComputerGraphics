namespace StructTest;

public class PlaneTests
{
    [Test]
    public void Plane_WhenCallNormalMethod_NormalVectorReceived()
    {
        Plane plane = new(new Point(0, 0, 0), new Vector(0, 0, 1));
        Vector expectedNormal = new(0, 0, 1);
        Point pointForNormal = new(0, 0, 0);

        var actualNormal = plane.GetNormalVector(pointForNormal);
        
        Assert.That(actualNormal, Is.EqualTo(expectedNormal));
    }
    
    [TestCaseSource(nameof(_intersectionCases))]
    public void Plane_WhenIntersectsWithRay_CorrectIntersectionPointReturned
    (
        Plane plane,
        Ray ray,
        Point expectedIntersectionPoint
    )
    {
        var actualIntersectionPoint = plane.GetIntersectionWith(ray);

        Assert.Multiple(() =>
        {
            Assert.That(actualIntersectionPoint is not null, Is.True);
            Assert.That(actualIntersectionPoint, Is.EqualTo(expectedIntersectionPoint));
        });
    }
    
    [TestCaseSource(nameof(_intersectionCases))]
    public void Plane_WhenIntersectsWithRay_TrueReturned
    (
        Plane plane,
        Ray ray,
        Point expectedIntersectionPoint
    )
    {
        var hasIntersection = plane.HasIntersectionWith(ray);

        Assert.That(hasIntersection, Is.True);
    }
    
    [TestCaseSource(nameof(_noIntersectionCases))]
    public void Plane_WhenNoIntersectionWithRay_NullReturned(Plane plane, Ray ray)
    {
        var intersection = plane.GetIntersectionWith(ray);

        Assert.That(intersection, Is.Null);
    }
    
    [TestCaseSource(nameof(_noIntersectionCases))]
    public void Plane_WhenNoIntersectionWithRay_FalseReturned(Plane plane, Ray ray)
    {
        var intersection = plane.HasIntersectionWith(ray);

        Assert.That(intersection, Is.False);
    }

    private static object[] _intersectionCases =
    {
        new object[]
        {
            new Plane(new Point(0, 0, 0), new Vector(0, 0, 1)),
            new Ray(new Point(0, 0, 1), new Vector(0, 0, -1)),
            new Point(0, 0, 0)
        },
        new object[]
        {
            new Plane(new Point(0, 0, 0), new Vector(0, 0, 1)),
            new Ray(new Point(0, 0, -1), new Vector(0, 0, 1)),
            new Point(0, 0, 0)
        }
    };
    
    private static object[] _noIntersectionCases =
    {
        new object[]
        {
            new Plane(new Point(0, 0, 0), new Vector(0, 0, 1)),
            new Ray(new Point(0, 0, 0), new Vector(0, 1, 0))
        },
        new object[]
        {
            new Plane(new Point(0, 0, 0), new Vector(0, 0, 1)),
            new Ray(new Point(0, 0, -1), new Vector(0, 1, 0))
        }
    };
}