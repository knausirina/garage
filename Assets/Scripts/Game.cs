using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game: MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GatesController _gatesController;
        [SerializeField] private InventoryController _inventoryController;

        private void Awake()
        {
            _playerController.TouchGagesAction += OnTouchGagesAction;
            _playerController.PickObjectAction += OnPickObjectAction;

            _inventoryController.AddToInventoryAction += OnAddToInventoryAction;
        }

        private void OnAddToInventoryAction(InventoryObject inventoryObject)
        {
            _playerController.AddInventoryObject(inventoryObject);
        }

        private void OnTouchGagesAction()
        {
            _gatesController.OpenGates();
        }

        private void OnPickObjectAction()
        {
            Debug.Log("xxx OnPickObjectAction");

            _inventoryController.Pick();
        }

        private void OnDestroy()
        {
            _playerController.TouchGagesAction -= OnTouchGagesAction;
            _playerController.PickObjectAction -= OnPickObjectAction;
            _inventoryController.AddToInventoryAction -= OnAddToInventoryAction;
        }
    }
}