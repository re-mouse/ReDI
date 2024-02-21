namespace ReDI.Tests
{
    public class ServiceWithInjectMethods : IValidateInjected
    {
        private bool _injected1;
        private bool _injected2;

        [Inject]
        public void Injected1() { _injected1 = true; }

        [Inject]
        public void Injected2() { _injected2 = true; }

        public bool Validate() { return _injected1 && _injected2; }
    }
}