using System;

namespace ReDI
{
    public class ModuleInstanceAlreadyRegisteredException : Exception
    {
        private readonly IModule _module;

        public ModuleInstanceAlreadyRegisteredException(IModule module)
        {
            _module = module;
        }

        public override string Message { get => $"Instance of module {_module.GetType()} already registered"; }
    }
}