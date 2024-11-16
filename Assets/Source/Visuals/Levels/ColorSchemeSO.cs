using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Levels
{
    [CreateAssetMenu(fileName = "ColorScheme", menuName = "Game/Levels/ColorScheme")]
    public class ColorSchemeSO : DescriptionBaseSO
    {
        public Color NoInteractionColor = Color.white;
        public Color HoveredColor = Color.yellow;
        public Color InteractedColor = Color.blue;

        
        public Color MemoryProgressColor;
        
        public Color DeploymentZonePlatformColor;
        public Color DeploymentZoneUnitPlatformColor;
        public Color DeploymentZoneBuildingPlatformColor;
    }
}
