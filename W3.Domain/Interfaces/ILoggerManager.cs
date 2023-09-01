namespace W3.Domain.Interfaces
{
    public interface ILoggerManager
    {
        void LogInfo(string name,string message);
        void LogError(string message);
    }

}
