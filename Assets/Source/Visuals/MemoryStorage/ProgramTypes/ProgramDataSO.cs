using UnityEngine;

namespace Source.Visuals.MemoryStorage.ProgramTypes
{ 
    public abstract class ProgramDataSO : MemoryDataSO
    {
        public override Sprite MemoryBackgroundIcon => memoryBackgroundIcon;
        public override Sprite MemoryForegroundIcon => memoryForegroundIcon;
        public override string MemoryName => memoryName;
        public override string MemoryDescription => memoryDescription;
        
        [SerializeField] private Sprite memoryBackgroundIcon;
        [SerializeField] private Sprite memoryForegroundIcon;
        [SerializeField] private string memoryName;
        [SerializeField] private string memoryDescription;
        
        public int MaxProgress;
        public float DataSize;
    }
}