using Source.Code.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.BattleField.View
{
    public class BossRewardsView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private RectTransform _selfRect;
        
        private BossReward _reward;
        private RectTransform _parentRect;
        
        public BossReward Reward => _reward;

        public void Init(BossReward reward)
        {
            _reward = reward;
            _parentRect = _selfRect.parent as RectTransform;
            SetElementByNormalizePosition();
        }

        public void SetTaken() => 
            _icon.color = Color.green;

        private void SetElementByNormalizePosition()
        {
            float parentWidth = _parentRect.rect.width;
            float localX = _reward.Treshold * parentWidth;

            localX -= parentWidth * _selfRect.pivot.x;

            Vector2 anchoredPosition = _selfRect.anchoredPosition;
            anchoredPosition.x = localX;
            _selfRect.anchoredPosition = anchoredPosition;
        }
        
        

    }
}