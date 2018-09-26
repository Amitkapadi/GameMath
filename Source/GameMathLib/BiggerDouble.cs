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
    /// BiggerDouble is float point number, but can handle a 256 bit number of 0s
    /// this means 1.15e+77 is max # of 0s.
    /// To be precise this is 115,792,089,237,316,195,423,570,985,008,687,907,853,269,984,665,640,564,039,457,584,007,913,129,639,936 
    /// 
    /// BigDouble is defined as [Value * 10 ^ Exp]
    /// where Value is a 64-bit double which has 53 bits of fraction 
    /// for ≈15.95 decimal digits of precision
    /// while Exp is a 256bit Int256 which stores the # of 0s
    /// </summary>
    public struct BiggerDouble
    {
        public double Value;
        public Int256 Exp; 

        public BiggerDouble(double d)
        {
            Value = 0;
            Exp = 0;
            ConvertFromDouble(d);
        }

        public BiggerDouble(byte value) : this((double)value)
        {
        }

        public BiggerDouble(bool value) 
        {
            this.Exp = 0;
            this.Value = value ? 1 : 0;
        }

        public BiggerDouble(char value) : this((double)value)
        {
        }

        public BiggerDouble(decimal value) : this((double)value)
        {
        }

        public BiggerDouble(float value) : this((double)value)
        {
        }

        public BiggerDouble(short value) : this((double)value)
        {
        }

        public BiggerDouble(int value) : this((double)value)
        {
        }

        public BiggerDouble(long value) : this((double)value)
        {
        }

        public BiggerDouble(sbyte value) : this((double)value)
        {
        }

        public BiggerDouble(ushort value) : this((double)value)
        {
        }

        public BiggerDouble(uint value) : this((double)value)
        {
        }

        public BiggerDouble(ulong value) : this((double)value)
        {
        }

        public BiggerDouble(double value, Int256 exp)
        {
            Value = value;
            Exp = exp;
        }

        public double RealValue
        {
            get
            {
                if(this.Exp < 308)
                {
                    double expandedPower = Math.Pow(10.0, (ulong)this.Exp);
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
            Int256 newExp = 0;
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
                Int256 newExp = Exp;
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
                Int256 newExp = Exp;
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

        public static BiggerDouble operator+(BiggerDouble a, BiggerDouble b)
        {
            if (a.Exp == b.Exp)
            {
                BiggerDouble m = new BiggerDouble();
                m.Exp = a.Exp;
                m.Value = a.Value + b.Value;
                m.Simplify();
                return m;
            }
            else if ( a.Exp > b.Exp)
            {
                // a is bigger
                Int256 deltaExp = a.Exp - b.Exp;
                if(deltaExp <= 16)
                {
                    double bX = b.Value / Math.Pow(10, (double)deltaExp);
                    BiggerDouble m = new BiggerDouble();
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
                Int256 deltaExp = b.Exp - a.Exp;
                if (deltaExp <= 16)
                {
                    double aX = a.Value / Math.Pow(10, (double)deltaExp);
                    BiggerDouble m = new BiggerDouble();
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
        
        public static int Compare(BiggerDouble left, object right)
        {
            if (right is BiggerDouble)
            {
                return Compare(left, (BiggerDouble)right);
            }

            if (right is bool)
            {
                return Compare(left, new BiggerDouble((bool)right));
            }

            if (right is byte)
            {
                return Compare(left, new BiggerDouble((byte)right));
            }

            if (right is char)
            {
                return Compare(left, new BiggerDouble((char)right));
            }

            if (right is decimal)
            {
                return Compare(left, new BiggerDouble((decimal)right));
            }

            if (right is double)
            {
                return Compare(left, new BiggerDouble((double)right));
            }

            if (right is short)
            {
                return Compare(left, new BiggerDouble((short)right));
            }

            if (right is int)
            {
                return Compare(left, new BiggerDouble((int)right));
            }

            if (right is long)
            {
                return Compare(left, new BiggerDouble((long)right));
            }

            if (right is sbyte)
            {
                return Compare(left, new BiggerDouble((sbyte)right));
            }

            if (right is float)
            {
                return Compare(left, new BiggerDouble((float)right));
            }

            if (right is ushort)
            {
                return Compare(left, new BiggerDouble((ushort)right));
            }

            if (right is uint)
            {
                return Compare(left, new BiggerDouble((uint)right));
            }

            if (right is ulong)
            {
                return Compare(left, new BiggerDouble((ulong)right));
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

        public static int Compare(BiggerDouble left, BiggerDouble right)
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

        public int CompareTo(BiggerDouble value)
        {
            return Compare(this, value);
        }

        public BiggerDouble ToAbs()
        {
            return Abs(this);
        }

        public static BiggerDouble Abs(BiggerDouble value)
        {
            if (value.Sign < 0)
            {
                return -value;
            }

            return value;
        }

        public static BiggerDouble Add(BiggerDouble left, BiggerDouble right)
        {
            return left + right;
        }

        public static BiggerDouble Subtract(BiggerDouble left, BiggerDouble right)
        {
            return left - right;
        }

        public static BiggerDouble Divide(BiggerDouble dividend, BiggerDouble divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }

            BiggerDouble bd = new BiggerDouble(
                dividend.Value / divisor.Value, 
                dividend.Exp - divisor.Exp);
            bd.Simplify();
            return bd;
        }

        public static BiggerDouble Multiply(BiggerDouble left, BiggerDouble right)
        {
            BiggerDouble bd = new BiggerDouble(
                left.Value * right.Value, 
                left.Exp + right.Exp);
            bd.Simplify();
            return bd;
        }

        public static implicit operator BiggerDouble(bool value)
        {
            return new BiggerDouble(value);
        }

        public static implicit operator BiggerDouble(byte value)
        {
            return new BiggerDouble(value);
        }

        public static implicit operator BiggerDouble(char value)
        {
            return new BiggerDouble(value);
        }

        public static explicit operator BiggerDouble(decimal value)
        {
            return new BiggerDouble(value);
        }

        public static explicit operator BiggerDouble(double value)
        {
            return new BiggerDouble(value);
        }

        public static implicit operator BiggerDouble(short value)
        {
            return new BiggerDouble(value);
        }

        public static implicit operator BiggerDouble(int value)
        {
            return new BiggerDouble(value);
        }

        public static implicit operator BiggerDouble(long value)
        {
            return new BiggerDouble(value);
        }

        public static implicit operator BiggerDouble(sbyte value)
        {
            return new BiggerDouble(value);
        }

        public static explicit operator BiggerDouble(float value)
        {
            return new BiggerDouble(value);
        }

        public static implicit operator BiggerDouble(ushort value)
        {
            return new BiggerDouble(value);
        }

        public static implicit operator BiggerDouble(uint value)
        {
            return new BiggerDouble(value);
        }

        public static implicit operator BiggerDouble(ulong value)
        {
            return new BiggerDouble(value);
        }

        public static explicit operator bool(BiggerDouble value)
        {
            return value.Sign != 0;
        }

        public static explicit operator byte(BiggerDouble value)
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

        public static explicit operator char(BiggerDouble value)
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

        public static explicit operator double(BiggerDouble value)
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

        public static explicit operator float(BiggerDouble value)
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

        public static explicit operator short(BiggerDouble value)
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

        public static explicit operator int(BiggerDouble value)
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

        public static explicit operator long(BiggerDouble value)
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

        public static explicit operator uint(BiggerDouble value)
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

        public static explicit operator ushort(BiggerDouble value)
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

        public static explicit operator ulong(BiggerDouble value)
        {
            if ((value.RealValue < ulong.MinValue) || (value.RealValue > ulong.MaxValue))
            {
                throw new OverflowException();
            }

            return (ulong)value.RealValue;
        }

        public static bool operator >(BiggerDouble left, BiggerDouble right)
        {
            return Compare(left, right) > 0;
        }

        public static bool operator <(BiggerDouble left, BiggerDouble right)
        {
            return Compare(left, right) < 0;
        }

        public static bool operator >=(BiggerDouble left, BiggerDouble right)
        {
            return Compare(left, right) >= 0;
        }

        public static bool operator <=(BiggerDouble left, BiggerDouble right)
        {
            return Compare(left, right) <= 0;
        }

        public static bool operator !=(BiggerDouble left, BiggerDouble right)
        {
            return Compare(left, right) != 0;
        }

        public static bool operator ==(BiggerDouble left, BiggerDouble right)
        {
            return Compare(left, right) == 0;
        }

        public static BiggerDouble operator +(BiggerDouble value)
        {
            return value;
        }

        public static BiggerDouble operator -(BiggerDouble value)
        {
            value.Negate();
            return value;
        }

        public static BiggerDouble operator -(BiggerDouble left, BiggerDouble right)
        {
            return left + -right;
        }

        public static BiggerDouble operator /(BiggerDouble dividend, BiggerDouble divisor)
        {
            return Divide(dividend, divisor);
        }

        public static BiggerDouble operator *(BiggerDouble left, BiggerDouble right)
        {
            return Multiply(left, right);
        }
        
        public static BiggerDouble operator ++(BiggerDouble value)
        {
            value.Value++;
            return value;
        }

        public static BiggerDouble operator --(BiggerDouble value)
        {
            value.Value--;
            return value;
        }

    }
}
