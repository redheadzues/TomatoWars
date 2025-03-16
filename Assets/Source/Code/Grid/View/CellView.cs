using System;
using DG.Tweening;
using Source.Code.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.Grid.View
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectHighlighter;
        [SerializeField] private Image _targetHighlighter;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private BoosterIconDraggable _draggable;

        private GridBooster _booster;
        public int Index { get; private set; }
        public Collider2D Collider => _collider;
        public BoosterIconDraggable Draggable => _draggable;

        private void Awake()
        {
            _selectHighlighter.gameObject.SetActive(false);
            HighlightAsTarget(false);
        }

        public void Init(int index)
        {
            Index = index;
            _draggable.Init(this);
        }
        
        public void SetBooster(GridBooster booster)
        {
            if(booster.Index != Index)
                throw new InvalidOperationException($"Booster index {booster.Index} does not match expected index {Index}.");
            
            _booster = booster;
            _draggable.ReturnPosition();
            
            if (booster.TypeId == BoosterTypeId.None)
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

        public void HighlightAsTarget(bool isHighlight)
        {
            _targetHighlighter.gameObject.SetActive(isHighlight);
        }

        public void AnimateFail()
        {
            var baseColor = _targetHighlighter.color;
            HighlightAsTarget(true);
            
            DOTween.Sequence()
                .Append(_targetHighlighter.DOColor(Color.red, 0.5f))
                .Join(transform.DOShakeScale(0.5f, 0.2f, 0, 0))
                .OnComplete(() =>
                {
                    _targetHighlighter.color = baseColor;    
                    HighlightAsTarget(false);
                });
        }

        public void ReturnPosition()
        {
            _draggable.ReturnPosition();
        }


        public void HighlightAsSelect(bool isActive)
        {
            if (_booster == null)
                return;
            
            _selectHighlighter.gameObject.SetActive(isActive);
        }
    }
}