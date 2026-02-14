using System.Text.Json.Serialization;
using Energify.Core.Attributes;

namespace Energify.Core.Model;

[Unit("W", "Watt")]
public readonly struct Power : IEquatable<Power>
{
    public double Watts { get; }

    public double KiloWatts => Watts / 1000;

    [JsonConstructor]
    public Power(double watts)
    {
        if (watts < 0)
            throw new ArgumentOutOfRangeException(nameof(watts), "Power in watts cannot be negative.");

        Watts = watts;
    }

    public override string ToString() => 
        Watts >= 1000 ? $"{KiloWatts:F2} kW" : $"{Watts} W";

    private const double Epsilon = 1e-9;

    public bool Equals(Power other)
    {
        return Math.Abs(Watts - other.Watts) < Epsilon;
    }

    public override bool Equals(object? obj)
    {
        return obj is Power other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Math.Round(Watts, 9));
    }

    public static Energy operator *(Power power, Duration duration) => new(power.KiloWatts * duration.HoursPerDay);
    public static Power operator +(Power p1, Power p2) => new(p1.Watts + p2.Watts);
    public static Power operator -(Power p1, Power p2) => new(p1.Watts - p2.Watts);
    public static bool operator >(Power p1, Power p2) => p1.Watts > p2.Watts;
    public static bool operator <(Power p1, Power p2) => p1.Watts < p2.Watts;
    public static bool operator >=(Power p1, Power p2) => !(p1 < p2);
    public static bool operator <=(Power p1, Power p2) => !(p1 > p2);
    public static implicit operator Power(double watts) => new(watts);
    public static bool operator ==(Power p1, Power p2) => p1.Equals(p2);
    public static bool operator !=(Power p1, Power p2) => !p1.Equals(p2);
}