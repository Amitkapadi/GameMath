// Licensed under the MIT license. 
// See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using BigMath.Utils;

namespace GameMath
{
    /// <summary>
    /// A standard double has a max of ~1.79 E+308, so only 308 zeros.
    /// 
    /// BigDouble is float point number, but can handle a 64 bit number of 0s
    /// this means 18,446,744,073,709,551,615 is max # of 0s
    /// 
    /// BigDouble is defined as [Value * 10 ^ Exp]
    /// where Value is a 64-bit double which has 53 bits of fraction 
    /// for ≈15.95 decimal digits of precision
    /// while Exp is a 64bit ulong which stores the # of 0s
    /// </summary>
    public class BigDouble
    {
        public double Value;
        public ulong Exp; // 18,446,744,073,709,551,615 is max # of 0s

        public BigDouble()
        {
        }

        public BigDouble(double d)
        {
            ConvertFromDouble(d);
        }

        public BigDouble(byte value) : this((double)value)
        {
        }

        public BigDouble(bool value) 
        {
            this.Value = value ? 1 : 0;
        }

        public BigDouble(char value) : this((double)value)
        {
        }

        public BigDouble(decimal value) : this((double)value)
        {
        }

        public BigDouble(float value) : this((double)value)
        {
        }

        public BigDouble(short value) : this((double)value)
        {
        }

        public BigDouble(int value) : this((double)value)
        {
        }

        public BigDouble(long value) : this((double)value)
        {
        }

        public BigDouble(sbyte value) : this((double)value)
        {
        }

        public BigDouble(ushort value) : this((double)value)
        {
        }

        public BigDouble(uint value) : this((double)value)
        {
        }

        public BigDouble(ulong value) : this((double)value)
        {
        }

        public BigDouble(double value, ulong exp)
        {
            Value = value;
            Exp = exp;
        }

        public double RealValue
        {
            get
            {
                if (this.Exp < 308)
                {
                    double expandedPower = Math.Pow(10.0, this.Exp);
                    double realValue = this.Value * expandedPower;
                    return realValue;
                }
                else
                {
                    return double.MaxValue;
                }
            }
        }

        void ConvertFromDouble(double d)
        {
            ulong newExp = 0;
            double newValue = d;
            while (newValue >= 1.0)
            {
                newValue /= 10.0;
                newExp++;
            }

            Value = newValue;
            Exp = newExp;
        }

        void Simplify()
        {
            if(Value >= 1.0 )
            {
                ulong newExp = Exp;
                double newValue = Value;

                while (newValue >= 1.0)
                {
                    newValue /= 10.0;
                    newExp++;
                }

                Value = newValue;
                Exp = newExp;
            }
            else if (Value < 0.1 && Value >= 0.0)
            {
                ulong newExp = Exp;
                double newValue = Value;

                while (newValue < 0.1)
                {
                    newValue *= 10.0;
                    newExp--;
                }

                Value = newValue;
                Exp = newExp;
            }
        }

