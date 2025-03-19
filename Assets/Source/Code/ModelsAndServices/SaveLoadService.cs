using System;

namespace Source.Code.ModelsAndServices
{
    public interface ISaveLoadService : IService
    {
        bool IsLoaded { get; }
        event Action LoadCompleted;
        void LoadOrInit();
        void Save();
    }
    
    public class SaveLoadService : ISaveLoadService
    {
        public bool IsLoaded { get; private set; }
        public event Action LoadCompleted;

        public void LoadOrInit()
        {
            IsLoaded = true;
            LoadCompleted?.Invoke();
        }
        public void Save(){}

    }
}