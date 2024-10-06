using Source.Logic.State;

namespace Source.Logic.Data
{
    public class MemoryStorageData
    {
        public Item[] Items;
        
        public class Item
        {
            public ProgramData Program;
        }
    }
}