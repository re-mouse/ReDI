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
        private Container _parent;
        private HashSet<Type> _constructingTypes = new HashSet<Type>();
        
        internal Container(IEnumerable<BindingInfo> bindings, Container parent)
        {
            _parent = parent;
            var toBuild = new HashSet<ServiceRegistration>();
            
            foreach (var binding in bindings)
            {
                var registration = new ServiceRegistration(binding.InstanceType, binding.AlwaysNewInstance, binding.IsDisposable, binding.Instance);
                
                foreach (var interfaceType in binding.AssociatedInterfaces)
                {
                    if (!_registrationsByInterface.TryGetValue(interfaceType, out var existingRegistrations))
                    {
                        existingRegistrations = new HashSet<ServiceRegistration>();
                        _registrationsByInterface[interfaceType] = existingRegistrations;
                    }
                    
                    existingRegistrations.Add(registration);
                }

                if (binding.CreateOnBuild)
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
            return obj != null ? (T)obj : _parent?.Resolve<T>();
        }
        
        public object? Resolve(Type type)
        {
            CheckIfDisposed();

            if (IsGenericList(type))
            {
                return ResolveList(type) ?? _parent?.ResolveList(type);
            }
            else if (IsGeneric(type))
            {
                return ResolveGeneric(type) ?? _parent?.ResolveGeneric(type);
            }
            else
            {
                return ResolveSingle(type) ?? _parent?.ResolveSingle(type);
            }
        }
        
        private bool IsGeneric(Type type)
        {
            return type.IsGenericType;
        }

        private object? ResolveGeneric(Type type)
        {
            var definition = type.GetGenericTypeDefinition();
            if (!_registrationsByInterface.TryGetValue(definition, out var registrations))
            {
                return null;
            }

            return Build(registrations.First(), type);
        }

        private object? ResolveSingle(Type type)
        {
            if (!_registrationsByInterface.TryGetValue(type, out var registrations))
            {
                return null;
            }

            return Build(registrations.First());
        }

        private object? ResolveList(Type type)
        {
            var serviceType = GetListTypeArgument(type);

            if (!_registrationsByInterface.TryGetValue(serviceType, out var registrations))
            {
                return null;
            }

            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(serviceType);

            var instance = Activator.CreateInstance(constructedListType, registrations.Count) as IList;
            foreach (var registration in registrations)
            {
                instance.Add(Build(registration));
            }

            return instance;
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

        private object Build(ServiceRegistration registration, Type concreteType = null)
        {
            if (registration.AlwaysNewInstance || registration.Instance == null)
            {
                CheckForCircularDependency(registration);
                registration.Instance = registration.Create(this, concreteType);
                _constructingTypes.Remove(registration.ServiceType);
            }

            if (!registration.IsInjected)
            {
                registration.Inject(registration.Instance, this, concreteType);
            }

            return registration.Instance;
        }

        private void CheckForCircularDependency(ServiceRegistration registration)
        {
            if (!_constructingTypes.Add(registration.ServiceType))
            {
                throw new CircularDependencyFoundException(registration.ServiceType);
            }
        }
    }
}
