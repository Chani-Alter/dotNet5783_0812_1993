using System.Collections;
using System.Reflection;
using System.Linq;

namespace DO;

/// <summary>
/// class for exteonsion method
/// </summary>
static class Tools
{
    /// <summary>
    ///A function that prints the field values of an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns>A string describing the entity and consistring of the details of the relevant fields</returns>
    public static string ToStringProperty<T>(this T entity)
    {

        string st = "";
        foreach (var (item, enumerable) in from PropertyInfo item in entity.GetType().GetProperties()
                                           let enumerable = item.GetValue(entity, null)
                                           select (item, enumerable))
        {
            if ((enumerable is IEnumerable) && !(enumerable is string))
            {
                IEnumerable? e = enumerable as IEnumerable;
                foreach (var a in e)
                {
                    st += a.ToStringProperty();

                }
            }
            else
            {
                st += "\n" + item.Name +
           "- " + item.GetValue(entity, null);
            }
        }

        return st;
    }
    public static void ToStringPropertyToIEnumerable(IEnumerable collection, string st)
    {
        foreach (var item in collection)
        {

            st += item;
        }
    }
}

