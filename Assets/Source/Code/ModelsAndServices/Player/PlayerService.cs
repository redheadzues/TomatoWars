using System;
using Source.Code.Grid;
using Source.Code.Services;
using Source.Code.StaticData;

namespace Source.Code.ModelsAndServices.Player
{
    public class PlayerService : Service
    {
        private readonly PlayerModel _model;

        public PlayerModel Model => _model; 

        public PlayerService(PlayerModel model)
        {
            _model = model;
            SyncOwnedWarriorByType();
        }

        public bool TryAddBoosterToWarrior(GridBooster booster, WarriorTypeId warriorTypeId)
        {
            var warrior = _model.OwnedWarriors[warriorTypeId];

            if (warrior == null || !warrior.IsOwned || booster.TypeId == BoosterTypeId.None)
                return false;

            warrior.Booster = new WarriorBooster
            {
                BoosterTypeId = booster.TypeId,
                Level = booster.Level,
                Rarity = booster.Rarity
            };

            return true;
        }

        public bool TrySpendGold(Currency currency, int value)
        {
            if (_model.Wallet.Balances.TryGetValue(currency, out var currentValue) && currentValue - value >= 0)
            {
                _model.Wallet.Balances[currency] -= value;
                return true;
            }

            return false;
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
    }
}