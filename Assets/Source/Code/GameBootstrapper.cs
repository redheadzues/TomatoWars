using System.Collections.Generic;
using Source.Code.BattleField;
using Source.Code.Grid;
using Source.Code.Models;
using Source.Code.Services;
using UnityEngine;

namespace Source.Code
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private List<GameWindow> _windows;
    
        private readonly CoreModel _model = new();
        private ServiceProvider _serviceProvider;
     
        private void Awake()
        {
            _serviceProvider = new ServiceProvider(_model);
        
            RegisterServices();
        
            _serviceProvider.Get<SaveLoadService>().Load();
            _serviceProvider.Get<StaticDataService>().LoadData();
        
            _windows.ForEach(window => window.SetProvider(_serviceProvider));
        }

        private void RegisterServices()
        {
            var staticData = new StaticDataService();
            _serviceProvider.RegisterInstance<StaticDataService>(staticData);
            
            _serviceProvider.RegisterLazy(() => new MergeGridService(_model, staticData));
            _serviceProvider.RegisterLazy(() => new SaveLoadService());
            _serviceProvider.RegisterLazy(() => new BattleFieldService(_model, staticData, this));
        }
    }
}