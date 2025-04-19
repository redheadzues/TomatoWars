using System.Collections.Generic;
using System.Linq;
using Source.Code.BattleField.View;
using Source.Code.ModelsAndServices.BattleField;
using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.BattleField
{
    public class BattleFieldPresenter
    {
        private readonly BattleFieldView _view;
        private readonly IBattleFieldService _battleFieldService;

        public BattleFieldPresenter(IBattleFieldService service, BattleFieldView view)
        {
            _battleFieldService = service;
            _view = view;
            
            _battleFieldService.WarriorAdded += AddWarriorView;
            _battleFieldService.BossAttacked += OnBossAttacked;
            _battleFieldService.ReadyToStart += OnReadyToStart;
            _battleFieldService.StageCompleted += OnStageCompleted;
            _battleFieldService.TickCalculated += OnTickCalculated;
            _battleFieldService.RewardsUpdated += OnRewardsUpdated;
            
            _battleFieldService.PrepareNewStage();
        }
        
        public void CleanUp()
        {
            _battleFieldService.WarriorAdded -= AddWarriorView;
            _battleFieldService.BossAttacked -= OnBossAttacked;
            _battleFieldService.ReadyToStart -= OnReadyToStart;
            _battleFieldService.StageCompleted -= OnStageCompleted;
            _battleFieldService.TickCalculated -= OnTickCalculated;
            _battleFieldService.RewardsUpdated -= OnRewardsUpdated;
        }
        
        private void OnBossAttacked(float centerAttack, float widthAttack) => 
            _view.ShowBossAttack(centerAttack, widthAttack);

        private void AddWarriorView(IWarrior warrior) => 
            _view.CreateNewWarrior(warrior);
        
        private void OnRewardsUpdated(BossReward reward) => 
            _view.UpdateBossReward(reward);

        private void OnReadyToStart()
        {
            List<BossReward> rewards = _battleFieldService.Rewards.Keys.ToList();
            
            _view.Init(_battleFieldService.Model.BossSprite, _battleFieldService.Model.BossMaxHp, rewards);
            _battleFieldService.Start();
        }
        
        private void OnStageCompleted()
        {
            _view.Clear();
            _battleFieldService.PrepareNewStage();
        }

        private void OnTickCalculated()
        {
            _view.UpdateBossHp(_battleFieldService.Model.BossCurrentHp, _battleFieldService.Model.BossMaxHp);
            _view.UpdateWarriors();
        }
    }
}