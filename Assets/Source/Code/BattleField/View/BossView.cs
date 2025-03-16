using DG.Tweening;
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

        private int _displayingHp;

        public void Init(Sprite sprite, int bossStartHp)
        {
            _bossSprite.sprite = sprite;
            _bossHpText.text = $"{bossStartHp}/{bossStartHp}";
            _bossHpBar.fillAmount = 1f;
        }
        
        public void UpdateBossHp(int currentHp, int maxHp)
        {
            float barFill = (float)currentHp / maxHp;
        
            _bossHpBar.DOFillAmount(barFill, 0.5f);
        
            DOTween.To(() => _displayingHp, x =>
            {
                _displayingHp = x;
                _bossHpText.text = $"{_displayingHp}/{maxHp}";
            }, currentHp, 0.5f);
        }
    }
}
