namespace ReDI
{
    public interface IModule
    {
        public void BindDependencies(TypeManager typeBinder);

        public void BindModuleDependencies(ModuleManager moduleBinder);
    }
}