using Source.Code.Models;
using Source.Code.Services;
using System;
using System.Collections;
using System.Linq;
using Source.Code.StaticData;
using UnityEngine;
using Random = System.Random;

namespace Source.Code.BattleField
{
    public class BattleFieldService : Service
    {
        private const float TICK_INTERVAL = 0.5f;
        private const float SPAWN_TIME = 2f;
        
        private readonly CoreModel _coreModel;
        private readonly BattleFieldModel _battleModel;
        private readonly StaticDataService _dataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());

        private int _tikDamage;
        private Coroutine _updateCoroutine;
        private float _timeBeforeSpawn;

        public IReadOnlyBattleFieldModel BattleFieldModel => _battleModel;
        
        public BattleFieldService(CoreModel model, StaticDataService dataService, ICoroutineRunner runner)
        {
            _coreModel = model;
            _coroutineRunner = runner;
            _dataService = dataService;
            _battleModel = new BattleFieldModel();
            
            InitializeBattleModel();
        }

        public void Start() => 
            _updateCoroutine = _coroutineRunner.StartCoroutine(UpdatePerTick());

        public void Stop() =>
            _coroutineRunner.StopCoroutine(_updateCoroutine);

        private void InitializeBattleModel()
        {
            _battleModel.SelectedWarriors = _coreModel.Player.SelectedWarrior;

            var boss = _dataService.GetBoss(_coreModel.Player.Stage);

            if (boss == null)
            {
                throw new NullReferenceException("Missing boss config");
            }
            
            _battleModel.BossMaxHp = boss.Hp;
            _battleModel.BossCurrentHp = boss.Hp;
            _battleModel.BossDamagePerSecond = boss.DamagePerSecond;
        }

        private IEnumerator UpdatePerTick()
        {
            while (true)
            {
                Update();
                yield return new WaitForSeconds(TICK_INTERVAL);
            }
        }

        private void Update()
        {
            _tikDamage = 0;
            
            SpawnWarrior();
            BossAttack();
            
            foreach (var warrior in _battleModel.Warriors)
            {
                if (warrior.NormalizePosition == 1f)
                    warrior.State = WarriorState.Fight;
                
                switch (warrior.State)
                {
                    case WarriorState.Walk:
                        Move(warrior);
                        break;
                    case WarriorState.Fight:
                        AddTickDamage(warrior);
                        break;
                }
            }

            _battleModel.BossCurrentHp -= _tikDamage;
        }

        private void AddTickDamage(Warrior warrior) => 
            _tikDamage += warrior.DamagePerSecond;

        private void Move(Warrior warrior) => 
            warrior.NormalizePosition = Math.Clamp(warrior.NormalizePosition + warrior.NormalizedSpeed, 0f, 1f);

        private void BossAttack()
        {
            var lineIndexToAttack = _random.Next(0, 3);
            
            var warriorForAttack = _battleModel.Warriors
                .Where(x => x.LineIndex == lineIndexToAttack).ToList();

            foreach (var warrior in warriorForAttack)
            {
                warrior.Health -= (int)(_battleModel.BossDamagePerSecond * TICK_INTERVAL);

                if (warrior.Health <= 0)
                    warrior.State = WarriorState.Died;
            }
            
            _battleModel.BossHitLine(lineIndexToAttack);
        }

        private void SpawnWarrior()
        {
            var selectedWarriorIndex = _random.Next(0, 5);
            var warriorType = _battleModel.SelectedWarriors[selectedWarriorIndex];

            var warrior = GetFreeWarrior(warriorType);
            
            warrior.State = WarriorState.Walk;
            warrior.Health = warrior.MaxHealth;
            warrior.NormalizePosition = 0;
            warrior.LineIndex = _random.Next(0, 3);
            _battleModel.WarriorSpawn(warrior);
        }

        
        private Warrior GetFreeWarrior(WarriorTypeId typeId) => 
            _battleModel.Warriors.FirstOrDefault(x => x.TypeId == typeId && x.State == WarriorState.Died) 
            ?? CreateNewWarrior(typeId);

        private Warrior CreateNewWarrior(WarriorTypeId typeId)
        {
            var config = _dataService.GetWarrior(typeId);
            var warrior = new Warrior
            {
                TypeId = typeId,
                Sprite = config.Sprite,
                Health = config.Health,
                MaxHealth = config.Health,
                DamagePerSecond = config.DamagePerSecond,
                NormalizedSpeed = config.NormalizedSpeed
            };
            
            _battleModel.AddWarrior(warrior);

            return warrior;
        }
    }
}