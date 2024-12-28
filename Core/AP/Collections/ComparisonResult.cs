namespace AP.Collections;

/// <summary>
/// A comparison ResultSet
/// </summary>
public enum ComparisonResult : int
{
    Invalid = 1, //byte.MaxValue,
    Equal = 0,
    Less = -1,
    Greater = 1
}