        public static BigDouble operator+(BigDouble a, BigDouble b)
        {
            if (a.Exp == b.Exp)
            {
                BigDouble m = new BigDouble();
                m.Exp = a.Exp;
                m.Value = a.Value + b.Value;
                m.Simplify();
                return m;
            }
            else if ( a.Exp > b.Exp)
            {
                // a is bigger
                ulong deltaExp = a.Exp - b.Exp;
                if (deltaExp <= 16)
                {
                    double bX = b.Value / Math.Pow(10, deltaExp);
                    BigDouble m = new BigDouble();
                    m.Exp = a.Exp;
                    m.Value = a.Value + bX;
                    m.Simplify();
                    return m;
                }
                else
                {
                    return a;
                }
            }
            else
            {
                // b is bigger
                ulong deltaExp = b.Exp - a.Exp;
                if (deltaExp <= 16)
                {
                    double aX = a.Value / Math.Pow(10, deltaExp);
                    BigDouble m = new BigDouble();
                    m.Exp = b.Exp;
                    m.Value = b.Value + aX;
                    m.Simplify();
                    return m;
                }
                else
                {
                    return b;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Negate()
        {
            Value = -Value;
        }
        
        public static int Compare(BigDouble left, object right)
        {
            if (right is BigDouble)
            {
                return Compare(left, (BigDouble)right);
            }

            if (right is bool)
            {
                return Compare(left, new BigDouble((bool)right));
            }

            if (right is byte)
            {
                return Compare(left, new BigDouble((byte)right));
            }

            if (right is char)
            {
                return Compare(left, new BigDouble((char)right));
            }

            if (right is decimal)
            {
                return Compare(left, new BigDouble((decimal)right));
            }

            if (right is double)
            {
                return Compare(left, new BigDouble((double)right));
            }

            if (right is short)
            {
                return Compare(left, new BigDouble((short)right));
            }

            if (right is int)
            {
                return Compare(left, new BigDouble((int)right));
            }

            if (right is long)
            {
                return Compare(left, new BigDouble((long)right));
            }

            if (right is sbyte)
            {
                return Compare(left, new BigDouble((sbyte)right));
            }

            if (right is float)
            {
                return Compare(left, new BigDouble((float)right));
            }

            if (right is ushort)
            {
                return Compare(left, new BigDouble((ushort)right));
            }

            if (right is uint)
            {
                return Compare(left, new BigDouble((uint)right));
            }

            if (right is ulong)
            {
                return Compare(left, new BigDouble((ulong)right));
            }

            throw new ArgumentException();
        }

        public int Sign
        {
            get
            {
                if (this.Value == 0)
                {
                    return 0;
                }

                return Value > 0.0 ? 1 : -1;
            }
        }

        public static int Compare(BigDouble left, BigDouble right)
        {
            int leftSign = left.Sign;
            int rightSign = right.Sign;

            // Compare signs
            if (leftSign == 0 && rightSign == 0)
            {
                return 0;
            }

            if (leftSign >= 0 && rightSign < 0)
            {
                return 1;
            }

            if (leftSign < 0 && rightSign >= 0)
            {
                return -1;
            }

            // Compare exponents
            if (left.Exp > right.Exp)
            {
                return 1;
            }

            if (left.Exp < right.Exp)
            {
                return -1;
            }

            return left.Value.CompareTo(right.Value);
        }


        public override int GetHashCode()
        {
            return this.Value.GetHashCode() ^ this.Exp.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public int CompareTo(BigDouble value)
        {
            return Compare(this, value);
        }

        public BigDouble ToAbs()
        {
            return Abs(this);
        }

        public static BigDouble Abs(BigDouble value)
        {
            if (value.Sign < 0)
            {
                return -value;
            }

            return value;
        }

        public static BigDouble Add(BigDouble left, BigDouble right)
        {
            return left + right;
        }

        public static BigDouble Subtract(BigDouble left, BigDouble right)
        {
            return left - right;
        }

        public static BigDouble Divide(BigDouble dividend, BigDouble divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }

            BigDouble bd = new BigDouble(
                dividend.Value / divisor.Value, 
                dividend.Exp - divisor.Exp);
            bd.Simplify();
            return bd;
        }

        public static BigDouble Multiply(BigDouble left, BigDouble right)
        {
            BigDouble bd = new BigDouble(
                left.Value * right.Value, 
                left.Exp + right.Exp);
            bd.Simplify();
            return bd;
        }

        public static implicit operator BigDouble(bool value)
        {
            return new BigDouble(value);
        }

        public static implicit operator BigDouble(byte value)
        {
            return new BigDouble(value);
        }

        public static implicit operator BigDouble(char value)
        {
            return new BigDouble(value);
        }

        public static explicit operator BigDouble(decimal value)
        {
            return new BigDouble(value);
        }

        public static explicit operator BigDouble(double value)
        {
            return new BigDouble(value);
        }

        public static implicit operator BigDouble(short value)
        {
            return new BigDouble(value);
        }

        public static implicit operator BigDouble(int value)
        {
            return new BigDouble(value);
        }

        public static implicit operator BigDouble(long value)
        {
            return new BigDouble(value);
        }

        public static implicit operator BigDouble(sbyte value)
        {
            return new BigDouble(value);
        }

        public static explicit operator BigDouble(float value)
        {
            return new BigDouble(value);
        }

        public static implicit operator BigDouble(ushort value)
        {
            return new BigDouble(value);
        }

        public static implicit operator BigDouble(uint value)
        {
            return new BigDouble(value);
        }

        public static implicit operator BigDouble(ulong value)
        {
            return new BigDouble(value);
        }

        public static explicit operator bool(BigDouble value)
        {
            return value.Sign != 0;
        }

        public static explicit operator byte(BigDouble value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }

            if ((value.RealValue < byte.MinValue) || (value.RealValue > byte.MaxValue))
            {
                throw new OverflowException();
            }

            return (byte)value.RealValue;
        }

        public static explicit operator char(BigDouble value)
        {
            if (value.Sign == 0)
            {
                return (char)0;
            }

            if ((value.RealValue < char.MinValue) || (value.RealValue > char.MaxValue))
            {
                throw new OverflowException();
            }

            return (char)(ushort)value.RealValue;
        }

        public static explicit operator double(BigDouble value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }

            if(value.Exp > 307)
            {
                throw new OverflowException();
            }

            return value.RealValue;
        }

        public static explicit operator float(BigDouble value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }

            if( value.Exp > 37 )
            {
                throw new OverflowException();
            }

            return (float)value.RealValue;
        }

