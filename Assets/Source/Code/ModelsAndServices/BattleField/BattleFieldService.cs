using System;
using System.Collections;
using System.Linq;
using Source.Code.BattleField.Buff;
using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using Source.Code.Warriors;
using UnityEngine;
using Random = System.Random;

namespace Source.Code.ModelsAndServices.BattleField
{
    public interface IBattleFieldService : IService
    {
        event Action ReadyToStart;
        event Action StageCompleted;
        event Action TickCalculated;
        event Action<float, float> BossAttacked;
        event Action<IWarrior> WarriorAdded;
        IReadOnlyBattleFieldModel Model { get; }
        void Start();
        void Stop();
        void PrepareNewStage();
    }
    
    public class BattleFieldService : IBattleFieldService
    {
        private readonly CoreModel _coreModel;
        private readonly IStaticDataService _staticData;
        private readonly IWarriorFactory _warriorFactory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly BattleEffectSystem _battleEffect;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());
        
        private BattleFieldModel _battleModel;
        private IdleNumber _tikDamage;
        private Coroutine _updateCoroutine;
        private float _timeBeforeSpawn;

        public IReadOnlyBattleFieldModel Model => _battleModel;
        public event Action ReadyToStart;
        public event Action StageCompleted;
        public event Action TickCalculated;
        public event Action<float, float> BossAttacked;
        public event Action<IWarrior> WarriorAdded;
        
        public BattleFieldService(CoreModel model, IStaticDataService staticData, ICoroutineRunner runner, IWarriorFactory warriorFactory)
        {
            _coreModel = model;
            _coroutineRunner = runner;
            _warriorFactory = warriorFactory;
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
        }

        private void InitializeBattleModel()
        {
            _battleModel.SelectedWarriors = _coreModel.Player.SelectedWarrior;

            var bossConfig = _staticData.GetBossConfig(_coreModel.Player.Stage);

            if (bossConfig == null)
            {
                throw new NullReferenceException("Missing boss config");
            }

            _battleModel.BossSprite = bossConfig.Sprite;
            _battleModel.BossMaxHp = bossConfig.Hp;
            _battleModel.BossCurrentHp = bossConfig.Hp;
            _battleModel.BossDamagePerSecond = bossConfig.DamagePerSecond;
            _battleModel.BossAttackWidth = bossConfig.AttackWidth;

            ReadyToStart?.Invoke();
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

            SpawnWarrior();

            BossAttack();
          
            _battleModel.BossCurrentHp -= _tikDamage;

            TickCalculated?.Invoke();
            
            if (_battleModel.BossCurrentHp <= 0)
            {
                Stop();
                _coreModel.Player.Stage++;
                StageCompleted?.Invoke();
            }
        }

        private void AddTickDamage(Warrior warrior) =>
            _tikDamage += warrior.BaseDamagePerSecond;

        private void BossAttack()
        {
            var centerAttackPosition = (float)_random.NextDouble();
            var attackWidth = _battleModel.BossAttackWidth;

            var warriorForAttack = _battleModel.Warriors
                .Where(x => 
                    x.NormalizePosition.X > centerAttackPosition - attackWidth/2
                    &&
                    x.NormalizePosition.X < centerAttackPosition + attackWidth/2)
                .ToList();
            
            foreach (var warrior in warriorForAttack)
            {
                warrior.TakeDamage(_battleModel.BossDamagePerSecond * StaticConfig.TICK_INTERVAL);
            }
            
            BossAttacked?.Invoke(centerAttackPosition, attackWidth);
        }

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