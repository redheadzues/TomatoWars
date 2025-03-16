using Source.Code.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.Grid.View
{
    public class WarriorView : MonoBehaviour
    {
        [SerializeField] private Image _warriorIcon;

        private IWarrior _warrior;
        
        public void Init(IWarrior warrior)
        {
            _warrior = warrior;
            _warriorIcon.sprite = warrior.Icon;
        }
        
    }
}