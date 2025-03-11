using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Source.Code.StaticData;
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
            ClearLines();
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
            var view = _warriors.FirstOrDefault(x => x.Id == warrior.Id);

            if (view == null)
            {
                Debug.LogWarning("view for spawn not found");
                return;
            }
            
            SetWarriorOnLine(view, warrior.LineIndex);
            view.gameObject.SetActive(true);
        }

        public void UpdateBossHp(int currentHp, int maxHp)
        {
            _bossView.UpdateBossHp(currentHp, maxHp);
        }

        private void SetWarriorOnLine(WarriorView warrior, int lineIndex)
        {
            warrior.transform.SetParent(_lines[lineIndex].transform);
            var randomPositionY = Random.Range(-0.5f, 0.5f);
            warrior.transform.localPosition = new Vector2(randomPositionY, -0.5f);
        }
        
        private void ClearLines()
        {
            foreach (var line in _lines)
            {
                foreach (Transform child in line.transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}