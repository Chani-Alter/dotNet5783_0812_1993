using DO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;

namespace Dal;

public static class XmlTools
{
    const string s_dir = @"..\xml\";

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
            throw new Exception($"fail to create xml file:{filePath}", ex);
        }
    }

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
            // DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            throw new Exception($"fail to load xml file: {filePath}", ex);
        }
    }
    public static void SaveListForXmlElement(XElement rootElem, string entity)
    {
        string filePath = $"{s_dir + entity}.xml";

        try
        {
            rootElem.Save(filePath);
        }
        catch (Exception ex)
        {
            throw new Exception($"fail to create xml file:{filePath}", ex);
        }
    }

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
            throw new Exception($"fail to load xml file: {filePath}", ex);
        }
    }
    public static XElement convertToXelement<T>(T entity ,string entityName) where T : struct
    {
        XElement xmlEntity = new XElement(entityName);
        entity.GetType().GetProperties().ToList().ForEach(p => xmlEntity.Add(new XElement(p.Name.ToString(), p.GetValue(entity, null))));
        return xmlEntity;
    }

    public static List<T?> createList<T>(XElement root , string entityName) where T : struct
    {
        IEnumerable<XElement>? rootXelement = root?.Elements(entityName) ?? throw new Exception("  ");
        object entityObj = new T();
        List<T?> list = new();

        foreach (XElement xmlElement in rootXelement)
        {
            xmlElement.Elements().ToList().ForEach(element => initializeXelement(entityObj, element , entityName));
            list.Add((T)entityObj);
        }

        return list;
    }
    public static void initializeXelement(object entityObj, XElement xmlElement, string entityName)
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

    public static T ToEnum<T>(this XElement element, string name) where T : struct, Enum
    {
        Enum.TryParse<T>((string?)element.Element(name), out var result);
        return result;
    }


    public static int NewID(string entity)
    {
        string filePath = $"{s_dir}config.xml";
        try
        {
            if (!File.Exists(filePath)) throw new Exception(" ");
            XElement rootConfig = XElement.Load(filePath);
            XElement? IdString = rootConfig.Element(entity);
            int id = Convert.ToInt32(IdString.Value) + 1;
            IdString.SetValue(id.ToString());
            rootConfig.Save(filePath);
            return id;
        }
        catch (Exception ex)
        {
            throw new Exception($"fail to load config file: {filePath}", ex);
        }
    }
}