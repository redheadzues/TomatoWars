using System;
using DG.Tweening;
using Source.Code.IdleNumbers;
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
        [SerializeField] private Image _incomeFilledImage;

        private IFarmCharacter _character;
        private Sequence _fillSequence;
        private Tween _fillTween;

        public CharacterTypeId TypeId => _character.TypeId;
        public event Action<CharacterTypeId> UpgradeRequested; 

        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            
            if(_character != null)
                StartFillIncome();
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
            _fillTween?.Kill();
            _fillSequence?.Kill();
        }

        public void Init(IFarmCharacter character)
        {
            _character = character;
            _icon.sprite = _character.Icon;
            _incomeFilledImage.fillAmount = 0;
            UpdateTextData();
            StartFillIncome();
        }

        public void UpdateTextData()
        {
            if (_character == null)
                return;
            
            _levelText.text = _character.Level.ToString();
            _costText.text = _character.Cost.ToString();
            _incomeText.text = _character.IncomePerSecond.ToString();
            
            if(_fillTween == null)
                StartFillIncome();
        }

        public void UpdateAvailabilityButton(IdleNumber value)
        {
            if (_character == null)
                return;
            
            _upgradeButton.interactable = value >= _character.Cost;
        }

        private void StartFillIncome()
        {
            if(_character.Level == 0)
                return;

            _incomeFilledImage.fillAmount = 
                (_character.IncomeTime - _character.RemainingTimeToIncome) / _character.IncomeTime;

            _fillSequence = DOTween.Sequence()
                .Append(_incomeFilledImage.DOFillAmount(1, _character.IncomeTime))
                .AppendCallback(() =>
                {
                    _incomeFilledImage.fillAmount = 0;
                    
                    _fillTween = _incomeFilledImage
                        .DOFillAmount(1, _character.IncomeTime)
                        .SetLoops(-1, LoopType.Restart);
                });
        }

        private void OnUpgradeButtonClicked() => 
            UpgradeRequested?.Invoke(_character.TypeId);
    }
}