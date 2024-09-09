using System;

namespace ReDI
{
    public class NotFoundInjectingConstructorException : Exception
    {
        private readonly Type _type;

        public NotFoundInjectingConstructorException(Type type) { _type = type; }
        
        public override string Message { get => $"No constructor with [Inject] attribute, it required if multiple constructor's defined for type {_type}"; }
    }
}