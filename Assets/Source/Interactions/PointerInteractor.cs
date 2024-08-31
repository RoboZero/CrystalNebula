using System.Collections.Generic;
using System.Linq;
using Source.Visuals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Interactions
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private LayerMask interactableMask;

        // Update is called once per frame
        void Update()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            var resultsInLayer = results.Where(r => ((1 << r.gameObject.layer) & interactableMask) != 0).ToList();
            foreach(var result in resultsInLayer)
            {
                Debug.Log(result.gameObject.name);
                if (result.gameObject.TryGetComponent<DataItem>(out var dataItem))
                {
                    dataItem.Select();
                }
            }
        }
    }
}
