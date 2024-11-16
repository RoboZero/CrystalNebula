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
        public override Vector2 BattlefieldScaleFactor => battlefieldScaleFactor;

        [SerializeField] private Sprite tooltipIcon;
        [SerializeField] private Sprite memoryBackgroundIcon;
        [SerializeField] private Vector2 battlefieldScaleFactor = Vector2.one;
        
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

        public override MemoryItem CreateDefaultInstance(int ownerId, string definition)
        {
            return new UnitMemory
            {
                Definition = definition,
                OwnerId = ownerId,
                Health = BaseHealth,
                Power = BasePower,
                DataSize = DataSize,
                CurrentRunProgress = 0,
                MaxRunProgress = MaxProgress,
                CanSwitchPlaces = CanSwitchPlaces,
                CanEngageCombat = CanEngageCombat
            };
        }

        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            var instance = (UnitMemory) CreateDefaultInstance(memoryData.OwnerId, memoryData.Definition);
            instance.Health = memoryData.Health ?? BaseHealth;
            instance.Power = memoryData.Power ?? BasePower;
            instance.CurrentRunProgress = memoryData.Progress;
            return instance;
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
