using System;

namespace ReDI
{
    public class ModuleInstanceAlreadyRegisteredException : Exception
    {
        private readonly Module _module;

        public ModuleInstanceAlreadyRegisteredException(Module module)
        {
            _module = module;
        }

        public override string Message { get => $"Instance of module {_module.GetType()} already registered"; }
    }
}