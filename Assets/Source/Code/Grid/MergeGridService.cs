using System;
using Source.Code.Models;
using Source.Code.Services;
using Source.Code.StaticData;

namespace Source.Code.Grid
{
    public class MergeGridService : Service
    {
        private const int BOOSTER_LIMIT = 30;
        
        private readonly GridModel _gridModel;
        private readonly PlayerModel _playerModel;
        private readonly StaticDataService _staticData;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());

        public IMergeGridModel GridModel => _gridModel;
        public bool IsEnableAddNewItem => GetFreeCellIndex() > -1;

        public MergeGridService(CoreModel model, StaticDataService staticData)
        {
            _gridModel = model.Grid;
            _playerModel = model.Player;
            _staticData = staticData;
        }

        public bool TryMerge(int firstIndex, int secondIndex, out GridBooster booster, out int emptyIndex)
        {
            booster = null;
            emptyIndex = -1;
            
            if (_gridModel.GridBoosters[firstIndex].TypeId == _gridModel.GridBoosters[secondIndex].TypeId
                && _gridModel.GridBoosters[firstIndex].Level == _gridModel.GridBoosters[secondIndex].Level)
            {
                _gridModel.GridBoosters[secondIndex] = null;
                var newLevel = _gridModel.GridBoosters[firstIndex].Level + 1;
                _gridModel.GridBoosters[firstIndex] = CreateNewBooster(firstIndex, newLevel);

                booster = _gridModel.GridBoosters[firstIndex];
                emptyIndex = secondIndex;
                
                return true;
            }

            return false;
        }

        public bool TryCreateNewBooster(out GridBooster booster)
        {
            booster = null;
            //to do check money

            int freeIndex = GetFreeCellIndex();

            if (freeIndex == -1)
                return false;
            
            int count = 0;
            int lvlSum = 0;
            
            foreach (var gridBooster in _gridModel.GridBoosters)
            {
                if (gridBooster != null)
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
            var values = Enum.GetValues(typeof(GridBoosterTypeId));
            var typeId = (GridBoosterTypeId)values.GetValue(_random.Next(values.Length));

            var config = _staticData.GetBooster(typeId);

            if (config == null)
                throw new NullReferenceException($"no config found by {typeId}");
            
            var newBooster = new GridBooster(index, typeId, level, config.Icon);
            
            _gridModel.GridBoosters[index] = newBooster;

            return newBooster;
        }

        private int GetFreeCellIndex() => 
            _gridModel.GridBoosters.FindIndex(booster => booster == null);
    }
}