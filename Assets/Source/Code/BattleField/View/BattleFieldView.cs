using System;
using System.Collections.Generic;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.BattleField.View
{
    public class BattleFieldView : MonoBehaviour
    {
        [SerializeField] private WarriorView _prefab;
        [SerializeField] private List<Transform> _lines;

        private List<WarriorView> _warriors;

        public void UpdateWarriors(List<IWarrior> warriors)
        {
            if (warriors.Count > _warriors.Count)
            {
                var difference = warriors.Count - _warriors.Count;

                for (int i = warriors.Count - difference - 1; i > warriors.Count; i++)
                {
                    CreateNewWarrior(warriors[i]);
                }
            }
            
            _warriors.ForEach(warrior => warrior.UpdateWarrior());
        }

        private void CreateNewWarrior(IWarrior warrior)
        {
            var newWarrior = Instantiate(_prefab, _lines[warrior.LineIndex]);
            newWarrior.Init(warrior);
            _warriors.Add(newWarrior);
        }
    }

    public class WarriorView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;

        private IWarrior _warrior;
        private float _lastPositionY;
        private WarriorState _lastState;
        
        public int Id { get; private set; }

        public void Init(IWarrior warrior)
        {
            Id = warrior.Id;
            _sprite.sprite = warrior.Sprite;
        }

        public void UpdateWarrior()
        {
            if(_warrior.State == _lastState && _warrior.State != WarriorState.Walk)
                return;
            
            switch (_warrior.State)
            {
                case WarriorState.Walk:
                    Move();
                    break;
                case WarriorState.Fight:
                    Attack();
                    break;
                case WarriorState.Died:
                    Die();
                    break;
                
                default: 
                    Move();
                    break;
            }
        }

        private void Move()
        {
            var parrent = transform.parent;

            if (parrent == null)
                Debug.LogError("Transform has no parent, cannot calculate normalized position.");

            var lineLength = parrent.transform.localScale.y;

            transform.localPosition = new Vector2(transform.localPosition.x, lineLength * _warrior.NormalizePosition);


        }

        private void Attack()
        {
            
        }

        private void Die()
        {
            
        }
        
    }
}