using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Programs;
using Source.Serialization.Data;
using Source.Visuals.Tooltip;
using UnityEngine;
using UnityEngine.UIElements;

namespace Source.Visuals.MemoryStorage.ProgramTypes
{ 
    public abstract class ProgramDataSO : MemoryDataSO
    {
        public override Sprite TooltipIcon => tooltipIcon;
        public override Sprite MemoryBackgroundIcon => memoryBackgroundIcon;
        public override Sprite MemoryForegroundIcon => memoryForegroundIcon;
        public override string MemoryName => memoryName;
        public override string MemoryDescription => memoryDescription;
        public override Vector2 BattlefieldScaleFactor => Vector2.one;

        [SerializeField] private Sprite tooltipIcon;
        [SerializeField] private Sprite memoryBackgroundIcon;
        [SerializeField] private Sprite memoryForegroundIcon;
        [SerializeField] private string memoryName;
        [TextArea] [SerializeField] private string memoryDescription;
        
        public int MaxProgress;
        public float DataSize;
        
        public override MemoryItem CreateMemoryInstance(MemoryData memoryData)
        {
            return new EmptyProgram()
            {
                OwnerId = memoryData.OwnerId,
                Definition = memoryData.Definition,
                CurrentRunProgress = memoryData.Progress,
                MaxRunProgress = MaxProgress,
                DataSize = DataSize
            };
        }

        public override void FillTooltipContent(MemoryItem memoryItem, TooltipContent tooltipContent)
        {
            if (memoryItem is ProgramMemory programMemory)
            {
                tooltipContent.Icon = TooltipIcon;
                tooltipContent.Header = MemoryName;
                tooltipContent.Description = MemoryDescription;
                tooltipContent.Stats.Clear();
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Progress", Value = $"{programMemory.CurrentRunProgress}/{programMemory.MaxRunProgress}" });
                tooltipContent.Stats.Add(new TooltipContent.Stat(){ Name = "Data Size", Value = $"{programMemory.DataSize}"});
                return;
            }

            tooltipContent.Clear();
            tooltipContent.Header = "???";
        }
    }
}