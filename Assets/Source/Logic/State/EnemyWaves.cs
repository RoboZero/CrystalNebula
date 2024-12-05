using System;
using System.Collections.Generic;
using Source.Visuals.BattlefieldStorage;

namespace Source.Logic.State
{
    [Serializable]
    public class EnemyWaves
    {
        public List<Wave> Waves;

        [Serializable]
        public class Wave
        {
            public float commandDelay;
            public float timeUntilNextWave;
            public float delayBetweenSpawn;
            public List<UnitMemoryDataSO> enemyUnitInOrder;
        }
    }
}