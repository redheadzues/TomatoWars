using Source.Code.Models;
using Source.Code.Services;
using System;
using System.Linq;
using Source.Code.StaticData;
using Unity.VisualScripting;


namespace Source.Code.BattleField
{
    public class BattleFieldService : Service
    {
        private readonly CoreModel _coreModel;
        private readonly BattleFieldModel _battleModel;
        private readonly StaticDataService _dataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());

        public IReadOnlyBattleFieldModel BattleFieldModel => _battleModel;
        
        public BattleFieldService(CoreModel model, StaticDataService dataService, ICoroutineRunner runner)
        {
            _coreModel = model;
            _coroutineRunner = runner;
            _dataService = dataService;
            _battleModel = new BattleFieldModel();
            
            InitializeBattleModel();
        }

        private void InitializeBattleModel()
        {
            _battleModel.SelectedWarriors = _coreModel.Player.SelectedWarrior;
        }

        private Warrior GetFreeWarrior(WarriorType type)
        {
            var warrior = _battleModel.Warriors
                .FirstOrDefault(x => x.State == WarriorState.Die && x.Type == type);

            if (warrior != null)
            {
                warrior.State = WarriorState.Walk;
                warrior.Health = _dataService.GetWarrior(type).Health;
                return warrior;
            }

            return CreateNewWarrior(type);
        }

        private Warrior CreateNewWarrior(WarriorType type)
        {
            var config = _dataService.GetWarrior(type);
            var warrior = new Warrior
            {
                Type = type,
                State = WarriorState.Walk,
                Health = config.Health,
                DamagePerSecond = config.DamagePerSecond,
                Speed = config.Speed
            };
            
            _battleModel.Warriors.Add(warrior);

            return warrior;
        }
        
    }
}