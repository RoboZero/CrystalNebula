using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Serialization.Data;
using Source.Visuals.MemoryStorage;
using UnityEngine;

namespace Source.Visuals.BattlefieldStorage
{
    [CreateAssetMenu(fileName = "UnitName", menuName = "Game/Unit")]
    public class UnitMemoryDataSO : MemoryDataSO
    {
        public override Sprite MemoryBackgroundIcon => memoryBackgroundIcon;
        public override Sprite MemoryForegroundIcon => Sprite;
        public override string MemoryName => UnitName;

        [SerializeField] private Sprite memoryBackgroundIcon;
        
        public Sprite Sprite;
        public string UnitName;
        public string Abbreviation;
        public int MaxProgress;
        public int BaseHealth;
        public int BasePower;
        public float DataSize;
        public bool CanSwitchPlaces = true;
        public bool CanEngageCombat = true;

        public UnitMemory CreateDefault(int ownerId, string definition)
        {
            return new UnitMemory
            {
                Definition = definition,
                OwnerId = ownerId,
                Health = BaseHealth,
                Power = BasePower,
                DataSize = DataSize,
                CanSwitchPlaces = CanSwitchPlaces,
                CanEngageCombat = CanEngageCombat
            };
        }

        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            Debug.Log($"Creating instance of Memory data {memoryData}");

            return new UnitMemory()
            {
                OwnerId = memoryData.OwnerId,
                Definition = memoryData.Definition,
                Health = memoryData.Health ?? BaseHealth,
                Power = memoryData.Power ?? BasePower,
                DataSize = DataSize,
                CanSwitchPlaces = CanSwitchPlaces,
                CanEngageCombat = CanEngageCombat,
                CurrentRunProgress = memoryData.Progress,
                MaxRunProgress = MaxProgress,
            };
        }
    }
}
