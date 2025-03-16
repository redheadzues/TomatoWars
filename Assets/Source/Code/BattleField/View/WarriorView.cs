using System.Collections;
using DG.Tweening;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.BattleField.View
{
    public class WarriorView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        
        private IWarrior _warrior;
        private WarriorState _lastState;
        private Coroutine _moveCoroutine;
        private Tween _moveTween;
        private Tween _attackTween;
        
        public int Id { get; private set; }

        private void OnDisable()
        {
            ResetAllAction();
        }

        public void Init(IWarrior warrior)
        {
            Id = warrior.Id;
            _sprite.sprite = warrior.Icon;
            _warrior = warrior;
        }

        public void UpdateWarrior()
        {
            if(_warrior.State == _lastState)
                return;
            
            SwitchState();
        }

        private void SwitchState()
        {
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

            _lastState = _warrior.State;
        }

        private void Move()
        {
            if (_moveCoroutine == null)
                _moveCoroutine = StartCoroutine(MoveCoroutine());
        }

        private void Attack()
        {
           ResetAllAction();

            _attackTween = transform
                .DOLocalMoveY(0.6f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void Die()
        {
            ResetAllAction();
            gameObject.SetActive(false);
        }

        private void ResetAllAction()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }
            
            _moveTween?.Kill();
            _attackTween?.Kill();
        }
        

        private IEnumerator MoveCoroutine()
        {
            var parent = transform.parent;

            if (parent == null)
            {
                Debug.LogError("Transform has no parent, cannot move by normalized position.");
                yield break;
            }
            
            _moveTween = DOTween.Sequence()
                .Append(transform.DORotate(new Vector3(0, 0, 3), 0.3f))
                .Append(transform.DORotate(new Vector3(0, 0, -3), 0.3f))
                .SetLoops(-1);

            while (_warrior.State == WarriorState.Walk)
            {
                transform.localPosition = Vector2.MoveTowards(
                    transform.localPosition, 
                    new Vector2(transform.localPosition.x, _warrior.NormalizePosition - 0.5f), 
                    _warrior.NormalizedSpeed / (0.5f/Time.deltaTime) );

                yield return null;
            }
        }
    }
}