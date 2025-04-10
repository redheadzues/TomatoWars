using System;
using System.Collections.Generic;
using Source.Code.IdleNumbers;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.ModelsAndServices.Farm
{
    public interface IFarmService : IService
    {
        public IReadOnlyDictionary<CharacterTypeId, FarmCharacter> FarmCharacters { get; }
        bool TryUpgradeCharacter(CharacterTypeId typeId);
    }
    
    public class FarmService : IFarmService
    {
        private readonly IPlayerService _playerService;
        private readonly IStaticDataService _staticDataService;
        private readonly FarmModel _model;
        private readonly Dictionary<CharacterTypeId, FarmCharacter> _farmCharacters;

        public IReadOnlyDictionary<CharacterTypeId, FarmCharacter> FarmCharacters => _farmCharacters;

        public FarmService(IPlayerService playerService, IStaticDataService staticDataService, FarmModel model)
        {
            _playerService = playerService;
            _staticDataService = staticDataService;
            _model = model;
            _farmCharacters = new();
            InitCharacters();
        }

        public bool TryUpgradeCharacter(CharacterTypeId typeId)
        {
            var config = _staticDataService.GetFarmCharacterConfig(typeId);
            var level = _model.CharactersLevel.GetValueOrDefault(typeId);

            if (config == null)
                throw new NullReferenceException($"Cant find FarmCharacterConfig by {typeId}");

            var cost = config.GetCostByLevel(level);

            if (_playerService.TrySpendCurrency(Currency.Gold, cost))
            {
                _model.CharactersLevel[typeId]++;
                ConfigureCharacter(typeId);
                return true;
            }

            return false;
        }

        private void InitCharacters()
        {
            foreach (CharacterTypeId typeId in Enum.GetValues(typeof(CharacterTypeId)))
            {
                if(typeId == CharacterTypeId.None)
                    continue;
               
                ConfigureCharacter(typeId);
            }
        }

        private void ConfigureCharacter(CharacterTypeId typeId)
        {
            var level = _model.CharactersLevel.GetValueOrDefault(typeId);
            var config = _staticDataService.GetFarmCharacterConfig(typeId);
            var icon = config.Icon;
            var cost = config.GetCostByLevel(level);
            var farmValue = config.GetFarmValueByLevel(level);

            var character = new FarmCharacter(typeId, icon, level, cost, farmValue);
                
            _farmCharacters[typeId] = character;
        }
    }

    public class FarmCharacter
    {
        public CharacterTypeId TypeId { get; private set; }
        public Sprite Icon { get; private set; }
        public int Level { get; private set; }
        public IdleNumber Cost { get; private set; }
        public IdleNumber FarmValue { get; private set; }

        public FarmCharacter(CharacterTypeId typeId, Sprite icon, int level, IdleNumber cost, IdleNumber farmValue)
        {
            TypeId = typeId;
            Icon = icon;
            Level = level;
            Cost = cost;
            FarmValue = farmValue;
        }
    }
}