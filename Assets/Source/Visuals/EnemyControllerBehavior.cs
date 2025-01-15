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

        [SerializeField] private Image enemyUnitImage;
        [SerializeField] private TextMeshProUGUI enemyArrivesText;
        [SerializeField] private TextMeshProUGUI enemyMovesText;
        

        [Header("Settings")]
        [SerializeField] private int ownerId;
        [SerializeField] private Gradient moveTimeGradient;
        
        private EnemyWaveController enemyWaveController;
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
                enemyWaveController = new EnemyWaveController(
                    ownerId,
                    level.EnemyWavesSO.EnemyWaves,
                    gameResources,
                    eventTrackerBehavior.EventTracker,
                    gameStateLoader.GameState,
                    createdCommandProgram
                );

                previousLevel = level;
            }

            if (enemyWaveController != null)
            {
                enemyWaveController.Tick(Time.deltaTime);
                if (enemyWaveController.CreatedUnitSO != null)
                {
                    enemyUnitImage.gameObject.SetActive(true);
                    enemyUnitImage.sprite = enemyWaveController.CreatedUnitSO.Sprite;
                }
                else
                {
                    enemyUnitImage.gameObject.SetActive(false);
                }

                enemyArrivesText.text = enemyWaveController.ArriveDelayTime.ToString("F1");
                enemyMovesText.text = enemyWaveController.MoveDelayTime.ToString("F1");

                var movePercentage = enemyWaveController.MoveDelayTime / enemyWaveController.MaxMoveDelayTime;
                enemyMovesText.color = moveTimeGradient.Evaluate(movePercentage);
            }
        }
    }
}