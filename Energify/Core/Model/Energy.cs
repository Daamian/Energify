using ElectricityCost.Attributes;

namespace ElectricityCost.Model;

[Unit("kWh", "Kilowatt-hour")]
public readonly struct Energy : IEquatable<Energy>
{
    public double KiloWattHours { get; }

    public Energy(double kiloWattHours)
    {
        if (kiloWattHours < 0)
            throw new ArgumentOutOfRangeException(nameof(kiloWattHours), "Energy in kWh cannot be negative.");

        KiloWattHours = kiloWattHours;
    }

    public override string ToString() => $"{KiloWattHours} kWh";
    public static  Money operator *(Energy energy, Money costPerKWh) => new ((decimal) energy.KiloWattHours * costPerKWh.Amount, costPerKWh.Currency);
    public static  Energy operator +(Energy e1, Energy e2) => new (e1.KiloWattHours + e2.KiloWattHours);
    public static  Energy operator -(Energy e1, Energy e2) => new (e1.KiloWattHours - e2.KiloWattHours);
    public static bool operator >(Energy e1, Energy e2) => e1.KiloWattHours > e2.KiloWattHours;
    public static bool operator <(Energy e1, Energy e2) => e1.KiloWattHours < e2.KiloWattHours;
    public static bool operator >=(Energy e1, Energy e2) => !(e1 < e2);
    public static bool operator <=(Energy e1, Energy e2) => !(e1 > e2);
    public static bool operator ==(Energy e1, Energy e2) => e1.Equals(e2);
    public static bool operator !=(Energy e1, Energy e2) => !e1.Equals(e2);
    public static Energy operator *(Energy energy, double factor) => new (energy.KiloWattHours * factor);

    public bool Equals(Energy other)
    {
        return KiloWattHours == other.KiloWattHours;
    }

    public override bool Equals(object? obj)
    {
        return obj is Energy other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(KiloWattHours);
    }
}