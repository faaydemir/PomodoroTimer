namespace XamarinHelperLib.Utils
{
    public interface IPersistanceService
    {
        bool Set<T>(string id, T value);
        T Get<T>(string id);
    }
}