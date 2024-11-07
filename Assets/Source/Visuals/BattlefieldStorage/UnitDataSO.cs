using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Serialization.Data;
using Source.Visuals.MemoryStorage;
using UnityEngine;

namespace Source.Visuals.BattlefieldStorage
{
    [CreateAssetMenu(fileName = "UnitName", menuName = "Game/Unit")]
    public class UnitDataSO : MemoryDataSO
    {
        public override Sprite MemoryBackgroundIcon => memoryBackgroundIcon;
        public override Sprite MemoryForegroundIcon => Sprite;
        public override string MemoryName => UnitName;

        [SerializeField] private Sprite memoryBackgroundIcon;
        
        public Sprite Sprite;
        public string UnitName;
        public string Abbreviation;
        public int BaseHealth = 3;
        public int BasePower = 1;
        public bool CanSwitchPlaces = true;
        public bool CanEngageCombat = true;

        public Unit CreateDefault(int ownerId, string definition)
        {
            return new Unit()
            {
                Definition = definition,
                OwnerId = ownerId,
                Health = BaseHealth,
                Power = BasePower,
                CanSwitchPlaces = CanSwitchPlaces,
                CanEngageCombat = CanEngageCombat
            };
        }

        public Unit CreateInstance(UnitData unitData)
        {
            return new Unit()
            {
                OwnerId = unitData.OwnerId,
                Definition = unitData.Definition,
                Health = unitData.Health,
                Power = unitData.Power,
                CanSwitchPlaces = CanSwitchPlaces,
                CanEngageCombat = CanEngageCombat
            };
        }
        
        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            return new UnitLineItem()
            {
                OwnerId = memoryData.OwnerId,
                Definition = memoryData.Definition,
                CurrentProgress = memoryData.Progress,
                MaxProgress = -1,
            };
        }
    }
}
