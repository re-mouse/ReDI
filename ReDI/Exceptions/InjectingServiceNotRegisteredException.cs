using System;

namespace ReDI
{
    public class InjectingServiceNotRegisteredException : Exception
    {
        private readonly Type _serviceType;
        private readonly Type _injectingType;

        public InjectingServiceNotRegisteredException(Type serviceType, Type injectingType)
        {
            _serviceType = serviceType;
            _injectingType = injectingType;
        }

        public override string Message { get => $"Not found service of type {_injectingType}, required for service {_serviceType}"; }
    }
}