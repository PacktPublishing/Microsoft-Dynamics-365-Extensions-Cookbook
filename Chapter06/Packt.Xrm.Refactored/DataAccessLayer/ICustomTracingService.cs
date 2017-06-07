namespace Packt.Xrm.Refactored.DataAccessLayer
{
    public interface ICustomTracingService
    {
        void Trace(string message, params object[] args);
    }
}
