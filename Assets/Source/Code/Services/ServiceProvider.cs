using System;
using System.Collections.Generic;
using Source.Code.Models;
using Source.Code.Services;
using UnityEngine;

public class ServiceProvider
{
    private readonly Dictionary<Type, Service> _services = new ();
    private readonly Dictionary<Type, Func<Service>> _factories = new();
    private readonly CoreModel _model;
    
    public ServiceProvider(CoreModel model) => 
        _model = model;

    public void RegisterLazy<T>(Func<T> factory) where T : Service => 
        _factories[typeof(T)] = factory;


    public T Get<T>() where T : Service
    {
        if (_services.TryGetValue(typeof(T), out var data))
            return data as T;

        if (!_factories.TryGetValue(typeof(T), out var factory))
            throw new Exception($"Service {typeof(T)} not registered!");
        
        var newService = factory();
        _services[typeof(T)] = newService;

        return newService as T;
    }

    public T RegisterInstance<T>(Service service) where T : Service
    {
        if (_services.ContainsKey(typeof(T)))
            return null;

        _services[typeof(T)] = service;

        return _services[typeof(T)] as T;
    }
}