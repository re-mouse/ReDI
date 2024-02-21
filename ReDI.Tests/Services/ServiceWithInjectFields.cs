namespace ReDI.Tests
{
    public class ServiceWithInjectFields : IValidateInjected
    {
        [Inject] private ServiceWithInjectConstructor _inject1;
        [Inject] public ServiceWithInjectProperties _inject2;

        public bool Validate() { return _inject1 != null && _inject2 != null; }
    }
}