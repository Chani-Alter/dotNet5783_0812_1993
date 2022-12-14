using System.Collections;
using System.Reflection;

namespace BO
{
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
        public static string ToStringProperty<T>(this T t)
        {

            string str = "";
            foreach (PropertyInfo item in t.GetType().GetProperties())
            {
                var enumerable = item.GetValue(t, null);

                if ((enumerable is IEnumerable) && !(enumerable is string))
                {
                    IEnumerable e = enumerable as IEnumerable;
                    foreach (var a in e)
                    {
                        str += a.ToStringProperty();

                    }
                }
                else
                {
                    str += "\n" + item.Name +
               ": " + item.GetValue(t, null);
                }
            }
            return str;
        }
    }

}
