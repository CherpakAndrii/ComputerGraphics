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
    
    [Test]
    public void Plane_WhenIntersectsWithRay_CorrectIntersectionPointReturned()
    {
        Plane plane = new(new Point(0, 0, 0), new Vector(0, 0, 1));
        Ray ray = new(new Point(0, 0, 1), new Vector(0, 0, -1));
        Point expectedIntersectionPoint = new(0, 0, 0);
        
        var actualIntersectionPoint = plane.GetIntersectionWith(ray);

        Assert.Multiple(() =>
        {
            Assert.That(actualIntersectionPoint is not null, Is.True);
            Assert.That(actualIntersectionPoint, Is.EqualTo(expectedIntersectionPoint));
        });
    }
}