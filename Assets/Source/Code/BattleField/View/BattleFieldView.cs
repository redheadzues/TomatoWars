using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using Source.Code.Warriors;
using UnityEngine;

namespace Source.Code.BattleField.View
{
    public class BattleFieldView : MonoBehaviour
    {
        [SerializeField] private WarriorView _prefab;
        [SerializeField] private List<SpriteRenderer> _lines;
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
            
            foreach (var line in _lines)
            {
                foreach (Transform child in line.transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
        
        public void CreateNewWarrior(IWarrior warrior)
        {
            var newWarrior = Instantiate(_prefab);
            
            newWarrior.Init(warrior);
            _warriors.Add(newWarrior);
        }

        public void UpdateWarriors(IReadOnlyList<IWarrior> warriors)
        {
            _warriors.ForEach(warrior => warrior.UpdateWarrior());
        }

        public void HitLine(int index)
        {
            _lines[index].DOColor(Color.red, 0.2f).SetLoops(2, LoopType.Yoyo);
        }
        
        public void SpawnWarrior(IWarrior warrior)
        {
            var view = _warriors.FirstOrDefault(x => x.Warrior == warrior);

            if (view == null)
            {
                Debug.LogWarning("view for spawn not found");
                return;
            }
            
            SetWarriorOnLine(view, warrior.LineIndex);
            view.gameObject.SetActive(true);
        }

        public void UpdateBossHp(IdleNumber currentHp, IdleNumber maxHp)
        {
            _bossView.UpdateBossHp(currentHp, maxHp);
        }

        private void SetWarriorOnLine(WarriorView warrior, int lineIndex)
        {
            warrior.transform.SetParent(_lines[lineIndex].transform);
            var randomPositionY = Random.Range(-0.5f, 0.5f);
            warrior.transform.localPosition = new Vector2(randomPositionY, -0.5f);
        }
    }
}