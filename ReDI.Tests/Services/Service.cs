namespace ReDI.Tests
{
    public class Service : IService, IDisposable
    {
        public bool IsDisposed { get; private set; }
        public void Dispose() => IsDisposed = true;
    }
}