        public static explicit operator short(BigDouble value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }

            if ((value.RealValue < short.MinValue) || (value.RealValue > short.MaxValue))
            {
                throw new OverflowException();
            }

            return (short)value.RealValue;
        }

        public static explicit operator int(BigDouble value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }

            if ((value.RealValue < int.MinValue) || (value.RealValue > int.MaxValue))
            {
                throw new OverflowException();
            }

            return ((int)value.RealValue);
        }

        public static explicit operator long(BigDouble value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }

            if ((value.RealValue < long.MinValue) || (value.RealValue > long.MaxValue))
            {
                throw new OverflowException();
            }

            return (long)value.RealValue;
        }

        public static explicit operator uint(BigDouble value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }

            if ((value.RealValue < uint.MinValue) || (value.RealValue > uint.MaxValue))
            {
                throw new OverflowException();
            }

            return (uint)value.RealValue;
        }

        public static explicit operator ushort(BigDouble value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }

            if ((value.RealValue < ushort.MinValue) || (value.RealValue > ushort.MaxValue))
            {
                throw new OverflowException();
            }

            return (ushort)value.RealValue;
        }

        public static explicit operator ulong(BigDouble value)
        {
            if ((value.RealValue < ulong.MinValue) || (value.RealValue > ulong.MaxValue))
            {
                throw new OverflowException();
            }

            return (ulong)value.RealValue;
        }

        public static bool operator >(BigDouble left, BigDouble right)
        {
            return Compare(left, right) > 0;
        }

        public static bool operator <(BigDouble left, BigDouble right)
        {
            return Compare(left, right) < 0;
        }

        public static bool operator >=(BigDouble left, BigDouble right)
        {
            return Compare(left, right) >= 0;
        }

        public static bool operator <=(BigDouble left, BigDouble right)
        {
            return Compare(left, right) <= 0;
        }

        public static bool operator !=(BigDouble left, BigDouble right)
        {
            return Compare(left, right) != 0;
        }

        public static bool operator ==(BigDouble left, BigDouble right)
        {
            return Compare(left, right) == 0;
        }

        public static BigDouble operator +(BigDouble value)
        {
            return value;
        }

        public static BigDouble operator -(BigDouble value)
        {
            value.Negate();
            return value;
        }

        public static BigDouble operator -(BigDouble left, BigDouble right)
        {
            return left + -right;
        }

        public static BigDouble operator /(BigDouble dividend, BigDouble divisor)
        {
            return Divide(dividend, divisor);
        }

        public static BigDouble operator *(BigDouble left, BigDouble right)
        {
            return Multiply(left, right);
        }
        
        public static BigDouble operator ++(BigDouble value)
        {
            value.Value++;
            return value;
        }

        public static BigDouble operator --(BigDouble value)
        {
            value.Value--;
            return value;
        }

    }
}
