using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class InventoryController : MonoBehaviour
    {
        public Action<InventoryObject> AddToInventoryAction;

        private List<InventoryObject> _inventoryObjects;
        private List<InventoryObject> _pickedObjects = new List<InventoryObject>();

        private void Awake()
        {
            _inventoryObjects = GetComponentsInChildren<InventoryObject>().ToList();

            Debug.Log("xxx InventoryController " + _inventoryObjects.Count);
        }

        public void Pick()
        {
            var element =_inventoryObjects.First();
            _inventoryObjects.Remove(element);

            _pickedObjects.Add(element);

            AddToInventoryAction?.Invoke(element);
        }
    }
} 