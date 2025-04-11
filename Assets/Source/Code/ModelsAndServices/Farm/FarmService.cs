using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Source.Code.IdleNumbers;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.ModelsAndServices.Farm
{
    public interface IFarmService : IService
    {
        IReadOnlyList<IFarmCharacter> FarmCharacters { get; }
        bool TryUpgradeCharacter(CharacterTypeId typeId);
    }
    
    public class FarmService : IFarmService, ICleanable
    {
        private readonly IPlayerService _playerService;
        private readonly IStaticDataService _staticDataService;
        private readonly FarmModel _model;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly List<FarmCharacter> _farmCharacters = new();
        
        private Coroutine _incomeCoroutine;
        
        public IReadOnlyList<IFarmCharacter> FarmCharacters => _farmCharacters;

        public FarmService(IPlayerService playerService, IStaticDataService staticDataService, FarmModel model, ICoroutineRunner runner)
        {
            _playerService = playerService;
            _staticDataService = staticDataService;
            _model = model;
            _coroutineRunner = runner;
            InitCharactersAndStartIncome();
        }

        public void CleanUp()
        {
            if (_incomeCoroutine != null)
                _coroutineRunner.StopCoroutine(_incomeCoroutine);
        }

        public bool TryUpgradeCharacter(CharacterTypeId typeId)
        {
            var config = _staticDataService.GetFarmCharacterConfig(typeId);
            var level = _model.CharactersLevel.GetValueOrDefault(typeId);

            if (config == null)
                throw new NullReferenceException($"Cant find FarmCharacterConfig by {typeId}");

            var cost = config.GetCostByLevel(level);

            if (_playerService.TrySpendCurrency(CurrencyTypeId.Gold, cost))
            {
                var newLevel = ++_model.CharactersLevel[typeId];

                var newCost = config.GetCostByLevel(newLevel);
                var newIncome = config.GetIncomeByLevel(newLevel);

                var character = _farmCharacters.FirstOrDefault(x => x.TypeId == typeId);

                if (character == null)
                    throw new NullReferenceException($"Cant find FarmCharacter in list by type {typeId}");

                character.Cost = newCost;
                character.IncomePerSecond = newIncome;
                character.Level = newLevel;
                
                return true;
            }

            return false;
        }


        private void InitCharactersAndStartIncome()
        {
            foreach (CharacterTypeId typeId in Enum.GetValues(typeof(CharacterTypeId)))
            {
                if(typeId == CharacterTypeId.None)
                    continue;
               
                var level = _model.CharactersLevel.GetValueOrDefault(typeId);
                var config = _staticDataService.GetFarmCharacterConfig(typeId);
                var icon = config.Icon;
                var cost = config.GetCostByLevel(level);
                var income = config.GetIncomeByLevel(level);
                var incomeTime = config.IncomeTime;

                _farmCharacters.Add(new FarmCharacter(typeId, icon, level, cost, income, incomeTime));
            }

            _incomeCoroutine = _coroutineRunner.StartCoroutine(IncomeCoroutine());
        }

        private IEnumerator IncomeCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(StaticConfig.TICK_INTERVAL);

                IdleNumber income = new();

                foreach (var character in _farmCharacters)
                {
                    character.RemainingTimeToIncome -= StaticConfig.TICK_INTERVAL;

                    if (character.RemainingTimeToIncome <= 0)
                    {
                        income += character.IncomePerSecond * character.IncomeTime;
                        character.RemainingTimeToIncome += character.IncomeTime;
                    }
                }
                
                if(income > 0)
                    _playerService.AddCurrency(CurrencyTypeId.Gold, income);
            }
        }
    }

    public interface IFarmCharacter
    {
        public CharacterTypeId TypeId { get; }
        public Sprite Icon { get; }
        public int Level { get; }
        public IdleNumber Cost { get; }
        public IdleNumber IncomePerSecond { get; }
        public float IncomeTime { get; }
    }
    
    public class FarmCharacter : IFarmCharacter
    {
        public CharacterTypeId TypeId { get; set; }
        public Sprite Icon { get; set; }
        public int Level { get; set; }
        public IdleNumber Cost { get; set; }
        public IdleNumber IncomePerSecond { get; set; }
        public float IncomeTime { get; set; }
        public float RemainingTimeToIncome;

        public FarmCharacter(CharacterTypeId typeId, Sprite icon, int level, IdleNumber cost, IdleNumber incomePerSecond, float incomeTime)
        {
            TypeId = typeId;
            Icon = icon;
            Level = level;
            Cost = cost;
            IncomePerSecond = incomePerSecond;
            IncomeTime = incomeTime;
            RemainingTimeToIncome = IncomeTime;
        }
    }
}