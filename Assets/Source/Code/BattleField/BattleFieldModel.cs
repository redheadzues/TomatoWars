using System.Collections.Generic;
using Source.Code.StaticData;

namespace Source.Code.BattleField
{
    public interface IReadOnlyBattleFieldModel
    {
        IReadOnlyList<WarriorTypeId> SelectedWarriors { get; }
        public int BossHp  { get; }
        public int BossCurrentHp { get; }
    }
    
    
    public class BattleFieldModel : IReadOnlyBattleFieldModel
    {
        public List<WarriorTypeId> SelectedWarriors { get; set; }
        IReadOnlyList<WarriorTypeId> IReadOnlyBattleFieldModel.SelectedWarriors => SelectedWarriors;

        public List<Warrior> Warriors;
        
        public int BossHp { get; set; }
        public int BossCurrentHp { get; set; }
        public int BossDamagePerSecond { get; set; }
        
        
        

    }
}