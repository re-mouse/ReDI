namespace ReDI.Tests
{
    public class ServiceWithInjectConstructor : IValidateInjected
    {
        private bool _injected;

        [Inject]
        public ServiceWithInjectConstructor() { _injected = true; }

        public ServiceWithInjectConstructor(int a) { }

        public bool Validate() { return _injected; }
    }
}