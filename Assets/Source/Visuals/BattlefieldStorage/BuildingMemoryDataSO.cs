using System.Globalization;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Serialization.Data;
using Source.Visuals.MemoryStorage;
using Source.Visuals.Tooltip;
using UnityEngine;

namespace Source.Visuals.BattlefieldStorage
{
    [CreateAssetMenu(fileName = "BuildingName", menuName = "Game/Building")]
    public class BuildingMemoryDataSO : MemoryDataSO
    {
        public override Sprite TooltipIcon => tooltipIcon;
        public override Sprite MemoryBackgroundIcon => memoryBackgroundIcon;
        public override Sprite MemoryForegroundIcon => Sprite;
        public override string MemoryName => BuildingName;
        public override string MemoryDescription => BuildingDescription;

        [SerializeField] private Sprite tooltipIcon;
        [SerializeField] private Sprite memoryBackgroundIcon;

        public Sprite Sprite;
        public string BuildingName;
        public string Abbreviation;
        [TextArea] public string BuildingDescription;
        public int MaxProgress;
        public int BaseHealth;
        public int BasePower;
        public float DataSize;

        public BuildingMemory CreateDefault(int ownerId, string definition, int? health = null, int? power = null)
        {
            return new BuildingMemory()
            {
                OwnerId = ownerId,
                Definition = definition,
                Health = health ?? BaseHealth,
                Power = power ?? BasePower,
                DataSize = DataSize
            };
        }

        public override MemoryItem CreateDefaultInstance(int ownerId, string definition)
        {
            return new BuildingMemory()
            {
                OwnerId = ownerId,
                Definition = definition,
                Health = BaseHealth,
                Power = BasePower,
                DataSize = DataSize,
                CurrentRunProgress = 0,
                MaxRunProgress = MaxProgress,
            };
        }

        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            var instance = (BuildingMemory) CreateDefaultInstance(memoryData.OwnerId, memoryData.Definition);
            instance.Health = memoryData.Health ?? BaseHealth;
            instance.Power = memoryData.Power ?? BasePower;
            instance.CurrentRunProgress = memoryData.Progress;
            return instance;
        }

        public override void FillTooltipContent(MemoryItem memoryItem, TooltipContent tooltipContent)
        {
            if (memoryItem is BuildingMemory buildingMemory)
            {
                tooltipContent.Icon = TooltipIcon;
                tooltipContent.Header = MemoryName;
                tooltipContent.Description = MemoryDescription;
                tooltipContent.Stats.Clear();
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Health", Value = $"{buildingMemory.Health}/{BaseHealth.ToString()}"});
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Power", Value = $"{buildingMemory.Power.ToString()}/{BasePower.ToString()}"});
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Progress", Value = $"{buildingMemory.CurrentRunProgress}/{buildingMemory.MaxRunProgress}" });
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Data Size", Value = $"{buildingMemory.DataSize.ToString(CultureInfo.InvariantCulture)}/{DataSize}"});
                return;
            }
            
            tooltipContent.Clear();
            tooltipContent.Header = "???";
        }
    }
}
