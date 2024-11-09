using Source.Interactions;
using Source.Logic.State.LineItems;
using Source.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visuals.MemoryStorage
{
    public class LineGemItemVisual : StandardInteractableVisual
    {
        [Header("Dependencies")]
        [SerializeField] private Image progressImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image foregroundImage;

        public LineStorage<MemoryItem> TrackedLineStorage => trackedLineStorage;

        public MemoryItem TrackedItem => trackedItem;
        public int TrackedSlot => trackedSlot;
        public bool IsTransferring => trackedTransferProgressPercent < 1;
        
        // TODO: Reevaluate if each item needs all info Storage gives
        private GameResources gameResources;
        private LineStorage<MemoryItem> trackedLineStorage;
        private MemoryItem trackedItem;
        private int trackedSlot;
        private float trackedTransferProgressPercent = 1;
        private MemoryDataSO memoryDataSO;

        private MemoryDataSO emptyGemSO;

        private void Start()
        {
            if (gameResources != null)
            {
                var emptySprite = GameResources.BuildDefinitionPath(GameResourceConstants.PROGRAMS_PATH, GameResourceConstants.EMPTY_MEMORY_DATA_DEFINITION);
                gameResources.TryLoadAsset(this, emptySprite, out emptyGemSO);
            }
        }

        public void SetGameResources(GameResources resources)
        {
            gameResources = resources;
        }

        public void SetStorage(LineStorage<MemoryItem> lineStorage)
        {
            trackedLineStorage = lineStorage;
        }
        
        public void SetDataItem(in MemoryItem item)
        {
            if (item != null && item != trackedItem)
            {
                if (item.Definition != null)
                {
                    gameResources.TryLoadAsset(this, item.Definition, out memoryDataSO);
                }
            }
            
            trackedItem = item;
        }
        
        public void SetSlot(int slot)
        { 
            trackedSlot = slot;
        }
        public void SetTransferProgressPercent(float transferProgressPercent)
        {
            trackedTransferProgressPercent = transferProgressPercent;
        }

        private void Update()
        {
            SetVisualToItem(trackedItem);
            
            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    backgroundImage.color = Color.white; 
                    break;
                case InteractVisualState.Hovered:
                    backgroundImage.color = Color.yellow;
                    break;
                case InteractVisualState.Selected:
                    backgroundImage.color = Color.blue;
                    break;
            }
        }
        
        private void SetVisualToItem(MemoryItem item)
        {
            backgroundImage.gameObject.SetActive(false);

            if (gameResources == null) return;
            
            if (item != null && memoryDataSO != null)
            {
                backgroundImage.sprite = memoryDataSO.MemoryBackgroundIcon;
                backgroundImage.gameObject.SetActive(true);
                backgroundImage.fillAmount = trackedTransferProgressPercent;

                if (memoryDataSO.MemoryForegroundIcon != null)
                {
                    foregroundImage.sprite = memoryDataSO.MemoryForegroundIcon;
                    foregroundImage.gameObject.SetActive(true);
                }
                foregroundImage.fillAmount = trackedTransferProgressPercent;

                if (trackedTransferProgressPercent >= 1)
                {
                    progressImage.fillAmount = ((float) item.CurrentRunProgress) / item.MaxRunProgress;
                    progressImage.gameObject.SetActive(true);
                }
            } else if(emptyGemSO != null)
            {
                backgroundImage.sprite = emptyGemSO.MemoryBackgroundIcon;
                backgroundImage.gameObject.SetActive(true);
                foregroundImage.gameObject.SetActive(false);
                progressImage.gameObject.SetActive(false);
            }
        }
    }
}
