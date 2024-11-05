using System;
using Source.Logic;
using Source.Logic.State;

namespace Source.Serialization.Data
{
    [Serializable]
    public class BuildingData : BattlefieldResource
    { 
        public int Health;
        public int Power;
    }
}
