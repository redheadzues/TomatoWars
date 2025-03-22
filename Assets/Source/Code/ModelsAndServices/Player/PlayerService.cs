using System;
using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.ModelsAndServices.Player
{
    public interface IPlayerService : IService
    {
        public event Action<WarriorTypeId> WarriorLevelUp;
        public event Action<WarriorTypeId, Booster> BoosterUpdated;
        PlayerModel Model { get; } 
        bool TryAddBoosterToWarrior(Booster booster, WarriorTypeId warriorTypeId);
        bool TrySpendCurrency(Currency currency, int value);
        bool TryLevelUpWarrior(WarriorTypeId typeId);
    }
    
    public class PlayerService : IPlayerService
    {
        private readonly PlayerModel _model;

        public PlayerModel Model => _model; 
        public event Action<WarriorTypeId> WarriorLevelUp;
        public event Action<WarriorTypeId, Booster> BoosterUpdated;

        public PlayerService(PlayerModel model)
        {
            _model = model;
            SyncOwnedWarriorByType();
            SyncCurrencyByType();
        }

        public bool TryAddBoosterToWarrior(Booster booster, WarriorTypeId warriorTypeId)
        {
            var warrior = _model.OwnedWarriors[warriorTypeId];
            
            if (warrior == null || !warrior.IsOwned || booster.TypeId == BoosterTypeId.None)
                return false;

            warrior.BoosterInfo = booster;
            
            BoosterUpdated?.Invoke(warriorTypeId, booster);

            return true;
        }

        public bool TrySpendCurrency(Currency currency, int value)
        {
            if (_model.Wallet.Balances.TryGetValue(currency, out var currentValue) && currentValue - value >= 0)
            {
                _model.Wallet.Balances[currency] -= value;
                return true;
            }

            return false;
        }

        public bool TryLevelUpWarrior(WarriorTypeId typeId)
        {
            if (typeId == WarriorTypeId.None)
                return false;
            
            WarriorLevelUp?.Invoke(typeId);
            return true;
        }

        private void SyncOwnedWarriorByType()
        {
            foreach (WarriorTypeId typeId in Enum.GetValues(typeof(WarriorTypeId)))
            {
                _model.OwnedWarriors.TryAdd(typeId, new OwnedWarrior
                {
                    TypeId = typeId,
                    IsOwned = true,
                });
            }
        }
        
        private void SyncCurrencyByType()
        {
            foreach (Currency typeId in Enum.GetValues(typeof(Currency)))
            {
                _model.Wallet.Balances.TryAdd(typeId, 0);
            }
        }
    }
}