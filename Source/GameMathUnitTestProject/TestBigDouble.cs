using System;
using GameMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TestBigDouble
{
    [TestMethod]
    public void TestCTor()
    {
        BigDouble bd1 = new BigDouble(1.0f, 10);
        Assert.AreEqual(1.0f, bd1.Value);
        Assert.AreEqual(10u, bd1.Exp);

        BigDouble bd2 = new BigDouble(123.4);
        Assert.AreEqual(0.1234, bd2.Value, 0.01);
        Assert.AreEqual(3u, bd2.Exp);
        Assert.AreEqual(123.4, bd2.RealValue, 0.01);

        BigDouble bd3 = -bd2;
        Assert.AreEqual(-123.4, bd2.RealValue, 0.01);

        BigDouble bd4 = new BigDouble(12.34E+30);
        Assert.AreEqual(0.1234, bd4.Value, 0.01);
        Assert.AreEqual(32u, bd4.Exp);
        Assert.AreEqual(12.34E+30, bd4.RealValue, 0.01);

        BigDouble bd5 = new BigDouble(1.0E+301);
        Assert.AreEqual(0.1, bd5.Value, 0.01);
        Assert.AreEqual(302u, bd5.Exp);
        Assert.AreEqual(1.0E+301, bd5.RealValue, 0.00001E+307);
    }

    [TestMethod]
    public void TestCompare()
    {
        BigDouble bd1a = new BigDouble(5.0, 10000u);
        BigDouble bd1b = new BigDouble(3.0, 10000u);
        BigDouble bd1c = new BigDouble(3.0, 100u);
        Assert.IsTrue(bd1a > bd1b);
        Assert.IsTrue(bd1a > bd1c);
        Assert.IsTrue(bd1a >= bd1b);
        Assert.IsTrue(bd1b < bd1a);
        Assert.IsTrue(bd1c < bd1b);
        Assert.IsTrue(bd1b <= bd1a);
    }

    [TestMethod]
    public void TestAddSub()
    {
        BigDouble bd1a = new BigDouble(5.0, 10000u);
        BigDouble bd1b = new BigDouble(3.0, 10000u);
        BigDouble bd1c = bd1a - bd1b;
        Assert.AreEqual(0.2, bd1c.Value, 0.0001);
        Assert.AreEqual(10001u, bd1c.Exp);

        BigDouble bd6 = new BigDouble(1.0, 10000u);
        BigDouble bd6b = bd6;
        Assert.AreEqual(1.0, bd6.Value, 0.0001);
        Assert.AreEqual(10000u, bd6.Exp);
        BigDouble bd6c = bd6 + bd6b;
        Assert.AreEqual(0.2, bd6c.Value, 0.0001);
        Assert.AreEqual(10001u, bd6c.Exp);

        BigDouble bd6d = bd6c - bd6;
        Assert.AreEqual(0.2, bd6c.Value, 0.0001);
        Assert.AreEqual(10001u, bd6c.Exp);

    }

    [TestMethod]
    public void TestMul()
    {
        {
            BigDouble bd1a = new BigDouble(10.0, 0);
            BigDouble bd1b = new BigDouble(10.0, 0);
            BigDouble bd1c = bd1a * bd1b;
            Assert.AreEqual(0.1, bd1c.Value, 0.0001);
            Assert.AreEqual(3u, bd1c.Exp);
            Assert.AreEqual(100.0, bd1c.RealValue, 0.001);
        }

        {
            BigDouble bd1a = new BigDouble(0.2, 2);
            BigDouble bd1b = new BigDouble(0.3, 3);
            BigDouble bd1c = bd1a * bd1b;
            Assert.AreEqual(0.6, bd1c.Value, 0.0001);
            Assert.AreEqual(4u, bd1c.Exp);
            Assert.AreEqual(6000.0, bd1c.RealValue, 0.001);
        }

        {
            BigDouble bd1a = new BigDouble(750);
            BigDouble bd1b = new BigDouble(1250);
            BigDouble bd1c = bd1a * bd1b;
            Assert.AreEqual(937500.0, bd1c.RealValue, 0.001);
        }
    }

    [TestMethod]
    public void TestDiv()
    {
        {
            BigDouble bd1a = new BigDouble(200);
            BigDouble bd1b = new BigDouble(5);
            BigDouble bd1c = bd1a / bd1b;
            Assert.AreEqual(0.4, bd1c.Value, 0.0001);
            Assert.AreEqual(2u, bd1c.Exp);
            Assert.AreEqual(40.0, bd1c.RealValue, 0.001);
        }

        {
            BigDouble bd1a = new BigDouble(2000000);
            BigDouble bd1b = new BigDouble(500);
            BigDouble bd1c = bd1a / bd1b;
            Assert.AreEqual(0.4, bd1c.Value, 0.0001);
            Assert.AreEqual(4u, bd1c.Exp);
            Assert.AreEqual(4000.0, bd1c.RealValue, 0.001);
        }
    }

    [TestMethod]
    public void TestPrecision()
    {
        // a 64-bit double has 53 bits of fraction for ≈15.95 decimal digits of precision
        BigDouble b1 = new BigDouble(1.0, 100);
        Assert.AreEqual(1.0E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(2.0, 99);
        Assert.AreEqual(1.2E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(3.0, 98);
        Assert.AreEqual(1.23E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(4.0, 97);
        Assert.AreEqual(1.234E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(5.0, 96);
        Assert.AreEqual(1.2345E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(6.0, 95);
        Assert.AreEqual(1.23456E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(7.0, 94);
        Assert.AreEqual(1.234567E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(8.0, 93);
        Assert.AreEqual(1.2345678E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(9.0, 92);
        Assert.AreEqual(1.23456789E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(1.0, 91);
        Assert.AreEqual(1.234567891E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(2.0, 90);
        Assert.AreEqual(1.2345678912E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(3.0, 89);
        Assert.AreEqual(1.23456789123E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(4.0, 88);
        Assert.AreEqual(1.234567891234E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(5.0, 87);
        Assert.AreEqual(1.2345678912345E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(6.0, 86);
        Assert.AreEqual(1.23456789123456E+100, b1.RealValue, 1E+85);
        b1 += new BigDouble(7.0, 85);
        Assert.AreEqual(1.234567891234567E+100, b1.RealValue, 1E+85);

        //b1 += new BigDouble(8.0, 84);
        //Assert.AreEqual(1.2345678912345677E+100, b1.RealValue, 1E+84);
        //b1 += new BigDouble(9.0, 83);
        //Assert.AreEqual(1.23456789123456789E+100, b1.RealValue, 1E+83);
        //b1 += new BigDouble(1.0, 82);
        //Assert.AreEqual(1.234567891234567891E+100, b1.RealValue, 1E+82);
        //b1 += new BigDouble(2.0, 81);
        //Assert.AreEqual(1.2345678912345678912E+100, b1.RealValue, 1E+81);
        //b1 += new BigDouble(3.0, 80);
        //Assert.AreEqual(1.23456789123456789123E+100, b1.RealValue, 1E+80);
    }

}
