using System.Collections.Generic;
using Source.Code.ModelsAndServices;
using Source.Code.ModelsAndServices.BattleField;
using Source.Code.ModelsAndServices.Grid;
using Source.Code.ModelsAndServices.Player;
using Source.Code.Warriors;

namespace Source.Code
{
    public class Game
    {
        private enum GameState
        {   
            LoadOrInitData,
            RegisterServices,
            GameLoop,
        }
        
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly List<GameWindow> _windows;
        
        private CoreModel _model;
        private ServiceProvider _serviceProvider;

        public Game(ICoroutineRunner coroutineRunner, List<GameWindow> windows)
        {
            _model = new();
            _coroutineRunner = coroutineRunner;
            _windows = windows;
            _serviceProvider = new ServiceProvider();
            
            ApplyState(GameState.LoadOrInitData);
        }

        private void ApplyState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.LoadOrInitData:
                    LoadOrInitData();
                    break;
                case GameState.RegisterServices:
                    RegisterServices();
                    break;
                case GameState.GameLoop:
                    InitializeSceneObject();
                    break;
            }
        }
        
        private void LoadOrInitData()
        {
            var saveLoad = _serviceProvider.RegisterInstance<ISaveLoadService>(new SaveLoadService());
            var staticData = _serviceProvider.RegisterInstance<IStaticDataService>(new StaticDataService());

            saveLoad.LoadCompleted += OnLoadComplete;
            staticData.LoadCompleted += OnLoadComplete;
            
            
            saveLoad.LoadOrInit();
            staticData.LoadData();

            void OnLoadComplete()
            {
                if (saveLoad.IsLoaded && staticData.IsLoaded)
                {
                    saveLoad.LoadCompleted -= OnLoadComplete;
                    staticData.LoadCompleted -= OnLoadComplete;
                    ApplyState(GameState.RegisterServices);
                }
            }
        }

        private void RegisterServices()
        {
            var staticData = _serviceProvider.Get<IStaticDataService>();
            var playerService = _serviceProvider.RegisterInstance<IPlayerService>(new PlayerService(_model.Player));
            var warriorStats = _serviceProvider.RegisterInstance<IWarriorStatsService>(new WarriorStatsService(staticData, playerService));
            var warriorFactory = _serviceProvider.RegisterInstance<IWarriorFactory>(new WarriorFactory(warriorStats, staticData));
            
            _serviceProvider.RegisterLazy<IMergeGridService>(() => new MergeGridService(_model.Grid, staticData, playerService));
            _serviceProvider.RegisterLazy<IBattleFieldService>(() => new BattleFieldService(_model, staticData, _coroutineRunner, warriorFactory));
            
            ApplyState(GameState.GameLoop);
        }

        private void RegisterStaticData()
        {
            
        }


        private void InitializeSceneObject()
        {
            _windows.ForEach(x => x.SetProvider(_serviceProvider));
        }
    }
}