using System.ComponentModel.DataAnnotations;

namespace Energify.Core.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class RangeAttribute(double minValue, double maxValue) : ValidationAttribute
{
    public double MinValue { get; } = minValue;
    public double MaxValue { get; } = maxValue;

    public override bool IsValid(object? value)
    {
        if (value is double doubleValue)
        {
            return doubleValue >= MinValue && doubleValue <= MaxValue;
        }

        return false;
    }
}