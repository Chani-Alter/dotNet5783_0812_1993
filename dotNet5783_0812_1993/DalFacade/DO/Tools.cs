using System.Collections;
using System.Reflection;

namespace DO
{
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
        public static string TostringProperty<T>(this T entity)
        {

            string st = "";
            foreach (PropertyInfo item in entity.GetType().GetProperties())
            {
                var enumerable = item.GetValue(entity, null);

                if ((enumerable is IEnumerable) && !(enumerable is string))
                {
                    IEnumerable e = enumerable as IEnumerable;
                    foreach (var a in e)
                    {
                        st += a.TostringProperty();

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
        public static void TostringPropertyToIEnumerable(IEnumerable collection, string st)
        {
            foreach (var item in collection)
            {

                st += item;
            }
        }
    }

}
