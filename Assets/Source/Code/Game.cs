using System.Collections.Generic;
using Source.Code.BattleField;
using Source.Code.Models;
using Source.Code.ModelsAndServices.Grid;
using Source.Code.ModelsAndServices.Player;
using Source.Code.Services;

namespace Source.Code
{
    public class Game
    {
        private enum GameState
        {   
            CreateServices,
            LoadData,
            GameLoop,
        }
        
        private readonly ICoroutineRunner _coroutineRunner;
        private CoreModel _model;

        private ServiceProvider _serviceProvider;
        private List<GameWindow> _windows;

        public Game(ICoroutineRunner coroutineRunner, List<GameWindow> windows)
        {
            _model = new();
            _coroutineRunner = coroutineRunner;
            _windows = windows;
            ApplyState(GameState.CreateServices);
        }

        private void ApplyState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.CreateServices:
                    CreateServices();
                    break;
                case GameState.LoadData:
                    LoadData();
                    break;
                case GameState.GameLoop:
                    InitializeSceneObject();
                    break;
            }
        }

        private void CreateServices()
        {
            _serviceProvider = new ServiceProvider();
            var staticData = _serviceProvider.RegisterInstance<IStaticDataService>(new StaticDataService());
            var playerService = _serviceProvider.RegisterInstance<IPlayerService>(new PlayerService(_model.Player));
            
            _serviceProvider.RegisterLazy<IMergeGridService>(() => new MergeGridService(_model, staticData, playerService));
            _serviceProvider.RegisterLazy<ISaveLoadService>(() => new SaveLoadService());
            _serviceProvider.RegisterLazy<IBattleFieldService>(() => new BattleFieldService(_model, staticData, _coroutineRunner));
            
            ApplyState(GameState.LoadData);
        }
        
        private void LoadData()
        {
            _serviceProvider.Get<IStaticDataService>().LoadData();
            _serviceProvider.Get<ISaveLoadService>().Load(() => ApplyState(GameState.GameLoop));
        }

        private void InitializeSceneObject()
        {
            _windows.ForEach(x => x.SetProvider(_serviceProvider));
        }
    }
}