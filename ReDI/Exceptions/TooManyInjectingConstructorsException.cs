using System;

namespace ReDI
{
    public class TooManyInjectingConstructorsException: Exception
    {
        private readonly Type _type;

        public TooManyInjectingConstructorsException(Type type) { _type = type; }

        public override string Message { get => $"{_type} contain more than 1 constructor with injecting attributes"; }
    }
}