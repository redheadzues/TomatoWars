using System;
using Source.Code.ModelsAndServices.Farm;
using Source.Code.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.Farm
{
    public class FarmCharacterView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _levelText;
        [SerializeField] private Text _costText;
        [SerializeField] private Text _incomeText;
        [SerializeField] private Button _upgradeButton;

        private IFarmCharacter _character;

        public CharacterTypeId TypeId => _character.TypeId;
        public event Action<CharacterTypeId> UpgradeRequested; 

        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
        }

        public void Init(IFarmCharacter character)
        {
            _character = character;
            _icon.sprite = _character.Icon;
            UpdateTextData();
        }

        public void UpdateTextData()
        {
            _levelText.text = _character.Level.ToString();
            _costText.text = _character.Cost.ToString();
            _incomeText.text = _character.IncomePerSecond.ToString();
        }
        
        private void OnUpgradeButtonClicked() => 
            UpgradeRequested?.Invoke(_character.TypeId);
    }
}