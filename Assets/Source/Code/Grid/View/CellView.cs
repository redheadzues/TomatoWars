using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.Grid.View
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _frame;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private BoosterIconDraggable _draggable;
        
        public int Index { get; private set; }
        public Collider2D Collider => _collider;
        public BoosterIconDraggable Draggable => _draggable;

        private void Awake()
        {
            HighlightSetActive(false);
        }

        public void Init(int index)
        {
            Index = index;
            _draggable.Init(this);
        }
        
        public void SetBooster(GridBooster booster)
        {
            if (booster == null)
            {
                _icon.gameObject.SetActive(false);
                _levelText.gameObject.SetActive(false);
                return;
            }
            
            _icon.sprite = booster.Icon;
            _levelText.text = booster.Level.ToString();
            
            _icon.gameObject.SetActive(true);
            _levelText.gameObject.SetActive(true);
        }

        public void AnimateFail()
        {
            throw new NotImplementedException();
        }

        public void ReturnPosition()
        {
            throw new NotImplementedException();
        }


        public void HighlightSetActive(bool isActive)
        {
            _frame.gameObject.SetActive(isActive);
        }
    }
}