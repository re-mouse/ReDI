using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ReDI
{
    internal class ServiceRegistration
    {
        private delegate void InjectDelegate(object instance, Container container);
        private delegate object ConstructorDelegate(Container container);
        
        private readonly ConstructorDelegate _constructor;
        private readonly InjectDelegate _inject;
        
        public bool AlwaysNewInstance { get; }
        public Type ServiceType { get; }
        public bool IsDisposable { get; }

        public bool IsInjected { get; private set; }
        public object? Instance { get; set; }
 
        public ServiceRegistration(Type serviceType, bool alwaysNewInstance, bool isDisposable, object instance)
        {
            ServiceType = serviceType;
            AlwaysNewInstance = alwaysNewInstance;
            IsDisposable = isDisposable;
            
            _constructor = GetConstructorDelegate();
            _inject = GetInjectingFieldsAndPropertiesDelegate();
            _inject += GetInjectingMethodsDelegate();
            Instance = instance;
            IsInjected = false;
        }

        private ConstructorDelegate GetConstructorDelegate()
        {
            var constructor = FindConstructor();
            var requiredTypes = constructor.GetParameters().Select(p => p.ParameterType).ToArray();
            var args = new object[requiredTypes.Length];
            
            return container =>
            {
                for (int i = 0; i < requiredTypes.Length; i++)
                {
                    Type? type = requiredTypes[i];
#if DEBUG
                        args[i] = container.Resolve(type) ?? throw new InjectingServiceNotRegisteredException(ServiceType, type);
#else
                        args[i] = container.Resolve(type);
#endif
                }

                return Activator.CreateInstance(ServiceType, args);
            };
        }

        private ConstructorInfo FindConstructor()
        {
            var constructors = ServiceType.GetConstructors();

            if (constructors.Length == 1)
            {
                return constructors.First();
            }
            
            var injectingConstructor = constructors
                    .Where(c => c.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0).ToArray();

#if DEBUG
            if (injectingConstructor.Length == 0)
            {
                throw new NotFoundInjectingConstructorException(ServiceType);
            }

            if (injectingConstructor.Length > 1)
            {
                throw new TooManyInjectingConstructorsException(ServiceType);
            }
#endif

            return injectingConstructor.First();
        }

        private InjectDelegate GetInjectingFieldsAndPropertiesDelegate()
        {
            InjectDelegate multiDelegate = (o, c) => { };
            
            var injectingFields = ServiceType
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
            
            var injectingProperties = ServiceType
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
        
        private InjectDelegate GetInjectingMethodsDelegate()
        {
            var injectingMethods = ServiceType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
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
                        args[i] = container.Resolve(type) ?? throw new InjectingServiceNotRegisteredException(ServiceType, type);
#else
                        args[i] = container.Resolve(type);
#endif
                    }

                    method.Invoke(obj, args);
                };
            }

            return multiDelegate;
        }

        public object Create(Container container)
        {
            var obj = _constructor(container);
            
            if (!AlwaysNewInstance)
            {
                Instance = obj;
            }

            return obj;
        }

        public object Inject(object obj, Container container)
        {
            IsInjected = true;
            _inject(obj, container);
            return obj;
        }
    }
}