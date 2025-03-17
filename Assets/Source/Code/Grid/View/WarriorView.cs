using System;
using Source.Code.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.Grid.View
{
    public class WarriorView : MonoBehaviour, IHighlightElement
    {
        [SerializeField] private Image _warriorIcon;
        [SerializeField] private Image _highlight;
        [SerializeField] private Collider2D _collider;

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
        }

        public void Highlight(bool isActive)
        {
            _highlight.gameObject.SetActive(isActive);
        }
    }

    public interface IHighlightElement
    {
        void Highlight(bool isActive);
    }
}