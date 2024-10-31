using System;
using Source.Logic;

namespace Source.Serialization.Data
{
    [Serializable]
    public class UnitData : BattlefieldResource
    {
        public int Health;
        public int Power;
    }
}
