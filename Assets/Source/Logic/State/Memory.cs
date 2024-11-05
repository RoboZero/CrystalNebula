using System;

namespace Source.Logic.State
{
    [Serializable]
    public class Memory
    {
        public int OwnerId;
        public string Definition;
        public int CurrentProgress;
        public int MaxProgress;
    }
}