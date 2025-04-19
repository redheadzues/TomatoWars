using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Source.Code.BattleField.Buff;
using Source.Code.IdleNumbers;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;
using Source.Code.Warriors;
using UnityEngine;
using Random = System.Random;

namespace Source.Code.ModelsAndServices.BattleField
{
    public interface IBattleFieldService : IService
    {
        IReadOnlyBattleFieldModel Model { get; }
        IReadOnlyDictionary<BossReward, bool> Rewards { get; }
        event Action ReadyToStart;
        event Action StageCompleted;
        event Action TickCalculated;
        event Action<float, float> BossAttacked;
        event Action<IWarrior> WarriorAdded;
        event Action<BossReward> RewardsUpdated; 
        void Start();
        void Stop();
        void PrepareNewStage();
    }
    
    public class BattleFieldService : IBattleFieldService
    {
        private readonly IPlayerService _playerService;
        private readonly IStaticDataService _staticData;
        private readonly IWarriorFactory _warriorFactory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly BattleEffectSystem _battleEffect;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());
        
        private BattleFieldModel _battleModel;
        private BossAttackHandler _bossAttackHandler;
        private IdleNumber _tikDamage;
        private Coroutine _updateCoroutine;
        private float _timeBeforeSpawn;

        public IReadOnlyBattleFieldModel Model => _battleModel;
        public IReadOnlyDictionary<BossReward, bool> Rewards => _battleModel.Rewards;
        public event Action ReadyToStart;
        public event Action StageCompleted;
        public event Action TickCalculated;
        public event Action<float, float> BossAttacked;
        public event Action<IWarrior> WarriorAdded;
        public event Action<BossReward> RewardsUpdated; 
        
        public BattleFieldService(IStaticDataService staticData, ICoroutineRunner runner, IWarriorFactory warriorFactory, IPlayerService playerService)
        {
            _coroutineRunner = runner;
            _warriorFactory = warriorFactory;
            _playerService = playerService;
            _staticData = staticData;
        }

        public void Start() => 
            _updateCoroutine = _coroutineRunner.StartCoroutine(UpdatePerTick());

        public void Stop() =>
            _coroutineRunner.StopCoroutine(_updateCoroutine);
        
        public void PrepareNewStage()
        {
            _battleModel = new();
            InitializeBattleModel();
            _bossAttackHandler = new BossAttackHandler(_battleModel);
            _timeBeforeSpawn = StaticConfig.VEGETABLES_SPAWN_TIME;
            ReadyToStart?.Invoke();
        }

        private void InitializeBattleModel()
        {
            _battleModel.SelectedWarriors =  _playerService.SelectedCharacters.ToList();

            var bossConfig = _staticData.GetBossConfig(_playerService.Stage);

            if (bossConfig == null)
                throw new NullReferenceException("Missing boss config");

            _battleModel.BossSprite = bossConfig.Sprite;
            _battleModel.BossMaxHp = bossConfig.Hp;
            _battleModel.BossCurrentHp = bossConfig.Hp;
            _battleModel.AttackConfig = bossConfig.AttackConfig;
            
            bossConfig.Rewards.ForEach(x => _battleModel.Rewards[x] = false);
        }

        private IEnumerator UpdatePerTick()
        {
            while(true)
            {
                Update();
                yield return new WaitForSeconds(StaticConfig.TICK_INTERVAL);
            }
        }
        
        private void Update()
        {
            _tikDamage = 0;
           
            foreach (var warrior in _battleModel.Warriors)
            {
                switch (warrior.State)
                {
                    case WarriorState.Walk:
                        warrior.Move(StaticConfig.TICK_INTERVAL);
                        break;
                    case WarriorState.Fight:
                        AddTickDamage(warrior);
                        break;
                }
            }
            
            _battleModel.BossCurrentHp -= _tikDamage;

            GiveRewards();


            if (_timeBeforeSpawn <= 0)
            {
                SpawnWarrior();
                _timeBeforeSpawn = StaticConfig.VEGETABLES_SPAWN_TIME;
            }
            else
            {
                _timeBeforeSpawn -= StaticConfig.TICK_INTERVAL;
            }

            _bossAttackHandler.Update(BossAttacked);
          
            TickCalculated?.Invoke();
            
            if (_battleModel.BossCurrentHp <= 0)
            {
                Stop();
                _playerService.IncreaseStage();
                StageCompleted?.Invoke();
            }
        }

        private void GiveRewards()
        {
            var reward = _battleModel.Rewards
                .FirstOrDefault(x
                    => x.Value == false
                       && x.Key.Treshold > _battleModel.BossCurrentHp / _battleModel.BossMaxHp).Key;

            if (reward != null)
            {
                _playerService.AddCurrency(reward.TypeId, reward.Value);
                _battleModel.Rewards[reward] = true;
                RewardsUpdated?.Invoke(reward);
            }
        }

        private void AddTickDamage(Warrior warrior) =>
            _tikDamage += warrior.BaseDamagePerSecond;


        private void SpawnWarrior()
        {
            CharacterTypeId characterType = CharacterTypeId.None;

            while (characterType == CharacterTypeId.None)
            {
                var selectedWarriorIndex = _random.Next(0, _battleModel.SelectedWarriors.Count);
                characterType = _battleModel.SelectedWarriors[selectedWarriorIndex];
            }

            var warrior = GetFreeWarrior(characterType);

            var newNormalizedPositionX = (float)_random.NextDouble();
            warrior.ResetWarrior(newNormalizedPositionX);
        }

        
        private Warrior GetFreeWarrior(CharacterTypeId typeId) => 
            _battleModel.Warriors.FirstOrDefault(x => x.TypeId == typeId && x.State == WarriorState.Died) 
            ?? CreateNewWarrior(typeId);

        private Warrior CreateNewWarrior(CharacterTypeId typeId)
        {
            var warrior = _warriorFactory.CreateWarrior(typeId);
            _battleModel.Warriors.Add(warrior);
            WarriorAdded?.Invoke(warrior);

            return warrior;
        }
    }
}