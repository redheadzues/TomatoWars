using System;
using System.Collections.Generic;
using Source.Code.IdleNumbers;
using Source.Code.StaticData;

namespace Source.Code.ModelsAndServices.Player
{
    public interface IPlayerService : IService
    {
        IReadOnlyList<CharacterTypeId> SelectedCharacters { get; }
        int Stage { get; }
        event Action<CharacterTypeId> WarriorLevelUp;
        event Action<CharacterTypeId, Booster> BoosterUpdated;
        event Action<CurrencyTypeId, IdleNumber> BalanceChanged;
        bool TryAddBoosterToWarrior(Booster booster, CharacterTypeId characterTypeId);
        bool TrySpendCurrency(CurrencyTypeId currency, IdleNumber value);
        bool TryLevelUpWarrior(CharacterTypeId typeId);
        void AddCurrency(CurrencyTypeId currency, IdleNumber value);
        void IncreaseStage();
        IOwnedWarrior GetOwnedWarriorByType(CharacterTypeId typeId);
        List<IOwnedWarrior> GetAllOwnedWarriors();
        IdleNumber GetCurrencyBalance(CurrencyTypeId typeId);
        bool TryBuyShards(out Dictionary<CharacterTypeId, int> shards);
    }
    
    public class PlayerService : IPlayerService
    {
        private readonly PlayerModel _model;
        private readonly IStaticDataService _staticData;

        public IReadOnlyList<CharacterTypeId> SelectedCharacters => _model.SelectedCharacters;
        public int Stage => _model.Stage;
        
        public event Action<CharacterTypeId> WarriorLevelUp;
        public event Action<CharacterTypeId, Booster> BoosterUpdated;
        public event Action<CurrencyTypeId, IdleNumber> BalanceChanged;

        public PlayerService(PlayerModel model, IStaticDataService staticData)
        {
            _model = model;
            _staticData = staticData;
        }

        public bool TryAddBoosterToWarrior(Booster booster, CharacterTypeId characterTypeId)
        {
            var warrior = _model.OwnedWarriors[characterTypeId];
            
            if (warrior == null || warrior.Level == 0 || booster.TypeId == BoosterTypeId.None)
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

            var warrior = _model.OwnedWarriors[typeId];
            
            var requiredShardsCount = 
                _staticData.GetWarriorConfig(typeId).GetShardsCountByLevel(warrior.Level);

            if (warrior.ShardsCount < requiredShardsCount)
                return false;

            warrior.ShardsCount -= requiredShardsCount;
            warrior.Level++;

            warrior.RequiredShardsToNextLevel =
                _staticData.GetWarriorConfig(typeId).GetShardsCountByLevel(warrior.Level);
            
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

        public IOwnedWarrior GetOwnedWarriorByType(CharacterTypeId typeId) => 
            _model.OwnedWarriors.GetValueOrDefault(typeId);

        public List<IOwnedWarrior> GetAllOwnedWarriors()
        {
            List<IOwnedWarrior> output = new();
            
            foreach (var warrior in _model.OwnedWarriors.Values)
            {
                output.Add(warrior);
            }

            return output;
        }

        public IdleNumber GetCurrencyBalance(CurrencyTypeId typeId) =>
            _model.Wallet.Balances.GetValueOrDefault(typeId, new IdleNumber(int.MinValue));

        public bool TryBuyShards(out Dictionary<CharacterTypeId, int> shards)
        {
            shards = new();

            if (_model.Wallet.Balances[CurrencyTypeId.Gem] < StaticConfig.SHARDS_PACK_PRICE)
                return false;

            Dictionary<Rarity, List<OwnedWarrior>> charactersByRarity = new();
            
            foreach (var character in _model.OwnedWarriors.Values)
            {
                charactersByRarity[character.Rarity].Add(character);
            }
            
            var random = new Random();

            for (int i = 0; i < StaticConfig.SHARDS_PER_PACK; i++)
            {
                var chance = (float)random.NextDouble();
                var rarity = StaticConfig.GetRarityByChance(chance);

                var characterIndex = random.Next(0, charactersByRarity[rarity].Count);

                var character = charactersByRarity[rarity][characterIndex];

                var shardsCountRange = StaticConfig.GetShardsCountRangeByRarity(rarity);

                var shardsCount = random.Next((int)shardsCountRange.X, (int)shardsCountRange.Y+1);

                _model.OwnedWarriors[character.TypeId].ShardsCount += shardsCount;

                shards[character.TypeId] += shardsCount;
            }

            return true;
        }

        public void IncreaseStage() => 
            _model.Stage++;
    }
}