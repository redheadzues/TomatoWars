using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Source.Code.StaticData;
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
        
        public void Init(Sprite bossSprite, int bossMaxHp)
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

        public void UpdateWarriors(IReadOnlyList<IWarrior> warriors)
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
        
        public void SpawnWarrior(IWarrior warrior)
        {
            var view = _warriors.FirstOrDefault(x => x.Warrior == warrior);

            if (view == null)
            {
                Debug.LogWarning("view for spawn not found");
                return;
            }
            
            view.gameObject.SetActive(true);
        }

        public void UpdateBossHp(int currentHp, int maxHp)
        {
            _bossView.UpdateBossHp(currentHp, maxHp);
        }
    }
}