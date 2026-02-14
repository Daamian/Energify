namespace Energify.Core.Service;

static class UnitInfoProvider
{
    public static (string Symbol, string FullName) GetUnitInfo<T> () where T : struct
    {
        var type = typeof(T);
        var unitAttribute = (Attributes.UnitAttribute?)Attribute.GetCustomAttribute(type, typeof(Attributes.UnitAttribute));

        if (unitAttribute == null)
            throw new InvalidOperationException($"Type {type.Name} does not have a UnitAttribute.");

        return (unitAttribute.Symbol, unitAttribute.FullName);
    }

    public static IEnumerable<(string TypeName, string Symbol, string FullName)> GetAllUnitInfos()
    {
        var assembly = typeof(UnitInfoProvider).Assembly;
        var unitTypes = assembly.GetTypes()
            .Where(t => t.IsValueType && t.GetCustomAttributes(typeof(Attributes.UnitAttribute), false).Length > 0);

        foreach (var type in unitTypes)
        {
            var unitAttribute = (Attributes.UnitAttribute)Attribute.GetCustomAttribute(type, typeof(Attributes.UnitAttribute))!;
            yield return (type.ToString(), unitAttribute.Symbol, unitAttribute.FullName);
        }
    }
}