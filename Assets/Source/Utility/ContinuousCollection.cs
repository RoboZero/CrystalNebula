using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Source.Utility
{
    public class ContinuousCollection<T> : IEnumerable<T>
    {
        private readonly HashSet<T> storedItems = new();
        private List<T> oldItems = new();
        [CanBeNull] private readonly Action<T> itemHasEnteredAction;
        [CanBeNull] private readonly Action<T> itemHasStayedAction;
        [CanBeNull] private readonly Action<T> itemHasLeftAction;
        [CanBeNull] private readonly Action<T> collectionClearedAction;

        public ContinuousCollection(Action<T> itemHasEnteredAction, 
            Action<T> itemHasStayedAction,
            Action<T> itemHasLeftAction, 
            Action<T> collectionClearedAction)
        {
            this.itemHasEnteredAction = itemHasEnteredAction;
            this.itemHasStayedAction = itemHasStayedAction;
            this.itemHasLeftAction = itemHasLeftAction;
            this.collectionClearedAction = collectionClearedAction;
        }
            
        public bool Tick(in List<T> newItems)
        {
            var isSame = true;
            
            if (itemHasStayedAction != null)
            {
                foreach (var interactable in storedItems.Union(newItems))
                {
                    itemHasStayedAction(interactable);
                }
            }
                
            if (itemHasEnteredAction != null)
            {
                foreach (var interactable in newItems.Except(oldItems))
                {
                    itemHasEnteredAction(interactable);
                    storedItems.Add(interactable);
                    isSame = false;
                }
            }

            if (itemHasLeftAction != null)
            {
                foreach (var interactable in oldItems.Except(newItems))
                {
                    itemHasLeftAction(interactable);
                    storedItems.Remove(interactable);
                    isSame = false;
                }
            }

            oldItems = newItems;
            return isSame;
        }

        public void Clear()
        {
            if (collectionClearedAction != null)
            {
                foreach (var interactable in storedItems)
                {
                    Debug.Log($"Reset {interactable}");
                    collectionClearedAction(interactable);
                }
            }
                
            storedItems.Clear();
            oldItems.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return storedItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
