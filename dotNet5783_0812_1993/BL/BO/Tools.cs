using System.Collections;
using System.Reflection;

namespace BO;

/// <summary>
/// class for exteonsion method
/// </summary>
static class Tools
{
    /// <summary>
    /// A method that prints attributes of entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns>string</returns>
    public static string ToStringProperty<T>(this T entity)
    {

        string st = "";
        foreach (PropertyInfo item in entity.GetType().GetProperties())
        {
            var enumerable = item.GetValue(entity, null);

            if ((enumerable is IEnumerable) && !(enumerable is string))
            {
                IEnumerable? en = enumerable as IEnumerable;
                foreach (var _item in en)
                {
                    st += _item.ToStringProperty();

                }
            }
            else
            {
                st += "\n" + item.Name +
           ": " + item.GetValue(entity, null);
            }
        }
        return st;
    }
}

