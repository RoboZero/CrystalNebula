#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;

namespace Source.Logic.Data
{
    [Serializable]
    public class ItemStorage<T> : IEnumerable<ItemStorage<T>.ItemSlot> where T : DataItem
    {
        public int Size { get; private set; }
        public int Capacity => allSlots.Count;

        private List<ItemSlot> allSlots = new();

        public bool GetItemSlotReference(int i, out ItemSlot slot)
        {
            if (i >= 0 && i < Size)
            {
                slot = allSlots[i];
                return true;
            }

            slot = new ItemSlot();
            return false;
        }
        
        public void Resize(int newSize)
        {
            if (newSize == Size) return;
            
            if (newSize > Capacity)
            {
                SetCapacity(newSize);
            }

            Size = newSize;
            for (var i = 0; i < Capacity; i++)
            {
                allSlots[i] = allSlots[i] with { IsActive = i < Size };
            }
        }
        
        public void SetCapacity(int newCapacity)
        {
            if (newCapacity < 0) 
                newCapacity = 0;

            var itemDelta = newCapacity - allSlots.Count;
            switch (itemDelta)
            {
                case > 0:
                {
                    var lastCreationIndex = allSlots.Count + itemDelta;
                    for (var i = allSlots.Count; i < lastCreationIndex; i++)
                    {
                        var record = new ItemSlot()
                        {
                            LineNumber = i,
                            Item = default(T),
                            IsActive = false
                        };
                        allSlots.Add(record);
                    }

                    break;
                }
                case < 0:
                {
                    var lastRemovalIndex = Math.Max(allSlots.Count - 1 + itemDelta, 0);
                    for (var i = allSlots.Count - 1; i >= lastRemovalIndex; i--)
                    {
                        allSlots.RemoveAt(i);
                    }

                    break;
                }
            }
        }
        
        public IEnumerator<ItemSlot> GetEnumerator()
        {
            return allSlots.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        // TODO: Reconsider IsActive as a record item since it can be 'faked' once retrieved.
        [Serializable]
        public record ItemSlot
        {
            public int LineNumber;
            public T? Item;
            public bool IsActive;
        }
    }
}