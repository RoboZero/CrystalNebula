using Source.Logic.State;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Levels
{
    [CreateAssetMenu(fileName = "Enemy Wave", menuName = "Game/Levels/EnemyWave")]
    public class EnemyWavesSO : DescriptionBaseSO
    {
        public EnemyWaves EnemyWaves => enemyWaves;
        
        [SerializeField] private EnemyWaves enemyWaves;
    }
}