namespace StructTest;

public class SphereTests
{
    [Test]
    public void Sphere_WhenCallNormalMethod_NormalVectorReceived()
    {
        Sphere sphere = new(new Point(0, 0, 0), 1);
        Vector expectedNormal = new(0, 0, 1);
        Point pointForNormal = new(0, 0, 1);

        var actualNormal = sphere.GetNormalVector(pointForNormal);
        
        Assert.That(actualNormal, Is.EqualTo(expectedNormal));
    }
    
    [TestCaseSource(nameof(_intersectionCases))]
    public void Sphere_WhenIntersectsWithRay_CorrectIntersectionPointReturned
    (
        Sphere sphere,
        Ray ray,
        Point expectedIntersectionPoint)
    {
         var actualIntersectionPoint = sphere.GetIntersectionWith(ray);

        Assert.Multiple(() =>
        {
            Assert.That(actualIntersectionPoint is not null, Is.True);
            Assert.That(actualIntersectionPoint, Is.EqualTo(expectedIntersectionPoint));
        });
    }
    
    [TestCaseSource(nameof(_intersectionCases))]
    public void Sphere_WhenIntersectsWithRay_TrueReturned
    (
        Sphere sphere,
        Ray ray,
        Point expectedIntersectionPoint)
    {
        var hasIntersection = sphere.HasIntersectionWith(ray);

        Assert.That(hasIntersection, Is.True);
    }
    
    [TestCaseSource(nameof(_noIntersectionCases))]
    public void Sphere_WhenRayInOppositeDirection_NoIntersection(Sphere sphere, Ray ray)
    {
        var intersection = sphere.GetIntersectionWith(ray);
        
        Assert.That(intersection, Is.Null);
    }
    
    [TestCaseSource(nameof(_noIntersectionCases))]
    public void Sphere_WhenRayInOppositeDirection_FalseReturned(Sphere sphere, Ray ray)
    {
        var hasIntersection = sphere.HasIntersectionWith(ray);
        
        Assert.That(hasIntersection, Is.False);
    }

    private static object[] _intersectionCases =
    {
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 0, 5), new Vector(0, 0, -1)),
            new Point(0, 0, 1)
        },
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 0, -5), new Vector(0, 0, 1)),
            new Point(0, 0, -1)
        },
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 5, 0), new Vector(0, -1, 0)),
            new Point(0, 1, 0)
        }
    };

    private static object[] _noIntersectionCases =
    {
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 0, 5), new Vector(0, 0, 1))
        },
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, 0, -5), new Vector(0, 0, -1)),
        },
        new object[]
        {
            new Sphere(new Point(0, 0, 0), 1),
            new Ray(new Point(0, -5, 0), new Vector(0, 1, 1))
        }
    };
}