using System;
using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.ModelsAndServices.Player
{
    public interface IPlayerService : IService
    {
        PlayerModel Model { get; } 
        event Action<CharacterTypeId> WarriorLevelUp;
        event Action<CharacterTypeId, Booster> BoosterUpdated;
        event Action<CurrencyTypeId, IdleNumber> BalanceChanged;
        bool TryAddBoosterToWarrior(Booster booster, CharacterTypeId characterTypeId);
        bool TrySpendCurrency(CurrencyTypeId currency, IdleNumber value);
        bool TryLevelUpWarrior(CharacterTypeId typeId);
        void AddCurrency(CurrencyTypeId currency, IdleNumber value);
    }
    
    public class PlayerService : IPlayerService
    {
        private readonly PlayerModel _model;

        public PlayerModel Model => _model; 
        public event Action<CharacterTypeId> WarriorLevelUp;
        public event Action<CharacterTypeId, Booster> BoosterUpdated;
        public event Action<CurrencyTypeId, IdleNumber> BalanceChanged;

        public PlayerService(PlayerModel model)
        {
            _model = model;
        }

        public bool TryAddBoosterToWarrior(Booster booster, CharacterTypeId characterTypeId)
        {
            var warrior = _model.OwnedWarriors[characterTypeId];
            
            if (warrior == null || !warrior.IsOwned || booster.TypeId == BoosterTypeId.None)
                return false;

            warrior.BoosterInfo = booster;
            
            BoosterUpdated?.Invoke(characterTypeId, booster);

            return true;
        }

        public bool TrySpendCurrency(CurrencyTypeId currency, IdleNumber value)
        {
            if (_model.Wallet.Balances.TryGetValue(currency, out var currentValue) && currentValue - value >= 0)
            {
                _model.Wallet.Balances[currency] -= value;
                
                BalanceChanged?.Invoke(currency, _model.Wallet.Balances[currency]);
                
                return true;
            }

            return false;
        }

        public bool TryLevelUpWarrior(CharacterTypeId typeId)
        {
            if (typeId == CharacterTypeId.None)
                return false;
            
            WarriorLevelUp?.Invoke(typeId);
            return true;
        }

        public void AddCurrency(CurrencyTypeId currency, IdleNumber value)
        {
            if(value <= 0)
                return;
            
            _model.Wallet.Balances[currency] += value;
            BalanceChanged?.Invoke(currency, _model.Wallet.Balances[currency]);
        }
    }
}