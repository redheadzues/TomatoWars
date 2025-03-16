using System;

namespace Source.Code.Services
{
    public class SaveLoadService : Service
    {
        public void Load(Action onLoadComplete = null)
        {
            onLoadComplete?.Invoke();
        }
        public void Save(){}

    }
}