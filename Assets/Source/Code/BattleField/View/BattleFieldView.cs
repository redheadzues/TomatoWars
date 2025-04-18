using System.Collections.Generic;
using DG.Tweening;
using Source.Code.IdleNumbers;
using Source.Code.Warriors;
using UnityEngine;

namespace Source.Code.BattleField.View
{
    public class BattleFieldView : MonoBehaviour
    {
        [SerializeField] private WarriorView _prefab;
        [SerializeField] private SpriteRenderer _field;
        [SerializeField] private SpriteRenderer _lineBossAttack;
        [SerializeField] private BossView _bossView;
        
        
        private readonly List<WarriorView> _warriors = new();

        private void Awake()
        {
            Clear();
        }
        
        public void Init(Sprite bossSprite, IdleNumber bossMaxHp)
        {
            _bossView.Init(bossSprite, bossMaxHp);
        }
        
        public void Clear()
        {
            _warriors.ForEach(x => Destroy(x.gameObject));
            _warriors.Clear();

            /*foreach (Transform child in _field.transform)
            {
                DestroyImmediate(child.gameObject);
            }*/
        }
        
        public void CreateNewWarrior(IWarrior warrior)
        {
            var newWarrior = Instantiate(_prefab, _field.transform);
            
            newWarrior.Init(warrior);
            _warriors.Add(newWarrior);
        }

        public void UpdateWarriors()
        {
            _warriors.ForEach(warrior => warrior.UpdateWarrior());
        }

        public void ShowBossAttack(float centerAttack, float widthAttack)
        {
            _lineBossAttack.transform.localScale = 
                new Vector2(widthAttack ,_lineBossAttack.transform.localScale.y);
            
            _lineBossAttack.transform.localPosition =
                new Vector2(centerAttack - 0.5f, _lineBossAttack.transform.localPosition.y);

            _lineBossAttack.DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
        }
        
        public void UpdateBossHp(IdleNumber currentHp, IdleNumber maxHp)
        {
            _bossView.UpdateBossHp(currentHp, maxHp);
        }
    }
}