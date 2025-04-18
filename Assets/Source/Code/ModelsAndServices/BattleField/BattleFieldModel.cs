using System.Collections.Generic;
using Source.Code.BattleField;
using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using Source.Code.Warriors;
using UnityEngine;

namespace Source.Code.ModelsAndServices.BattleField
{
    public interface IReadOnlyBattleFieldModel
    {
        IReadOnlyList<CharacterTypeId> SelectedWarriors { get; }
        IReadOnlyList<IWarrior> ReadOnlyWarriors { get; }
        IdleNumber BossMaxHp { get; }
        IdleNumber BossCurrentHp { get; }
        Sprite BossSprite { get; }
    }


    public class BattleFieldModel : IReadOnlyBattleFieldModel
    {
        private int _bossCurrentHp;
        
        public List<CharacterTypeId> SelectedWarriors { get; set; } = new();
        IReadOnlyList<CharacterTypeId> IReadOnlyBattleFieldModel.SelectedWarriors => SelectedWarriors;

        public List<Warrior> Warriors = new();
        
        public IReadOnlyList<IWarrior> ReadOnlyWarriors => Warriors;

        public IdleNumber BossMaxHp { get; set; }

        public IdleNumber BossCurrentHp { get; set; }

        public Sprite BossSprite { get; set; }

        public int BossDamagePerSecond { get; set; }
        public float BossAttackWidth { get; set; }

    }
}