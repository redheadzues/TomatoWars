using System;
using System.Collections.Generic;

namespace Source.Code.ModelsAndServices
{
    public class ServiceProvider
    {
        private readonly Dictionary<Type, IService> _services = new ();
        private readonly Dictionary<Type, Func<IService>> _factories = new();

        public void RegisterLazy<T>(Func<T> factory) where T : class, IService => 
            _factories[typeof(T)] = factory;

        public T RegisterInstance<T>(T service) where T : class, IService
        {
            if (_services.ContainsKey(typeof(T)))
                throw new Exception($"Service {typeof(T)} is already registered!");

            _services[typeof(T)] = service;

            return _services[typeof(T)] as T;
        }

        public T Get<T>() where T : class, IService
        {
            if (_services.TryGetValue(typeof(T), out var data))
                return data as T;

            if (!_factories.TryGetValue(typeof(T), out var factory))
                throw new Exception($"Service {typeof(T)} not registered!");
        
            var newService = factory();
            _services[typeof(T)] = newService;

            return newService as T;
        }
    }
}