using System;

namespace Source.Serialization.Data
{
    [Serializable]
    public class MemoryData
    {
        public int OwnerId;
        public string Definition;
        public int Progress;
        
        // TODO: JSON extract specific types beyond null.
        public int? Health;
        public int? Power;

        public override string ToString()
        {
            return $"{Definition}: (OId: {OwnerId}, Prog: {Progress}, H: {Health}, P: {Power})";
        }
    }
}