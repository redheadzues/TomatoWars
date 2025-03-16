using System;
using System.Collections.Generic;
using System.Linq;
using Source.Code.Models;
using Source.Code.ModelsAndServices.Player;
using Source.Code.Services;
using Source.Code.StaticData;
using Random = System.Random;

namespace Source.Code.Grid
{
    public class MergeGridService : Service
    {
        private const int BOOSTER_LIMIT = 30;
        
        private readonly GridModel _gridModel;
        private readonly StaticDataService _staticData;
        private readonly PlayerService _playerService;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());

        public IMergeGridModel GridModel => _gridModel;
        public IReadOnlyList<WarriorTypeId> SelectedWarriors => _playerService.Model.SelectedWarrior;
        public bool IsEnableAddNewItem => GetFreeCellIndex() > -1;

        public MergeGridService(CoreModel model, StaticDataService staticData, PlayerService playerService)
        {
            _gridModel = model.Grid;
            _playerService = playerService;
            _staticData = staticData;

            if (_gridModel.GridBoosters == null)
                _gridModel.GridBoosters = Enumerable.Range(0, BOOSTER_LIMIT)
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

        public bool TryCreateNewBooster(out GridBooster booster)
        {
            booster = null;

            if (!_playerService.TrySpendGold(0))
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

            return true;
        }

        private GridBooster CreateNewBooster(int index, int level)
        {
            var values = Enum.GetValues(typeof(BoosterTypeId));
            var typeId = (BoosterTypeId)values.GetValue(_random.Next(1, values.Length));

            var config = _staticData.GetBooster(typeId);

            if (config == null)
                throw new NullReferenceException($"no config found by {typeId}");
            
            var newBooster = new GridBooster(index, typeId, level, config.Icon);
            
            _gridModel.GridBoosters[index] = newBooster;

            return newBooster;
        }

        private GridBooster CopyBoosterWithNewIndex(GridBooster booster, int newIndex) => 
            new(newIndex, booster.TypeId, booster.Level, booster.Icon);


        private GridBooster CreateEmptyBooster(int index) => 
            new GridBooster(index);

        private int GetFreeCellIndex() => 
            _gridModel.GridBoosters.FindIndex(booster => booster.TypeId == BoosterTypeId.None);
    }
}