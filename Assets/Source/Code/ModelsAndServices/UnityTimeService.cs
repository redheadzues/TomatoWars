using UnityEngine;

namespace Source.Code.ModelsAndServices
{
    public interface ITimeService
    {
        float TimeFromStart { get; }
    }
    
    public class UnityTimeService : ITimeService
    {
        public float TimeFromStart => Time.time;
    }
}