using System;
using Source.Logic.Data;

namespace Source.Logic
{
    [Serializable]
    public class BattlefieldItem : DataItem
    {
        public Building Building;
        public Unit Unit;
    }
}