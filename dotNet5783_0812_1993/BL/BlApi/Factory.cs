namespace BlApi;

public class Factory
{
    static public IBl Get() {
        return new BlImplementation.Bl();
    }
}
