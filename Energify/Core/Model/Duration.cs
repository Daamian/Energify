using System.Reflection;
using System.Text.Json.Serialization;
using Energify.Core.Attributes;
using CustomRange = Energify.Core.Attributes.RangeAttribute;

namespace Energify.Core.Model;

[Unit("h/day", "Hours per day")]
public readonly struct Duration : IEquatable<Duration>
{

    [CustomRange(0, 24)]
    public double HoursPerDay { get; }

    [JsonConstructor]
    public Duration(double hoursPerDay)
    {
        if (hoursPerDay < 0 || hoursPerDay > 24)
            throw new ArgumentOutOfRangeException(nameof(hoursPerDay), "Hours per day must be between 0 and 24.");

        HoursPerDay = hoursPerDay;
    }
    
    public override string ToString() 
    { 
        var attr = GetType().GetCustomAttribute<UnitAttribute>();
        return $"{HoursPerDay} {attr?.Symbol}";
    }

    public static Duration operator +(Duration d1, Duration d2) => new (d1.HoursPerDay + d2.HoursPerDay);
    public static Duration operator -(Duration d1, Duration d2) => new (d1.HoursPerDay - d2.HoursPerDay);
    public static bool operator >(Duration d1, Duration d2) => d1.HoursPerDay > d2.HoursPerDay;
    public static bool operator <(Duration d1, Duration d2) => d1.HoursPerDay < d2.HoursPerDay;
    public static bool operator >=(Duration d1, Duration d2) => !(d1 < d2);
    public static bool operator <=(Duration d1, Duration d2) => !(d1 > d2);
    public static implicit operator Duration(double hoursPerDay) => new(hoursPerDay);
    public static bool operator ==(Duration d1, Duration d2) => d1.Equals(d2);
    public static bool operator !=(Duration d1, Duration d2) => !d1.Equals(d2);

    public bool Equals(Duration other)
    {
        return HoursPerDay == other.HoursPerDay;
    }

    public override bool Equals(object? obj)
    {
        return obj is Duration other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(HoursPerDay);
    }
}