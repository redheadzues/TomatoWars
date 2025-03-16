using System.Linq;
using Source.Code.Grid;
using Source.Code.Models.Player;
using Source.Code.StaticData;

namespace Source.Code.Services
{
    public class PlayerService : Service
    {
        private readonly PlayerModel _model;

        public PlayerModel Model => _model; 

        public PlayerService(PlayerModel model)
        {
            _model = model;
        }

        public bool TryAddBoosterToWarrior(GridBooster booster, WarriorTypeId warriorTypeId)
        {
            var warrior = _model.OwnedWarriors.FirstOrDefault(x => x.TypeId == warriorTypeId);

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

        public bool TrySpendGold(int value)
        {
            if (_model.Gold - value >= 0)
            {
                _model.Gold -= value;
                return true;
            }

            return false;
        }

    }
}