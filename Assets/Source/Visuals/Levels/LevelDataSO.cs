using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Levels
{
    [CreateAssetMenu(fileName = "Level", menuName = "Game/Levels/Level")]
    public class LevelDataSO : DescriptionBaseSO
    {
        public ColorSchemeAssociationsSO ColorSchemeAssociationsSO;
    }
}
