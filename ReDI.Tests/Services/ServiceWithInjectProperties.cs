namespace ReDI.Tests
{
    public class ServiceWithInjectProperties : IValidateInjected
    {
        [Inject] private ServiceWithInjectFields injectFields { get; set; }

        public bool Validate()
        {
            return injectFields != null;
        }
    }
}