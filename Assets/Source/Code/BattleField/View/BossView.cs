using System.Collections.Generic;
using DG.Tweening;
using Source.Code.IdleNumbers;
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
        [SerializeField] private Image _prefab;

        private IdleNumber _displayingHp;
        private List<Image> _rewardIcons;

        public void Init(Sprite sprite, IdleNumber bossStartHp)
        {
            _bossSprite.sprite = sprite;
            _bossHpText.text = $"{bossStartHp}/{bossStartHp}";
            _bossHpBar.fillAmount = 1f;
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
    }
}
