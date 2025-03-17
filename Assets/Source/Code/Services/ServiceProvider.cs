using System;
using System.Collections.Generic;
using Source.Code.Services;

public class ServiceProvider
{
    private readonly Dictionary<Type, IService> _services = new ();
    private readonly Dictionary<Type, Func<IService>> _factories = new();

    public void RegisterLazy<T>(Func<T> factory) where T : class, IService => 
        _factories[typeof(T)] = factory;


    public T Get<T>() where T : class, IService
    {
        if (_services.TryGetValue(typeof(T), out var data))
            return (T)data;

        if (!_factories.TryGetValue(typeof(T), out var factory))
            throw new Exception($"Service {typeof(T)} not registered!");
        
        var newService = factory();
        _services[typeof(T)] = newService;

        return (T)newService;
    }

    public T RegisterInstance<T>(T service) where T : class, IService
    {
        var existingService = _services.GetValueOrDefault(typeof(T));

        if (_services.ContainsKey(typeof(T)))
            throw new Exception($"Service {typeof(T)} is already registered!");

        _services[typeof(T)] = service;

        return (T)_services[typeof(T)];
    }
}