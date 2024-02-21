using System;

namespace ReDI
{
    public class CircularDependencyFoundException : Exception
    {
        private readonly Type _type;

        public CircularDependencyFoundException(Type type) { _type = type; }

        public override string Message { get => $"{_type} refers to circular dependency"; }
    }
}