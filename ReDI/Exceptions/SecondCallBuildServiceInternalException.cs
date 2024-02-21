using System;

namespace ReDI
{
    public class SecondCallBuildServiceInternalException : Exception
    {
        private readonly Type _type;

        public SecondCallBuildServiceInternalException(Type type) { _type = type; }

        public override string Message { get => $"Build of serivce {_type} was called second time"; }
    }
}