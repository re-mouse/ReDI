using System;

namespace ReDI
{
    public class ModuleNotAssignedException : Exception
    {
        private readonly Type _moduleType;

        public ModuleNotAssignedException(Type moduleType)
        {
            _moduleType = moduleType;
        }

        public override string Message { get => $"Not find module instance of {_moduleType}, probably null registration"; }
    }
}