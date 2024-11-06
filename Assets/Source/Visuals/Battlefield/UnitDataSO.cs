using Source.Logic;
using Source.Logic.State;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Serialization.Data;
using Source.Utility;
using Source.Visuals.LineStorage;
using UnityEngine;

namespace Source.Visuals.Battlefield
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
        
        public override Memory CreateMemoryInstance(MemoryData memoryData)
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
