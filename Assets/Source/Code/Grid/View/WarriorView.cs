using System;
using Source.Code.BattleField;
using Source.Code.StaticData;
using Source.Code.UiGeneral;
using Source.Code.Warriors;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.Grid.View
{
    public class WarriorView : MonoBehaviour, IHighlightElement
    {
        [SerializeField] private Image _warriorIcon;
        [SerializeField] private Image _highlight;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Tooltip _tooltip;
        
        private IWarrior _warrior;
        public WarriorTypeId TypeId => _warrior.TypeId;
        public Collider2D Collider => _collider;

        private void Awake()
        {
            Highlight(false);
        }

        public void Init(IWarrior warrior)
        {
            _warrior = warrior;
            _warriorIcon.sprite = warrior.Icon;
            UpdateDescription();
        }
        
        public void Highlight(bool isActive)
        {
            _highlight.gameObject.SetActive(isActive);
        }

        public void UpdateDescription()
        {
            var description = $"Урон {_warrior.BaseDamagePerSecond}\n" +
                                $"Скорость {_warrior.BaseNormalizedSpeed}";
            
            _tooltip.SetDescription(description);
        }
    }
}