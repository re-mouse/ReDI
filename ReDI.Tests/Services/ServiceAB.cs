namespace ReDI.Tests
{
    public class ServiceAB
    {
        public ServiceA ServiceA { get; }
        public ServiceB ServiceB { get; }

        public ServiceAB(ServiceA serviceA, ServiceB serviceB)
        {
            ServiceA = serviceA;
            ServiceB = serviceB;
        }
    }
}