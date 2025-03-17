using Source.Code.ModelsAndServices;
using UnityEngine;

namespace Source.Code
{
    public abstract class GameWindow : MonoBehaviour
    {
        protected ServiceProvider Provider;

        private bool _isProviderSet;

        public void SetProvider(ServiceProvider provider)
        {
            if (!_isProviderSet)
            {
                Provider = provider;
                _isProviderSet = true;
            
                OnProviderInitialized();
            }
        }

        protected abstract void OnProviderInitialized();
    }
}