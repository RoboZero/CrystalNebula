using System;
using Source.Logic;

namespace Source.Serialization.Data
{
    [Serializable]
    public class BuildingData : BattlefieldResource
    { 
        public int Health;
        public int Power;
    }
}
