using System;
using System.Collections.Generic;
using System.Drawing;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals
{
    [CreateAssetMenu(fileName = "Level", menuName = "Game/Levels/Level")]
    public class LevelDataSO : DescriptionBaseSO
    {
        public ColorSchemeAssociationsSO ColorSchemeAssociationsSO;
    }
}
