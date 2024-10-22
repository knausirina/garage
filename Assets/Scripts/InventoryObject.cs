using System.Collections;
using UnityEngine;

public class InventoryObject : MonoBehaviour
{
    private bool _isPicked = false;
    public bool IsPicked => _isPicked;

    public void TogglePick(bool isPicked)
    {
        _isPicked = isPicked;
    }
}