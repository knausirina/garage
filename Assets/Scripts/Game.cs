using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Assets.Scripts
{
    public class Game: MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GatesController _gatesController;
        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private Backet _backet;

        private void Awake()
        {
            _playerController.TouchGagesAction += OnTouchGagesAction;
            _playerController.PickObjectAction += OnPickObjectAction;
            _playerController.TouchBacketAction += OnTouchBacketAction;

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

        private void OnPickObjectAction()
        {
            _inventoryController.Pick();
        }

        private void OnTouchBacketAction()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                var inventoryObject = _inventoryController.RemovePicketObject();
                if (inventoryObject == null)
                {
                    return;
                }

                inventoryObject.transform.position = _backet.Point.position;
                inventoryObject.transform.parent = _backet.transform;
                var collider = inventoryObject.GetComponent<MeshCollider>();
                collider.enabled = true;
                inventoryObject.gameObject.AddComponent<Rigidbody>();
            }
        }

        private void OnDestroy()
        {
            _playerController.TouchGagesAction -= OnTouchGagesAction;
            _playerController.PickObjectAction -= OnPickObjectAction;
            _inventoryController.AddedToInventoryAction -= OnAddToInventoryAction;
            _inventoryController.AddedToInventoryAction -= OnAddToInventoryAction;
        }
    }
}