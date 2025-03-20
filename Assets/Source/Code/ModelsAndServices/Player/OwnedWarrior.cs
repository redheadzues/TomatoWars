using System;
using Source.Code.StaticData;

namespace Source.Code.ModelsAndServices.Player
{
    [Serializable]
    public class OwnedWarrior
    {
        private int _level;
        private WarriorBooster _booster;
        
        public WarriorTypeId TypeId;
        public bool IsOwned;

        public WarriorBooster Booster
        {
            get => _booster;
            set
            {
                _booster = value;
                BoosterUpdated?.Invoke(TypeId, _booster);
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                LevelUp?.Invoke(TypeId);
            }
        }

        public event Action<WarriorTypeId> LevelUp;
        public event Action<WarriorTypeId, WarriorBooster> BoosterUpdated;
    }
}