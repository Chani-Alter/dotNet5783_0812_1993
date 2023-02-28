using BO;
using System.Reflection;

namespace BlImplementation;

/// <summary>
/// Cast object by reflection : Bonus!!!!
/// </summary>
static class Utils
{
    internal static S Cast<S, T>(this T t) where S : new()
    {
        object s = new S();
        foreach (PropertyInfo prop in t?.GetType().GetProperties() ?? throw new NoBlPropertiesInObject())
        {
            PropertyInfo? type = s?.GetType().GetProperty(prop.Name);
            if (type == null || type.Name == "Category"||type.Name=="InStock")
                continue;
            var value = t?.GetType()?.GetProperty(prop.Name)?.GetValue(t, null);
            type.SetValue(s, value);
        }
        return (S)s;
    }
}
