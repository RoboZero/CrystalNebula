using UnityEngine;

namespace Source.Visuals.MemoryStorage.ProgramTypes
{ 
    public abstract class ProgramDataSO : MemoryDataSO
    {
        public override Sprite MemoryBackgroundIcon => memoryBackgroundIcon;
        public override Sprite MemoryForegroundIcon => memoryForegroundIcon;
        public override string MemoryName => memoryName;
        
        [SerializeField] private Sprite memoryBackgroundIcon;
        [SerializeField] private Sprite memoryForegroundIcon;
        [SerializeField] private string memoryName;
        
        public int MaxProgress;
        public float DataSize;
    }
}