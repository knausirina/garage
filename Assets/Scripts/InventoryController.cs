using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class InventoryController : MonoBehaviour
    {
        public Action<InventoryObject> AddedToInventoryAction;

        private List<InventoryObject> _inventoryObjects;
        private List<InventoryObject> _pickedObjects = new List<InventoryObject>();

        private const int Capacity = 1;

        private void Awake()
        {
            _inventoryObjects = GetComponentsInChildren<InventoryObject>().ToList();
        }

        public void Pick()
        {
            if (IsFull())
            {
                return;
            }

            var element =_inventoryObjects.First();
            _inventoryObjects.Remove(element);

            _pickedObjects.Add(element);

            AddedToInventoryAction?.Invoke(element);
        }

        public InventoryObject RemovePicketObject()
        {
            if (_pickedObjects.Count == 0)
            {
                return null;
            }

            var element = _pickedObjects.First();
            _pickedObjects.Remove(element);
            return element;
        }

        public bool IsFull()
        {
            return _pickedObjects.Count >= Capacity;
        }
    }
} 