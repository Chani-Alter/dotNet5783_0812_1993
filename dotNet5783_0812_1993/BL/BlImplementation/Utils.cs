using BO;
using System.Reflection;

namespace BlImplementation;

internal class Utils
{
    internal static S cast<S, T>(T t) where S : new()
    {
        object s = new S();
        foreach (PropertyInfo prop in t?.GetType().GetProperties() ?? throw new NoBlPropertiesInObject())
        {
            PropertyInfo? type = s?.GetType().GetProperty(prop.Name);
            if (type == null || type.Name == "Category")
                continue;
            var value = t?.GetType()?.GetProperty(prop.Name)?.GetValue(t, null);
            type.SetValue(s, value);
        }
        return (S)s;
    }
}
