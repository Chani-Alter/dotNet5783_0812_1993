namespace BO;

/// <summary>
///A class for errors thrown when non-existent items are requested
/// </summary>

[Serializable]

public class DoesNotExistedBlException : Exception
{
    public DoesNotExistedBlException(string _message, Exception innerException) : base(_message, innerException)
    { }

    public override string ToString() => base.ToString() + $"Entity is not exist";
}

/// <summary>
/// Throws class for items that exist in duplicates
/// </summary>
[Serializable]
public class BLAlreadyExistException : Exception
{
    public BLAlreadyExistException(string message, Exception innerException) : base(message, innerException)
    { }
    public override string ToString() => base.ToString() + $"Entity is already exist";
}


/// <summary>
///A class for errors thrown when an update error occurs
/// </summary>

public class UpdateErrorBlException : Exception
{
    public UpdateErrorBlException(string? _message) : base(_message) { }
}

/// <summary>
/// A class for errors thrown for a invalid input
/// </summary>

public class InvalidInputBlException : Exception
{
    public InvalidInputBlException(string? _message) : base(_message) { }
}


/// <summary>
/// A class for errors thrown impossible action
/// </summary>
public class ImpossibleActionBlException : Exception
{
    public ImpossibleActionBlException(string? _message) : base(_message) { }
}