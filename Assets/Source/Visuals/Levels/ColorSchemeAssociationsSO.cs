using System;
using System.Collections.Generic;
using System.Drawing;
using Source.Utility;
using Source.Visuals.Levels;
using UnityEngine;

namespace Source.Visuals
{
    [CreateAssetMenu(fileName = "ColorSchemeAssociation", menuName = "Game/Levels/ColorSchemeAssociation")]
    public class ColorSchemeAssociationsSO : DescriptionBaseSO
    {
        [SerializeField] private ColorSchemeSO baseColorScheme;
        [SerializeField] private List<PlayerToColorScheme> playerToColorSchemes;

        private Dictionary<int, ColorSchemeSO> playerIdToColorScheme;

        public ColorSchemeSO GetColorScheme(int playerId)
        {
            if (playerIdToColorScheme == null)
            {
                playerIdToColorScheme = new Dictionary<int, ColorSchemeSO>();
                foreach (var pair in playerToColorSchemes)
                {
                    playerIdToColorScheme.Add(pair.PlayerId, pair.colorSchemeSO);
                }
            }

            return playerIdToColorScheme.TryGetValue(playerId, out var colorSchemeSO) ? colorSchemeSO : baseColorScheme;
        }
        
        [Serializable]
        public struct PlayerToColorScheme
        {
            public int PlayerId;
            public ColorSchemeSO colorSchemeSO;
        }
    }
}
