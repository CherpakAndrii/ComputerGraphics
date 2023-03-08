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
}