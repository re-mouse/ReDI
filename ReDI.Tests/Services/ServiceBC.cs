namespace ReDI.Tests
{
    public class ServiceBC : IValidateInjected
    {
        public ServiceB ServiceB { get; }
        public ServiceC ServiceC { get; }

        public ServiceBC(ServiceB serviceB, ServiceC serviceC)
        {
            ServiceB = serviceB;
            ServiceC = serviceC;
        }

        public bool Validate() { return ServiceB != null && ServiceC != null; }
    }
}