using Vector = Structures.Vector;

namespace StructTest;

public class VectorTests
{
    [Test]
    public void VectorPlusVectorTest()
    {
        Vector vector1 = new Vector(1, 1, 1);
        Vector vector2 = new Vector(2, 3, 5);

        Vector result = vector1 + vector2;
        
        Assert.AreEqual(new Vector(3, 4, 6), result);
    }
    
    [Test]
    public void VectorMinusVectorTest()
    {
        Vector vector1 = new Vector(1, 1, 1);
        Vector vector2 = new Vector(2, 3, 5);

        Vector result = vector1 - vector2;
        
        Assert.AreEqual(new Vector(-1, -2, -4), result);
    }
    
    [Test]
    public void VectorModuleSimpleTest()
    {
        Vector vector = new Vector(0, 3, 4);

        float module = vector.GetModule();

        Assert.AreEqual(5, module);
    }
    
    [Test]
    public void VectorModuleTest()
    {
        Vector vector = new Vector(11, 13, 19);

        float module = vector.GetModule();

        Assert.AreEqual((float)Math.Sqrt(Math.Pow(11, 2)+Math.Pow(13, 2)+Math.Pow(19, 2)), module);
    }
    
    [Test]
    public void VectorNormalizeTest()
    {
        Vector vector = new Vector(112, 132, 197);
        vector.Normalize();

        float module = vector.GetModule();

        Assert.That(1 - module < 1e-6, Is.True);
    }
    
    [Test]
    public void VectorIsCollinearTest()
    {
        Vector vector1 = new Vector(5, -7, 4);
        Vector vector2 = new Vector(-7.5f, 10.5f, -6);

        bool isCollinear = vector1.IsCollinearTo(vector2);

        Assert.That(isCollinear, Is.True);
    }
    
    [Test]
    public void VectorIsPerpendicularTest__False()
    {
        Vector vector1 = new Vector(5, -7, 4);
        Vector vector2 = new Vector(-7.5f, 10.5f, -6);

        bool isPerpendicular = vector1.IsPerpendicularTo(vector2);

        Assert.That(isPerpendicular, Is.False);
    }
    
    [Test]
    public void VectorIsPerpendicularTest__True()
    {
        Vector vector1 = new Vector(2, 3, 1);
        Vector vector2 = new Vector(-3, 4, -6);

        bool isPerpendicular = vector1.IsPerpendicularTo(vector2);

        Assert.That(isPerpendicular, Is.True);
    }
    
    [Test]
    public void VectorMultiplicationTest()
    {
        Vector vector1 = new Vector(-5, 7, 2);
        Vector vector2 = new Vector(2, -3, 4);

        float result = vector1*vector2;

        Assert.AreEqual(-23f, result);
    }

    [Test]
    public void VectorCrossProductTest1()
    {
        Vector vector1 = new Vector(1, 2, 3);
        Vector vector2 = new Vector(9, 8, 7);

        Vector result = vector1.CrossProductWith(vector2);

        Assert.AreEqual(new Vector(-10, 20, -10), result);
    }
    
    [Test]
    public void VectorCrossProductTest2()
    {
        Vector vector1 = new Vector(-5, 7, 2);
        Vector vector2 = new Vector(2, -3, 4);

        Vector result = vector1.CrossProductWith(vector2);

        Assert.AreEqual(new Vector(34, 24, 1), result);
    }
    
    [Test]
    public void AngleBetweenVectorsTest()
    {
        Vector vector1 = new Vector(2, 3, 1);
        Vector vector2 = new Vector(-3, 4, -6);

        float result = vector1.GetAngleWith(vector2);
        float error = (float)Math.Abs(result - Math.PI/2);
        
        Assert.That(error<1e-6f, Is.True);
    }

    [Test]
    public void AngleBetweenVectorsTest2()
    {
        Vector vector1 = new Vector(3, 0, -3);
        Vector vector2 = new Vector(-1, 1, 2);

        float result = vector1.GetAngleWith(vector2);
        float error = (float)Math.Abs(result - 5*Math.PI/6);
        
        Assert.That(error<1e-6f, Is.True);
    }
}