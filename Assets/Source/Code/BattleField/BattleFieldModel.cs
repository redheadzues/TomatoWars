using System;
using System.Collections.Generic;
using Source.Code.StaticData;

namespace Source.Code.BattleField
{
    public interface IReadOnlyBattleFieldModel
    {
        IReadOnlyList<WarriorTypeId> SelectedWarriors { get; }
        IReadOnlyList<IWarrior> ReadOnlyWarriors { get; }
        public int BossMaxHp { get; }
        public int BossCurrentHp { get; }

        event Action<IWarrior> OnWarriorAdded;
        event Action<int> OnBossHitLine;
        event Action<IWarrior> OnWarriorSpawned;
        event Action<int, int> OnBossGetDamage;
    }


    public class BattleFieldModel : IReadOnlyBattleFieldModel
    {
        private int _bossCurrentHp;
        
        public List<WarriorTypeId> SelectedWarriors { get; set; } = new();
        IReadOnlyList<WarriorTypeId> IReadOnlyBattleFieldModel.SelectedWarriors => SelectedWarriors;

        public List<Warrior> Warriors = new();

        public IReadOnlyList<IWarrior> ReadOnlyWarriors => Warriors;

        public int BossMaxHp { get; set; }

        public int BossCurrentHp
        {
            get => _bossCurrentHp;
            set
            {
                if (_bossCurrentHp != value)
                {
                    _bossCurrentHp = value;
                    OnBossGetDamage?.Invoke(BossCurrentHp, BossMaxHp);
                }
            }
        }

        public int BossDamagePerSecond { get; set; }

        public event Action<IWarrior> OnWarriorAdded;
        public event Action<int> OnBossHitLine;
        public event Action<IWarrior> OnWarriorSpawned;
        public event Action<int, int> OnBossGetDamage;

        public void AddWarrior(Warrior warrior)
        {
            Warriors.Add(warrior);
            OnWarriorAdded?.Invoke(warrior);
        }

        public void BossHitLine(int index) =>
            OnBossHitLine?.Invoke(index);

        public void WarriorSpawn(IWarrior warrior)
        {
            OnWarriorSpawned?.Invoke(warrior);
        }
    }
}