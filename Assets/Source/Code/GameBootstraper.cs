using System.Collections.Generic;
using Source.Code.Models;
using UnityEngine;

public class GameBootstraper : MonoBehaviour
{
    [SerializeField] private List<GameWindow> _windows;
    
    private readonly CoreModel _model = new();
    private ServiceProvider _serviceProvider;
     
    private void Awake()
    {
        _serviceProvider = new ServiceProvider(_model);
        
        _serviceProvider.Get<SaveLoadService>().Load();
        _serviceProvider.Get<StaticDataService>().LoadData();
        
        _windows.ForEach(window => window.SetProvider(_serviceProvider));
    }
}