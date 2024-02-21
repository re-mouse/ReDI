namespace ReDI.Tests
{
    public class ServiceABC : IValidateInjected
    {
        [Inject] public ServiceA ServiceA { get; private set; }
        public ServiceB ServiceB { get; }
        public ServiceC ServiceC { get; private set; }

        [Inject]
        public ServiceABC(ServiceB serviceB) { ServiceB = serviceB; }

        public ServiceABC()
        {
            //Wrong constructor
        }

        [Inject]
        public void TestInject(ServiceC serviceC) { ServiceC = serviceC; }

        public bool Validate() { return ServiceA != null && ServiceB != null && ServiceC != null; }
    }
}