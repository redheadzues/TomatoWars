using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.BattleField.View
{
    public class BossView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _bossSprite;
        [SerializeField] private Image _bossHpBar;
        [SerializeField] private TMP_Text _bossHpText;
        [SerializeField] private BossRewardsView _rewardPrefab;

        private readonly List<BossRewardsView> _rewardViews = new();
        private IdleNumber _displayingHp;

        public void Init(Sprite sprite, IdleNumber bossStartHp, List<BossReward> rewards)
        {
            _bossSprite.sprite = sprite;
            _bossHpText.text = $"{bossStartHp}/{bossStartHp}";
            _bossHpBar.fillAmount = 1f;
            
            InitRewards(rewards);
        }

        public void UpdateBossHp(IdleNumber currentHp, IdleNumber maxHp)
        {
            var idleRatio = currentHp / maxHp;
            var ratio = idleRatio.Value;

            _bossHpBar.DOFillAmount(ratio, 0.5f);

            this.IdleTweenTo(_displayingHp, currentHp, 0.5f, val =>
            {
                _displayingHp = val;
                _bossHpText.text = $"{_displayingHp:F2}/{maxHp:F2}";
            });
        }

        public void UpdateRewards(BossReward reward)
        {
            var rewardsView = _rewardViews.FirstOrDefault(x => x.Reward == reward);

            if (rewardsView == null)
            {
                print($"Cant find reward view for {reward.Treshold} treshold");
                return;
            }

            rewardsView.SetTaken();
        }
        
        private void InitRewards(List<BossReward> rewards)
        {
            foreach (var reward in rewards)
            {
                var rewardsView = Instantiate(_rewardPrefab, _bossHpBar.transform);
                rewardsView.Init(reward);
                _rewardViews.Add(rewardsView);
            }
        }
    }
}
