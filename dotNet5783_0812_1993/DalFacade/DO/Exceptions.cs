
namespace DO;

/// <summary>
///A class for errors thrown when non-existent items are requested
/// </summary>

[Serializable]
public class DoesNotExistedDalException : Exception
{
    public int EntityId;
    public string EntityName;
    public DoesNotExistedDalException(int _id, string _name) : base()
    { 
        EntityId = _id; EntityName = _name; 
    }

    public DoesNotExistedDalException(int _id, string _name, string _message) : base(_message)
    { 
        EntityId = _id; EntityName = _name; 
    }

    public DoesNotExistedDalException(int _id, string _name, string _message, Exception innerException) : base(_message, innerException)
    {
        EntityId = _id; EntityName = _name;
    }
    public DoesNotExistedDalException(string? _message) : base(_message)
    { }
    public override string ToString()
    {
        if (EntityId == -1)
        {
            return $" {EntityName} are not exist.";
        }
        return $"id:{EntityId} of type {EntityName} is not exist.";
    }
}
/// <summary>
/// A class for errors thrown when items  exist in duplicates
/// </summary>

[Serializable]
public class DuplicateDalException : Exception
{
    public int EntityId;
    public string EntityName;

    public DuplicateDalException(int _id, string _name) : base()
    {
        EntityId = _id; EntityName = _name;
    }

    public DuplicateDalException(int _id, string _name, string _message) : base(_message)
    {
        EntityId = _id; EntityName = _name;
    }

    public DuplicateDalException(int _id, string _name, string _message, Exception innerException) : base(_message, innerException)
    { 
        EntityId = _id; EntityName = _name; 
    }
    public DuplicateDalException(string? _message) : base(_message)
    { }

    public override string ToString() => $"id:{EntityId} of type {EntityName} is already exist.";
}
