using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Levels
{
    [CreateAssetMenu(fileName = "ColorScheme", menuName = "Game/Levels/ColorScheme")]
    public class ColorSchemeSO : DescriptionBaseSO
    {
        public Color MemoryProgressColor;
        
        public Color DeploymentZonePlatformColor;
        public Color DeploymentZoneUnitPlatformColor;
        public Color DeploymentZoneBuildingPlatformColor;
    }
}
