using Source.Code.BattleField.View;
using Source.Code.StaticData;

namespace Source.Code.BattleField
{
    public class BattleFieldPresenter
    {
        private readonly IReadOnlyBattleFieldModel _model;
        private readonly BattleFieldView _view;


        public BattleFieldPresenter(IReadOnlyBattleFieldModel model, BattleFieldView view)
        {
            _model = model;
            _view = view;
            _model.OnWarriorAdded += AddWarriorOnView;
            _model.OnBossHitLine += HitLine;
            _model.OnWarriorSpawned += SpawnWarrior;
            _model.OnBossGetDamage += DealBossDamage;
        }

        private void DealBossDamage(int currentHp, int maxHp)
        {
            _view.UpdateBossHp(currentHp, maxHp);
        }

        private void SpawnWarrior(IWarrior warrior)
        {
            _view.SpawnWarrior(warrior);
        }

        private void HitLine(int index)
        {
            _view.HitLine(index);
        }

        private void AddWarriorOnView(IWarrior warrior) => 
            _view.CreateNewWarrior(warrior);
        
        
    }
}