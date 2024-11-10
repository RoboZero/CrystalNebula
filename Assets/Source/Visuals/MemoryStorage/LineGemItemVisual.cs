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
        [SerializeField] private Image emptyGemImage;
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
        private MemoryItem trackedTransferItem;
        private int trackedSlot;
        private float trackedTransferProgressPercent = 1;
        private MemoryDataSO memoryDataSO;

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
            SetVisualToItem(trackedItem, memoryDataSO);

            switch (CurrentVisualState)
            {
                case InteractVisualState.None:
                    emptyGemImage.color = Color.white; 
                    backgroundImage.color = Color.white; 
                    break;
                case InteractVisualState.Hovered:
                    emptyGemImage.color = Color.yellow;
                    backgroundImage.color = Color.yellow;
                    break;
                case InteractVisualState.Selected:
                    emptyGemImage.color = Color.blue;
                    backgroundImage.color = Color.blue;
                    break;
            }
        }
        
        private void SetVisualToItem(MemoryItem item, MemoryDataSO memoryData)
        {
            if (gameResources == null) return;

            if (item != null && memoryData != null)
            {
                backgroundImage.sprite = memoryData.MemoryBackgroundIcon;
                backgroundImage.gameObject.SetActive(true);
                backgroundImage.fillAmount = trackedTransferProgressPercent;

                if (memoryData.MemoryForegroundIcon != null)
                {
                    foregroundImage.sprite = memoryData.MemoryForegroundIcon;
                    foregroundImage.gameObject.SetActive(true);
                }
                foregroundImage.fillAmount = trackedTransferProgressPercent;

                progressImage.fillAmount = ((float) item.CurrentRunProgress) / item.MaxRunProgress;
                progressImage.gameObject.SetActive(true);
            } 
            else
            {
                backgroundImage.gameObject.SetActive(false);
                foregroundImage.gameObject.SetActive(false);
                progressImage.gameObject.SetActive(false);
            }
        }
    }
}
