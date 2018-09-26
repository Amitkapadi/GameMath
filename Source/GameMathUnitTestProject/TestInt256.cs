using System;
using GameMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TestInt256
{
    void Verify256Value(Int256 x, ulong a, ulong b, ulong c, ulong d)
    {
        Assert.AreEqual(a, x.A);
        Assert.AreEqual(b, x.B);
        Assert.AreEqual(c, x.C);
        Assert.AreEqual(d, x.D);
    }

    [TestMethod]
    public void TestCTor()
    {
        //public static implicit operator Int256(bool value)
        //public static implicit operator Int256(byte value)
        //public static implicit operator Int256(char value)
        //public static explicit operator Int256(decimal value)
        //public static explicit operator Int256(double value)
        //public static implicit operator Int256(short value)
        //public static implicit operator Int256(int value)
        //public static implicit operator Int256(long value)
        //public static implicit operator Int256(sbyte value)
        //public static explicit operator Int256(float value)
        //public static implicit operator Int256(ushort value)
        //public static implicit operator Int256(uint value)
        //public static implicit operator Int256(ulong value)

        //public static explicit operator bool(Int256 value)
        //public static explicit operator byte(Int256 value)
        //public static explicit operator char(Int256 value)
        //public static explicit operator decimal(Int256 value)
        //public static explicit operator double(Int256 value)
        //public static explicit operator float(Int256 value)
        //public static explicit operator short(Int256 value)
        //public static explicit operator int(Int256 value)
        //public static explicit operator long(Int256 value)
        //public static explicit operator uint(Int256 value)
        //public static explicit operator ushort(Int256 value)
        //public static explicit operator ulong(Int256 value)

        Assert.AreEqual(true, (bool)new Int256(2));
        Assert.AreEqual(12, (float)new Int256(12));
        Assert.AreEqual(0x123456789ABCDEF0u, (ulong)new Int256(0x123456789ABCDEF0));

        Verify256Value(new Int256(1), 0, 0, 0, 1);
        Verify256Value(new Int256(10.0f), 0, 0, 0, 10);
        Verify256Value(new Int256(20.0), 0, 0, 0, 20);
        Verify256Value(new Int256(true), 0, 0, 0, 1);
        Verify256Value(new Int256('A'), 0, 0, 0, 'A');
        Verify256Value(new Int256(), 0, 0, 0, 0);
        Verify256Value(new Int256(1, 2, 3, 4), 1, 2, 3, 4);
        Verify256Value(new Int256(0x12345678), 0, 0, 0, 0x12345678);
        Verify256Value(new Int256(0x12345678, 0xA2A4A6A8, 0xB2B4B6B8, 0xC2C4C6C8), 0x12345678, 0xA2A4A6A8, 0xB2B4B6B8, 0xC2C4C6C8);
        Verify256Value(new Int256(0x123456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4), 0x123456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4);

    }

    [TestMethod]
    public void TestToString()
    {
        Assert.AreEqual(new Int256(0x12345678, 0xA2A4A6A8, 0xB2B4B6B8, 0xC2C4C6C8).ToString("X1"), "0000000012345678 00000000A2A4A6A8 00000000B2B4B6B8 00000000C2C4C6C8");
        Assert.AreEqual(new Int256(0, 0, 0, 1).ToString("X1"), "0000000000000000 0000000000000000 0000000000000000 0000000000000001");
        Assert.AreEqual(new Int256(0x123456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4).ToString("X1"), "123456789ABCDEF0 A2A4A6A8D1D2D3D4 B2B4B6B8C1C2C3C4 C2C4C6C8B1B2B3B4");
    }

    [TestMethod]
    public void TestCompare()
    {
        Assert.AreEqual(0, new Int256(1).CompareTo(new Int256(1)));
        Assert.AreEqual(-1, new Int256(1).CompareTo(new Int256(2)));
        Assert.AreEqual(1, new Int256(2).CompareTo(new Int256(1)));
        Assert.AreEqual(0, new Int256(0x123456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4).CompareTo(new Int256(0x123456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4)));

        //public ulong[] ToUIn64Array()
        //public uint[] ToUIn32Array()

        var t1 = new Int256(0x123456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4);
        ulong[] t2 = t1.ToUIn64Array();
        Assert.AreEqual(t2[0], 0xC2C4C6C8B1B2B3B4u);
        Assert.AreEqual(t2[1], 0xB2B4B6B8C1C2C3C4u);
        Assert.AreEqual(t2[2], 0xA2A4A6A8D1D2D3D4u);
        Assert.AreEqual(t2[3], 0x123456789ABCDEF0u);

        uint[] t3 = t1.ToUIn32Array();
        Assert.AreEqual(t3[0], 0xB1B2B3B4u);
        Assert.AreEqual(t3[1], 0xC2C4C6C8u);
        Assert.AreEqual(t3[2], 0xC1C2C3C4u);
        Assert.AreEqual(t3[3], 0xB2B4B6B8u);
        Assert.AreEqual(t3[4], 0xD1D2D3D4u);
        Assert.AreEqual(t3[5], 0xA2A4A6A8u);
        Assert.AreEqual(t3[6], 0x9ABCDEF0u);
        Assert.AreEqual(t3[7], 0x12345678u);

        //public static bool operator >(Int256 left, Int256 right)
        //public static bool operator <(Int256 left, Int256 right)
        //public static bool operator >=(Int256 left, Int256 right)
        //public static bool operator <=(Int256 left, Int256 right)
        //public static bool operator !=(Int256 left, Int256 right)
        //public static bool operator ==(Int256 left, Int256 right)

        Assert.AreEqual(true, new Int256(1) == new Int256(1));
        Assert.AreEqual(true, new Int256(10) == 10);
        Assert.AreEqual(true, new Int256(9) < 10);
        Assert.AreEqual(true, new Int256(11) > 10);
        Assert.AreEqual(true, new Int256(1) != new Int256(2));
        Assert.AreEqual(true, new Int256(0x123456789ABCDEFA) == 0x123456789ABCDEFA);

        var small1 = new Int256(0x123456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4);
        var big1 = new Int256(0x223456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4);
        var big1b = new Int256(0x223456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4);
        var small2 = new Int256(0x123456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4);
        var big2 = new Int256(0x123456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B5);

        Assert.AreEqual(true, small1 < big1);
        Assert.AreEqual(true, small2 < big2);
        Assert.AreEqual(true, small1 <= big1);
        Assert.AreEqual(true, small2 <= big2);
        Assert.AreEqual(true, big1 > small1);
        Assert.AreEqual(true, big2 > small2);
        Assert.AreEqual(true, big1 >= small1);
        Assert.AreEqual(true, big2 >= small2);
        Assert.AreEqual(true, big1 >= big1b);
        Assert.AreEqual(true, big1 <= big1b);
    }

    [TestMethod]
    public void TestPlusMinusOps()
    {
        //public static Int256 operator +(Int256 value)
        //public static Int256 operator -(Int256 value)
        //public static Int256 operator +(Int256 left, Int256 right)
        //public static Int256 operator -(Int256 left, Int256 right)

        Verify256Value(new Int256(1) + new Int256(1), 0, 0, 0, 2);
        var t1 = new Int256(1);
        t1++;
        Verify256Value(t1, 0, 0, 0, 2);

        Verify256Value(new Int256(5) - new Int256(3), 0, 0, 0, 2);
        var t2 = new Int256(3);
        t2--;
        Verify256Value(t2, 0, 0, 0, 2);

        var big1 = new Int256(0x223456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4);
        Verify256Value(big1 + new Int256(1), 0x223456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B5);
        Verify256Value(big1 + new Int256(1, 0, 0, 0), 0x223456789ABCDEF1, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4);
        Verify256Value(big1 - new Int256(1), 0x223456789ABCDEF0, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B3);
        Verify256Value(big1 - new Int256(1, 0, 0, 0), 0x223456789ABCDEEF, 0xA2A4A6A8D1D2D3D4, 0xB2B4B6B8C1C2C3C4, 0xC2C4C6C8B1B2B3B4);

        var bigWrap1a = new Int256(0xFFFFFFFFFFFFFFFF, 0, 0, 0);
        var bigWrap1b = new Int256(0, 0xFFFFFFFFFFFFFFFF, 0, 0);
        var bigWrap1c = new Int256(0, 0, 0xFFFFFFFFFFFFFFFF, 0);
        var bigWrap1d = new Int256(0, 0, 0, 0xFFFFFFFFFFFFFFFF);
        Verify256Value(bigWrap1a + new Int256(1, 0, 0, 0), 0, 0, 0, 0);
        Verify256Value(bigWrap1b + new Int256(0, 1, 0, 0), 1, 0, 0, 0);
        Verify256Value(bigWrap1c + new Int256(0, 0, 1, 0), 0, 1, 0, 0);
        Verify256Value(bigWrap1d + new Int256(1), 0, 0, 1, 0);
        var bigWrap2a = new Int256(1, 0, 0, 0);
        var bigWrap2b = new Int256(0, 1, 0, 0);
        var bigWrap2c = new Int256(0, 0, 1, 0);
        var bigWrap2d = new Int256(0, 0, 0, 0);
        Verify256Value(bigWrap2a - new Int256(1), 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF);
        Verify256Value(bigWrap2b - new Int256(1), 0, 0, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF);
        Verify256Value(bigWrap2c - new Int256(1), 0, 0, 0, 0xFFFFFFFFFFFFFFFF);
        Verify256Value(bigWrap2d - new Int256(1), 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF);

        //public static Int256 operator ++(Int256 value)
        //public static Int256 operator --(Int256 value)

        bigWrap2d--;
        Assert.AreEqual(true, bigWrap2d == -1);
        bigWrap2d++;
        Assert.AreEqual(true, bigWrap2d == 0);
        bigWrap2d++;
        Assert.AreEqual(true, bigWrap2d == 1);
    }

    [TestMethod]
    public void TestMathOps()
    {
        //public static Int256 operator /(Int256 dividend, Int256 divisor)
        //public static Int256 operator *(Int256 left, Int256 right)

        var m1 = new Int256(0, 0, 0, 2);
        m1 /= 2;
        Verify256Value(m1, 0, 0, 0, 1);

        var m2 = new Int256(0, 0, 1, 0);
        m2 /= 2;
        Verify256Value(m2, 0, 0, 0, 0x8000000000000000);
        m2 /= 2;
        Verify256Value(m2, 0, 0, 0, 0x4000000000000000);
        m2 *= 2;
        Verify256Value(m2, 0, 0, 0, 0x8000000000000000);
        m2 *= 2;
        Verify256Value(m2, 0, 0, 1, 0);
        m2 *= 2;
        Verify256Value(m2, 0, 0, 2, 0);

        //public static Int256 operator %(Int256 dividend, Int256 divisor)
        var m3 = new Int256(0, 0, 0, 13);
        Assert.AreEqual(3, m3 % 10);

        //public static Int256 operator >>(Int256 value, int shift)
        //public static Int256 operator <<(Int256 value, int shift)
        var m4 = new Int256(0, 0, 1, 0);
        m4 >>= 2;
        Verify256Value(m4, 0, 0, 0, 0x4000000000000000);
        m4 <<= 3;
        Verify256Value(m4, 0, 0, 2, 0);

        //public static Int256 operator |(Int256 left, Int256 right)
        //public static Int256 operator &(Int256 left, Int256 right)
        var m5a = new Int256(0, 0, 1, 0);
        var m5b = new Int256(2, 2, 2, 2);
        m5a |= m5b;
        Verify256Value(m5a, 2, 2, 3, 2);

        var m6a = new Int256(0, 0, 1, 0);
        var m6b = new Int256(2, 2, 3, 2);
        m6a &= m6b;
        Verify256Value(m6a, 0, 0, 1, 0);

        //public static Int256 operator ~(Int256 value)
    }
}

