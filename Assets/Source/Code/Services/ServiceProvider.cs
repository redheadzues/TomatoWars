using System;
using System.Collections.Generic;
using Source.Code.Models;
using Source.Code.Services;

public class ServiceProvider
{
    private readonly Dictionary<Type, ICoreModelService> _services = new ();
    private readonly CoreModel _model;
    
    public ServiceProvider(CoreModel model)
    {
        _model = model;
    }

    public T Get<T>() where T : class, ICoreModelService, new()
    {
        
        if (_services.TryGetValue(typeof(T), out var data))
            return data as T;

        var newService = new T();
        newService.Init(_model);

        _services[typeof(T)] = newService;
        
        return newService;
    }

}