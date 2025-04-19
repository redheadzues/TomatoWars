using System.Collections.Generic;
using UnityEngine;

namespace Source.Code.NavigationButtons
{
    public class NavigationController : MonoBehaviour
    {
        [SerializeField] private List<NavigationButton> _buttons;

        private GameWindow _currentWindow;
        
        private void OnEnable() => 
            _buttons.ForEach(x => x.WindowRequiredToOpen += OnRequiredOpenWindow);

        private void OnDisable() => 
            _buttons.ForEach(x => x.WindowRequiredToOpen -= OnRequiredOpenWindow);

        private void OnRequiredOpenWindow(GameWindow window)
        {
            if(_currentWindow == window)
                return;
            
            _buttons.ForEach(x => x.Window.gameObject.SetActive(false));
            
            window.gameObject.SetActive(true);

            _currentWindow = window;
        }
    }
}
