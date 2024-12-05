using System.Globalization;
using Source.Logic.State;
using Source.Logic.State.LineItems.Programs;
using Source.Serialization;
using Source.Visuals.Levels;
using Source.Visuals.MemoryStorage.ProgramTypes;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Source.Visuals
{
    public class EnemyControllerBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private CommandProgramDataSO CommandProgramDataSO;
        [SerializeField] private GameResources gameResources;
        [SerializeField] private GameStateLoader gameStateLoader;
        [SerializeField] private EventTrackerBehavior eventTrackerBehavior;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI timerText;
        

        [Header("Settings")]
        [SerializeField] private int ownerId;
        
        private EnemyController enemyController;
        private CommandProgram createdCommandProgram;

        private LevelDataSO previousLevel;

        private void Start()
        {
            gameResources.TryLoadDefinition(this, CommandProgramDataSO, out var definition);
            createdCommandProgram = (CommandProgram) CommandProgramDataSO.CreateDefaultInstance(ownerId, definition);
        }

        private void Update()
        {
            if (gameResources.TryLoadAsset(this, gameStateLoader.GameState.Level.Definition, out LevelDataSO level)
                && level != previousLevel)
            {
                enemyController = new EnemyController(
                    ownerId,
                    level.EnemyWavesSO.EnemyWaves,
                    gameResources,
                    eventTrackerBehavior.EventTracker,
                    gameStateLoader.GameState,
                    createdCommandProgram
                );

                previousLevel = level;
            }

            if (enemyController != null)
            {
                enemyController.Tick(Time.deltaTime);
                if (enemyController.CreatedUnitSO != null)
                {
                    image.gameObject.SetActive(true);
                    image.sprite = enemyController.CreatedUnitSO.Sprite;
                }
                else
                {
                    image.gameObject.SetActive(false);
                }

                timerText.text = enemyController.DelayTime.ToString("F2");
            }
        }
    }
}