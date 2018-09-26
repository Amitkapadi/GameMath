# GameMath
Some C# classes for big numbers as seen in popular idle games

The GameMathLib exposes these:

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
    public struct BigDouble
    
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
       
    /// <summary>
    /// Represents a 256-bit signed integer.
    /// </summary>
    public struct Int256 : IComparable<Int256>, IComparable, IEquatable<Int256>, IFormattable
