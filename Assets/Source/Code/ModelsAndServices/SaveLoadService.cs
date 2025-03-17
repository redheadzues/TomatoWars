using System;

namespace Source.Code.ModelsAndServices
{
    public interface ISaveLoadService : IService
    {
        void Load(Action onLoadComplete = null);
        void Save();
    }
    
    public class SaveLoadService : ISaveLoadService
    {
        public void Load(Action onLoadComplete = null)
        {
            onLoadComplete?.Invoke();
        }
        public void Save(){}

    }
}