using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.NavigationButtons
{
    public class NavigationButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameWindow _gameWindow;

        public GameWindow Window => _gameWindow;
        
        public event Action<GameWindow> WindowRequiredToOpen; 
        
        private void OnEnable() => 
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked()
        {
            WindowRequiredToOpen?.Invoke(_gameWindow);
        }
    }
}