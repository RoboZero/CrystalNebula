namespace Source.Logic.Data
{
    public class BattlefieldData
    {
        public int Length;
        public Item[] Items;

        public class Item
        {
            public int Location;
            public BuildingData Building;
            public UnitData Unit;
        }
    }
}