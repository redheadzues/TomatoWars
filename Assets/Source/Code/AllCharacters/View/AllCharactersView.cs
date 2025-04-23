using System;
using System.Collections.Generic;
using System.Linq;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.AllCharacters.View
{
    public class AllCharactersView : MonoBehaviour
    {
        [SerializeField] private CharacterView _prefab;
        [SerializeField] private Transform _container;

        private List<CharacterView> _views;
        
        public event Action<CharacterTypeId> UpgradeRequired;
        
        public void Init(List<IOwnedWarrior> allCharacters)
        {
            foreach (var character in allCharacters)
            {
                var view = Instantiate(_prefab, _container);
                
                view.Init(character);
                _views.Add(view);
            }
        }

        private void OnUpgradeRequired(CharacterTypeId typeId) => 
            UpgradeRequired?.Invoke(typeId);

        public void UpdateCharacterView(CharacterTypeId typeId)
        {
            var view = _views.FirstOrDefault(x => x.Character.TypeId == typeId);

            if (view == null)
            {
                Debug.Log($"Cant find view by character type {typeId}");
                return;
            }
            
            view.UpdateView();
        }
    }

    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _progressFillingImage;
        [SerializeField] private Text _levelText;
        [SerializeField] private Text _shardsCountText;
        [SerializeField] private Button _upgradeButton;
        
        private IOwnedWarrior _character;

        public IOwnedWarrior Character => _character;

        public event Action<CharacterTypeId> UpgradeRequired;

        private void OnEnable() => 
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);

        private void OnDisable() => 
            _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);

        public void Init(IOwnedWarrior character) => 
            _character = character;

        public void UpdateView()
        {
            
        }

        private void OnUpgradeButtonClicked() => 
            UpgradeRequired?.Invoke(_character.TypeId);
    }
}