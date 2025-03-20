using System.Collections.Generic;
using Source.Code.ModelsAndServices;
using Source.Code.ModelsAndServices.BattleField;
using Source.Code.Warriors;

namespace Source.Code.BattleField.Buff
{
    public class BuffSystem
    {
        private readonly BattleFieldModel _model;
        private readonly ICoroutineRunner _runner;
        private readonly Dictionary<Warrior, HashSet<IBuff>> _permanentBuff;

        public BuffSystem(BattleFieldModel model, ICoroutineRunner runner)
        {
            _model = model;
            _runner = runner;
        }

        private void InitWarriorsWithBooster()
        {
            
        }
        
        
        
        
    }

    public class CharacteristicsBuff : IBuff
    {
        private Warrior _warrior;
        private ITimeService _timeService;
        
        private int _damagePerSecond;
        private float _normalizedSpeed;
        private float _criticalChance;
        private float _criticalPower;
        private float _duration;
        private float _startTime;

        public bool IsExpired => _startTime + _duration < _timeService.TimeFromStart;

        public void ApplyBuff(Warrior warrior)
        {
            /*_warrior = warrior;
            _startTime = _timeService.TimeFromStart;
            _warrior.BaseDamagePerSecond += _damagePerSecond;
            _warrior.BaseNormalizedSpeed += _normalizedSpeed;
            _warrior.BaseCriticalChance += _criticalChance;
            _warrior.BaseCriticalPower += _criticalPower;*/
        }

        public void RemoveBuff()
        {
            /*_warrior.BaseDamagePerSecond -= _damagePerSecond;
            _warrior.BaseNormalizedSpeed -= _normalizedSpeed;
            _warrior.BaseCriticalChance -= _criticalChance;
            _warrior.BaseCriticalPower -= _criticalPower;*/
        }
    }

    public interface IBuff
    {
        bool IsExpired { get; }
        void ApplyBuff(Warrior warrior);
        void RemoveBuff();
    }
}