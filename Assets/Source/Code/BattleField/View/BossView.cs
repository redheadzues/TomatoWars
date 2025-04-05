using System.Collections;
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

        private IdleNumber _displayingHp;
        private IdleNumber _bossMaxHp;
        private Coroutine _calculateBossHpCoroutine;
        
        public void Init(Sprite sprite, IdleNumber bossMaxHp)
        {
            _bossSprite.sprite = sprite;
            _displayingHp = bossMaxHp;
            DisplayBossHp();
            _bossHpBar.fillAmount = 1f;
        }
        
        public void UpdateBossHp(IdleNumber currentHp, IdleNumber bossMaxHp)
        {
            _bossMaxHp = bossMaxHp;
            float barFill = (float)(currentHp / _bossMaxHp);
        
            _bossHpBar.DOFillAmount(barFill, StaticConfig.TICK_INTERVAL);
        
            if(_calculateBossHpCoroutine != null)
                StopCoroutine(_calculateBossHpCoroutine);

            _calculateBossHpCoroutine = StartCoroutine(CalculateCurrentDisplayBossHp(currentHp, bossMaxHp));
        }

        private void DisplayBossHp()
        {
            _bossHpText.text = $"{_displayingHp}/{_bossMaxHp}";
        }

        private IEnumerator CalculateCurrentDisplayBossHp(IdleNumber currentHp, IdleNumber bossMaxHp)
        {
            float remainingTime = 0;
            var difference = _displayingHp - currentHp;
            var startHp = _displayingHp;


            while (remainingTime < StaticConfig.TICK_INTERVAL)
            {
                remainingTime += Time.time;
                var ratio = remainingTime / StaticConfig.TICK_INTERVAL;
                _displayingHp = startHp - difference * ratio;
                
                DisplayBossHp();

                yield return null;
            }

            _displayingHp = currentHp;
            DisplayBossHp();
        }
    }
}
