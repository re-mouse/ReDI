namespace ReDI
{
    public abstract class Module
    {
        public abstract void BindDependencies(TypeManager typeBinder);

        public abstract void BindModuleDependencies(ModuleManager moduleBinder);
    }
}