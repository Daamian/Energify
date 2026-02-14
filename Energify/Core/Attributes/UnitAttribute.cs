namespace Energify.Core.Attributes;

[AttributeUsage(AttributeTargets.Struct)]
public class UnitAttribute(string symbol, string fullName) : Attribute
{
    public string Symbol { get; } = symbol;
    public string FullName { get; } = fullName;
}