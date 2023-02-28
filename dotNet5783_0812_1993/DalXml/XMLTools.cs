using DO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;

namespace Dal;

/// <summary>
/// a class of reflaction function and help function to the dalXml classes
/// </summary>
public static class XmlTools
{
    #region XML SERIALIZER

    /// <summary>
    /// Save list of T in the file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">the list</param>
    /// <param name="entity">the file name</param>
    /// <exception cref="XMLFileNullExeption"></exception>
    public static void SaveListForXmlSerializer<T>(List<T?> list, string entity) where T : struct
    {
        string filePath = $"{s_dir + entity}.xml";

        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

            XmlSerializer serializer = new XmlSerializer(typeof(List<T?>));

            serializer.Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new XMLFileNullExeption($"fail to create xml file:{filePath}", ex);
        }
    }

    /// <summary>
    /// load a list of T from the XML file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="XMLFileNullExeption"></exception>
      public static List<T?> LoadListFromXmlSerializer<T>(string entity) where T : struct
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            if (!File.Exists(filePath)) return new();
            using FileStream file = new(filePath, FileMode.Open);
            XmlSerializer x = new(typeof(List<T?>));
            return x.Deserialize(file) as List<T?> ?? new();
        }
        catch (Exception ex)
        {
            throw new XMLFileNullExeption($"fail to load xml file: {filePath}", ex);
        }
    }

    #endregion

    #region XML XELEMENT

    /// <summary>
    /// save a XElement in the file
    /// </summary>
    /// <param name="rootElem"></param>
    /// <param name="entity"></param>
    /// <exception cref="XMLFileNullExeption"></exception>
    public static void SaveListForXmlElement(XElement rootElem, string entity)
    {
        string filePath = $"{s_dir + entity}.xml";

        try
        {
            rootElem.Save(filePath);
        }
        catch (Exception ex)
        {
            throw new XMLFileNullExeption($"fail to create xml file:{filePath}", ex);
        }
    }

    /// <summary>
    /// load an xElement from the xml file
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="XMLFileNullExeption"></exception>
    public static XElement LoadListFromXmlElement(string entity)
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            if (File.Exists(filePath))
                return XElement.Load(filePath);
            XElement rootElem = new(entity);
            rootElem.Save(filePath);
            return rootElem;
        }
        catch (Exception ex)
        {
            throw new XMLFileNullExeption($"fail to load xml file: {filePath}", ex);
        }
    }

    /// <summary>
    /// convert an object to xml element
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <param name="entityName"></param>
    /// <returns></returns>
    public static XElement ConvertToXelement<T>(T entity ,string entityName) where T : struct
    {
        XElement xmlEntity = new XElement(entityName);
        entity.GetType().GetProperties().ToList().ForEach(p => xmlEntity.Add(new XElement(p.Name.ToString(), p.GetValue(entity, null))));
        return xmlEntity;
    }

    /// <summary>
    /// create a list from XElement
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="root"></param>
    /// <param name="entityName"></param>
    /// <returns></returns>
    /// <exception cref="XMLFileNullExeption"></exception>
    public static List<T?> CreateList<T>(XElement root , string entityName) where T : struct
    {
        IEnumerable<XElement>? rootXelement = root?.Elements(entityName) ?? throw new XMLFileNullExeption("The Xml filr is in corect");
        object entityObj = new T();
        List<T?> list = new();

        foreach (XElement xmlElement in rootXelement)
        {
            xmlElement.Elements().ToList().ForEach(element => InitializeXelement(entityObj, element , entityName));
            list.Add((T)entityObj);
        }

        return list;
    }

    /// <summary>
    /// convert from an xElement to object 
    /// </summary>
    /// <param name="entityObj"></param>
    /// <param name="xmlElement"></param>
    /// <param name="entityName"></param>
    public static void InitializeXelement(object entityObj, XElement xmlElement, string entityName)
    {
        PropertyInfo? property = entityObj?.GetType()?.GetProperty(xmlElement.Name.ToString());
        if (entityName == "order")
        {
            if (xmlElement.Name.ToString() != "ID" && !xmlElement.Name.ToString().EndsWith("Date"))
                property?.SetValue(entityObj, xmlElement.Value);
            else if (xmlElement.Name.ToString() == "ID")
                property?.SetValue(entityObj, int.Parse(xmlElement.Value));
            else if (xmlElement.Value != "")
                property?.SetValue(entityObj, DateTime.Parse(xmlElement.Value));
            else
                property?.SetValue(entityObj, null);
        }
        else if (entityName == "orderItem")
            if (xmlElement.Name.ToString() == "Price")
                property?.SetValue(entityObj, double.Parse(xmlElement.Value));
            else
                property?.SetValue(entityObj, int.Parse(xmlElement.Value));
        else
            if (xmlElement.Name.ToString() == "ID" || xmlElement.Name.ToString() == "InStock")
            property?.SetValue(entityObj, int.Parse(xmlElement.Value));
        else if (xmlElement.Name.ToString() == "Price")
            property?.SetValue(entityObj, double.Parse(xmlElement.Value));
        else if (xmlElement.Name.ToString() == "Category")
        {
            Category category;
            Enum.TryParse<Category>(xmlElement.Value, true,out category);
            property?.SetValue(entityObj,category);
        }
        else if (xmlElement.Value != "")
            property?.SetValue(entityObj, xmlElement.Value);
        else
            property?.SetValue(entityObj, null);

    }

    /// <summary>
    /// return the next id for the next object and save the next one in the config file.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="XMLFileNullExeption"></exception>
    public static int NewID(string entity)
    {
        string filePath = $"{s_dir}config.xml";
        try
        {
            if (!File.Exists(filePath)) throw new XMLFileNullExeption("Xml file doesnt exist");
            XElement rootConfig = XElement.Load(filePath);
            XElement? IdString = rootConfig.Element(entity);
            int id = Convert.ToInt32(IdString.Value) + 1;
            IdString.SetValue(id.ToString());
            rootConfig.Save(filePath);
            return id;
        }
        catch (Exception ex)
        {
            throw new XMLFileNullExeption($"fail to load config file: {filePath}", ex);
        }
    }
    #endregion

    const string s_dir = @"..\xml\";
}
