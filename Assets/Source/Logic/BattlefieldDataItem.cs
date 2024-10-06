using System;
using Source.Logic.Data;

namespace Source.Logic
{
    [Serializable]
    public class BattlefieldDataItem : DataItem
    {
        public BuildingData BuildingData;
        public UnitData UnitData;
    }
}
