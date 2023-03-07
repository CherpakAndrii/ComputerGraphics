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
}