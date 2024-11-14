using System.Globalization;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Serialization.Data;
using Source.Visuals.MemoryStorage;
using Source.Visuals.Tooltip;
using UnityEngine;
using UnityEngine.UIElements;

namespace Source.Visuals.BattlefieldStorage
{
    [CreateAssetMenu(fileName = "UnitName", menuName = "Game/Unit")]
    public class UnitMemoryDataSO : MemoryDataSO
    {
        public override Sprite TooltipIcon => tooltipIcon;
        public override Sprite MemoryBackgroundIcon => memoryBackgroundIcon;
        public override Sprite MemoryForegroundIcon => Sprite;
        public override string MemoryName => UnitName;
        public override string MemoryDescription => UnitDescription;

        [SerializeField] private Sprite tooltipIcon;
        [SerializeField] private Sprite memoryBackgroundIcon;
        
        public Sprite Sprite;
        public string UnitName;
        public string Abbreviation;
        [TextArea] public string UnitDescription;
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

        public override void FillTooltipContent(MemoryItem memoryItem, TooltipContent tooltipContent)
        {
            if (memoryItem is UnitMemory unitMemory)
            {
                tooltipContent.Icon = TooltipIcon;
                tooltipContent.Header = MemoryName;
                tooltipContent.Description = MemoryDescription;
                tooltipContent.Stats.Clear();
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Health", Value = $"{unitMemory.Health}/{BaseHealth.ToString()}"});
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Power", Value = $"{unitMemory.Power.ToString()}/{BasePower.ToString()}"});
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Progress", Value = $"{unitMemory.CurrentRunProgress}/{unitMemory.MaxRunProgress}" });
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Data Size", Value = $"{unitMemory.DataSize.ToString(CultureInfo.InvariantCulture)}/{DataSize}"});
                return;
            }
            
            tooltipContent.Clear();
            tooltipContent.Header = "???";
        }
    }
}
