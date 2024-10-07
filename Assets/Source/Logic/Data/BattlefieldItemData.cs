using System;

namespace Source.Logic.Data
{
    [Serializable]
    public class BattlefieldItemData : DataItem
    {
        public int Location;
        public BuildingData Building;
        public UnitData Unit;
    }
}