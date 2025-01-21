using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GatesController _gatesController;
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private Basket _basket;

    private void Awake()
    {
        _playerController.TouchGagesAction += OnTouchGagesAction;
        _playerController.PickObjectAction += OnPickObjectAction;
        _playerController.TouchBasketAction += OnTouchBasketAction;

        _inventoryController.AddedToInventoryAction += OnAddToInventoryAction;
    }

    private void OnAddToInventoryAction(InventoryObject inventoryObject)
    {
        _playerController.AddInventoryObject(inventoryObject);
    }

    private void OnTouchGagesAction()
    {
        _gatesController.OpenGates();
    }

    private void OnPickObjectAction(InventoryObject inventoryObject)
    {
        _inventoryController.Pick(inventoryObject);
    }

    private void OnTouchBasketAction()
    {
        var inventoryObject = _inventoryController.RemovePicketObject();
        if (inventoryObject == null)
        {
            return;
        }

        inventoryObject.transform.position = _basket.Point.position;
        inventoryObject.transform.parent = _basket.transform;
        var collider = inventoryObject.GetComponent<BoxCollider>();
        collider.enabled = true;
        inventoryObject.gameObject.AddComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        _playerController.TouchGagesAction -= OnTouchGagesAction;
        _playerController.PickObjectAction -= OnPickObjectAction;
        _inventoryController.AddedToInventoryAction -= OnAddToInventoryAction;
        _inventoryController.AddedToInventoryAction -= OnAddToInventoryAction;
    }
}