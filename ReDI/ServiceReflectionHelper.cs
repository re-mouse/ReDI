using System;
using System.Linq;
using System.Reflection;

namespace ReDI
{
    internal delegate void InjectDelegate(object instance, Container container);
    internal delegate object ConstructorDelegate(Container container);
    internal static class ServiceReflectionHelper
    {
        
        public static ConstructorDelegate GetConstructorDelegate(Type serviceType)
        {
            var constructor = FindConstructor(serviceType);
            var requiredTypes = constructor.GetParameters().Select(p => p.ParameterType).ToArray();
            var args = new object[requiredTypes.Length];
            
            return container =>
            {
                for (int i = 0; i < requiredTypes.Length; i++)
                {
                    Type? type = requiredTypes[i];
#if DEBUG
                        args[i] = container.Resolve(type) ?? throw new InjectingServiceNotRegisteredException(serviceType, type);
#else
                        args[i] = container.Resolve(type);
#endif
                }

                return Activator.CreateInstance(serviceType, args);
            };
        }

        private static ConstructorInfo FindConstructor(Type serviceType)
        {
            var constructors = serviceType.GetConstructors();

            if (constructors.Length == 1)
            {
                return constructors.First();
            }
            
            var injectingConstructor = constructors
                    .Where(c => c.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0).ToArray();

#if DEBUG
            if (injectingConstructor.Length == 0)
            {
                throw new NotFoundInjectingConstructorException(serviceType);
            }

            if (injectingConstructor.Length > 1)
            {
                throw new TooManyInjectingConstructorsException(serviceType);
            }
#endif

            return injectingConstructor.First();
        }

        public static InjectDelegate GetInjectingFieldsAndPropertiesDelegate(Type serviceType)
        {
            InjectDelegate multiDelegate = (o, c) => { };
            
            var injectingFields = serviceType
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.SetField | BindingFlags.Instance)
                .Where(f => f.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0).ToArray();
            foreach (var field in injectingFields)
            {
                multiDelegate += (obj, container) =>
                {
                    var injectingValue = container.Resolve(field.FieldType);
#if !DEBUG
                    if (property.CanWrite)
#endif
                    field.SetValue(obj, injectingValue);
                };
            }
            
            var injectingProperties = serviceType
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0).ToArray();
            foreach (var property in injectingProperties)
            {
                multiDelegate += (obj, container) =>
                {
                    var injectingValue = container.Resolve(property.PropertyType);
#if !DEBUG
                    if (property.CanWrite)
#endif
                    property.SetValue(obj, injectingValue);
                };
            }

            return multiDelegate;
        }
        
        public static InjectDelegate GetInjectingMethodsDelegate(Type type)
        {
            var injectingMethods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(f => f.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0).ToArray();
            
            InjectDelegate multiDelegate = (o, c) => { };
            foreach (var method in injectingMethods)
            {
                var requiredTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var args = new object[requiredTypes.Length];
                
                multiDelegate += (obj, container) =>
                {
                    for (int i = 0; i < requiredTypes.Length; i++)
                    {
                        Type? type = requiredTypes[i];
#if DEBUG
                        args[i] = container.Resolve(type) ?? throw new InjectingServiceNotRegisteredException(type, type);
#else
                        args[i] = container.Resolve(type);
#endif
                    }

                    method.Invoke(obj, args);
                };
            }

            return multiDelegate;
        }
    }
}