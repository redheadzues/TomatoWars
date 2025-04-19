using System;
using System.Collections.Generic;
using System.Linq;
using Source.Code.Grid;
using Source.Code.IdleNumbers;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;
using Random = System.Random;

namespace Source.Code.ModelsAndServices.Grid
{
    public interface IMergeGridService : IService
    {
        IReadOnlyGridModel GridModel { get; }
        IReadOnlyList<CharacterTypeId> SelectedWarriors { get; }
        bool IsEnableAddNewItem { get; }
        IdleNumber CurrentCost { get; }
        bool TryMerge(int hostIndex, int inputIndex, out GridBooster newHostBooster, out int emptyIndex);
        bool TryMerge(CharacterTypeId characterType, int boosterIndex, out GridBooster booster);
        bool TryCreateNewBooster(out GridBooster booster);

    }
    
    public class MergeGridService : IMergeGridService
    {
        private readonly GridModel _gridModel;
        private readonly IStaticDataService _staticData;
        private readonly IPlayerService _playerService;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());
        
        public IReadOnlyGridModel GridModel => _gridModel;
        public IReadOnlyList<CharacterTypeId> SelectedWarriors => _playerService.SelectedCharacters;
        public bool IsEnableAddNewItem => GetFreeCellIndex() > -1;
        public IdleNumber CurrentCost => (_gridModel.BoostersCreated + 1) * 10;

        public MergeGridService(GridModel model, IStaticDataService staticData, IPlayerService playerService)
        {
            _gridModel = model;
            _playerService = playerService;
            _staticData = staticData;

            if (_gridModel.GridBoosters == null)
                _gridModel.GridBoosters = Enumerable.Range(0, StaticConfig.BOOSTER_LIMIT)
                    .Select(i => new GridBooster(i)).ToList();
        }

        public bool TryMerge(int hostIndex, int inputIndex, out GridBooster newHostBooster, out int emptyIndex)
        {
            newHostBooster = null;
            emptyIndex = inputIndex;
            
            if (_gridModel.GridBoosters[hostIndex].TypeId == _gridModel.GridBoosters[inputIndex].TypeId
                && _gridModel.GridBoosters[hostIndex].Level == _gridModel.GridBoosters[inputIndex].Level)
            {
                _gridModel.GridBoosters[inputIndex] = CreateEmptyBooster(inputIndex);
                var newLevel = _gridModel.GridBoosters[hostIndex].Level + 1;
                _gridModel.GridBoosters[hostIndex] = CreateNewBooster(hostIndex, newLevel);

                newHostBooster = _gridModel.GridBoosters[hostIndex];
                
                return true;
            }

            if (_gridModel.GridBoosters[hostIndex].TypeId == BoosterTypeId.None)
            {
                _gridModel.GridBoosters[hostIndex] = CopyBoosterWithNewIndex(_gridModel.GridBoosters[inputIndex], hostIndex);
                _gridModel.GridBoosters[inputIndex] = CreateEmptyBooster(inputIndex);

                newHostBooster = _gridModel.GridBoosters[hostIndex];

                return true;
            }

            return false;
        }
        
        public bool TryMerge(CharacterTypeId characterType, int boosterIndex, out GridBooster booster)
        {
            booster = _gridModel.GridBoosters[boosterIndex];

            Booster boosterInfo = new(booster.TypeId, booster.Level, booster.Rarity);
     
            if (_playerService.TryAddBoosterToWarrior(boosterInfo, characterType))
            {
                booster = CreateEmptyBooster(boosterIndex);
                return true;
            }

            return false;
        }

        public bool TryCreateNewBooster(out GridBooster booster)
        {
            booster = null;

            if (!_playerService.TrySpendCurrency(CurrencyTypeId.Gold, CurrentCost))
                return false;

            int freeIndex = GetFreeCellIndex();

            if (freeIndex == -1)
                return false;
            
            int count = 0;
            int lvlSum = 0;
            
            foreach (var gridBooster in _gridModel.GridBoosters)
            {
                if (gridBooster.TypeId != BoosterTypeId.None)
                {
                    count++;
                    lvlSum += gridBooster.Level;
                }
            }

            var averageLvl  = (count > 0) ? lvlSum / count : 1;
            booster = CreateNewBooster(freeIndex, averageLvl );
            _gridModel.BoostersCreated++;

            return true;
        }

        private GridBooster CreateNewBooster(int index, int level)
        {
            var values = Enum.GetValues(typeof(BoosterTypeId));
            var typeId = (BoosterTypeId)values.GetValue(_random.Next(1, values.Length));

            var config = _staticData.GetBoosterConfig(typeId);

            if (config == null)
                throw new NullReferenceException($"no config found by {typeId}");
            
            var newBooster = new GridBooster(index, typeId, level, config.Icon);
            
            _gridModel.GridBoosters[index] = newBooster;

            return newBooster;
        }
        
        private GridBooster CreateEmptyBooster(int index)
        {
            var booster =  new GridBooster(index);
            _gridModel.GridBoosters[index] = booster;

            return booster;
        }

        private GridBooster CopyBoosterWithNewIndex(GridBooster booster, int newIndex) => 
            new(newIndex, booster.TypeId, booster.Level, booster.Icon);

        private int GetFreeCellIndex() => 
            _gridModel.GridBoosters.FindIndex(booster => booster.TypeId == BoosterTypeId.None);
    }
}