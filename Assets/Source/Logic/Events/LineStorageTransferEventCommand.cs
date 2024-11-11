using System;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.Logic.State.LineItems;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class LineStorageTransferEventCommand : EventCommand
    {
        // TODO: Separate real-time event specifics from animations for playback.
        public LineStorage<MemoryItem> FromStorage => fromStorage;
        public int FromSlot => fromSlot;
        public LineStorage<MemoryItem> ToStorage => toStorage;
        public int ToSlot => toSlot;
        public float TransferProgressPercent => transferProgressPercent;

        private LineStorage<MemoryItem> fromStorage;
        private int fromSlot;
        private LineStorage<MemoryItem> toStorage;
        private int toSlot;
        private TransferEventOverrides transferEventOverrides;

        private float transferProgressPercent;

        public LineStorageTransferEventCommand(
            EventTracker eventTracker,
            LineStorage<MemoryItem> fromStorage,
            int fromSlot,
            LineStorage<MemoryItem> toStorage,
            int toSlot,
            TransferEventOverrides transferEventOverrides
        ) : base(eventTracker)
        {
            this.fromStorage = fromStorage;
            this.fromSlot = fromSlot;
            this.toStorage = toStorage;
            this.toSlot = toSlot;
            this.transferEventOverrides = transferEventOverrides;
        }

        public override async UniTask<bool> Apply(CancellationToken cancellationToken)
        {
            AddLog($"{GetType().Name} Starting line storage transfer from slot {fromStorage}:{fromSlot} to slot {toStorage}:{toSlot}");
            var failurePrefix = $"Unable to transfer from {fromStorage}:{fromSlot} to {toStorage}:{toSlot}: ";

            if (!fromStorage.Items.InBounds(fromSlot))
            {
                AddLog(failurePrefix + $"from slot {fromSlot} is not in fromStorage bounds length {fromStorage.Items.Count}");
                return false;
            }
            
            if (!toStorage.Items.InBounds(toSlot))
            {
                AddLog(failurePrefix + $"to slot {toSlot} is not in toStorage bounds length {toStorage.Items.Count}");
                return false;
            }

            if (transferEventOverrides != null && !transferEventOverrides.CanSwitch && toStorage.Items[toSlot] != null)
            {
                AddLog(failurePrefix + $"cannot switch and to slot {toSlot} has item in it {toStorage.Items[toSlot]}");
                return false;
            }

            var fromMemory = fromStorage.Items[fromSlot];
            var toMemory = toStorage.Items[toSlot];
            
            AddLog($"Starting transfer of from memory {fromMemory} and to memory {toMemory}");
            if (CalculateTransferTime(
                    fromMemory?.DataSize ?? 0, 
                    fromStorage.DataPerSecondTransfer,
                    toMemory?.DataSize ?? 0, 
                    toStorage.DataPerSecondTransfer,
                    out var transferTime))
            {
                await DOVirtual.Float(0, 1, transferTime, (x) => transferProgressPercent = x).ToUniTask(cancellationToken: cancellationToken);
            }
            
            // TODO: Check if canceled, if was undo swap. 
            (toStorage.Items[toSlot], fromStorage.Items[fromSlot]) = (fromStorage.Items[fromSlot], toStorage.Items[toSlot]);

            AddLog($"Successfully transferred slot {fromSlot} to slot {toSlot}");
            
            return true;
        }

        private bool CalculateTransferTime(float memoryDataSizeA, float dataTransferRateA, float memoryDataSizeB, float dataTransferRateB, out float transferTimeSeconds)
        {
            var minDataTransferRate = Mathf.Min(dataTransferRateA, dataTransferRateB);
            var maxDataSize = Mathf.Max(memoryDataSizeA, memoryDataSizeB);

            if (minDataTransferRate <= 0)
            {
                // TODO: Should fail if no data will ever be transferred, not complete instantly.
                AddLog($"Min Transfer Rate {minDataTransferRate} would never finish. Instantly transferring");
                transferTimeSeconds = 0;
                return false;
            }

            if (maxDataSize == 0)
            {
                AddLog($"Max Data Size {maxDataSize} is 0. Instantly transferring");
                transferTimeSeconds = 0;
                return false;
            }

            transferTimeSeconds = maxDataSize / minDataTransferRate;
            return true;
        }
    }
}