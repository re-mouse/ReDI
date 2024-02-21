using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ReDI
{
    public class Container : IDisposable
    {

        private readonly List<ServiceRegistration> _registrations = new List<ServiceRegistration>();
        private readonly Dictionary<Type, HashSet<ServiceRegistration>> _registrationsByInterface = new Dictionary<Type, HashSet<ServiceRegistration>>();
        
        private bool _isDisposed;
        private HashSet<Type> _constructingTypes = new HashSet<Type>();
        
        internal Container(IEnumerable<BindingInfo> bindings)
        {
            var toBuild = new HashSet<ServiceRegistration>();
            
            foreach (var binding in bindings)
            {
                var registration = new ServiceRegistration(binding.boundType, binding.alwaysNewInstance, binding.isDisposable, binding.instance);
                
                foreach (var interfaceType in binding.associatedInterfaces)
                {
                    if (!_registrationsByInterface.TryGetValue(interfaceType, out var existingRegistrations))
                    {
                        existingRegistrations = new HashSet<ServiceRegistration>();
                        _registrationsByInterface[interfaceType] = existingRegistrations;
                    }
                    
                    existingRegistrations.Add(registration);
                }

                if (binding.createOnBuild)
                    toBuild.Add(registration);
                
                _registrations.Add(registration);
            }

            foreach (var buildingService in toBuild)
            {
                Build(buildingService);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
#if DEBUG
                throw new ObjectDisposedException("Container is already disposed");
#endif
                return;
            }

            foreach (var registration in _registrations)
            {
                if (registration.IsDisposable && registration.Instance is IDisposable disposable)
                    disposable.Dispose();
            }

            _isDisposed = true;
        }
        
        public T? Resolve<T>() where T : class
        {
            var obj = Resolve(typeof(T));
            return obj != null ? (T)obj : null;
        }
        
        public object? Resolve(Type type)
        {
            CheckIfDisposed();

            bool isList = IsGenericList(type);
            var serviceType = isList ? GetListTypeArgument(type) : type;
            
            if (!_registrationsByInterface.TryGetValue(serviceType, out var registrations))
                return null;
            
            if (IsGenericList(type))
            {
                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(serviceType);

                var instance = Activator.CreateInstance(constructedListType, registrations.Count) as IList;
                foreach (var registration in registrations)
                    instance.Add(Build(registration));
                
                return instance;
            }
            else
            {
                return Build(registrations.First());
            }
        }
        
        private bool IsGenericList(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));
        }
        
        private Type GetListTypeArgument(Type type)
        {
            return type.GetGenericArguments()[0];
        }
        
        private void CheckIfDisposed()
        {
#if DEBUG
            if (_isDisposed)
                throw new ObjectDisposedException("Container is disposed");
#endif
        }

        private object Build(ServiceRegistration registration)
        {
            if (registration.AlwaysNewInstance)
            {
                if (!_constructingTypes.Add(registration.ServiceType))
                    throw new CircularDependencyFoundException(registration.ServiceType);
                
                var obj = registration.Create(this);
                _constructingTypes.Remove(registration.ServiceType);
                
                registration.Inject(obj, this);
                return obj;
            }
            else if (registration.Instance == null)
            {
                if (!_constructingTypes.Add(registration.ServiceType))
                    throw new CircularDependencyFoundException(registration.ServiceType);
                
                registration.Instance = registration.Create(this);
                
                _constructingTypes.Remove(registration.ServiceType);
            }

            if (!registration.IsInjected)
            {
                registration.Inject(registration.Instance, this);
            }

            return registration.Instance;
        }
    }
}
