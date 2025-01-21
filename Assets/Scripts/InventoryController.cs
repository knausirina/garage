using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public void Pick(InventoryObject inventoryObject)
    {
        if (IsFull())
        {
            return;
        }

        var index = _inventoryObjects.IndexOf(inventoryObject);
        _inventoryObjects.Remove(inventoryObject);
        _pickedObjects.Add(inventoryObject);

        AddedToInventoryAction?.Invoke(inventoryObject);
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