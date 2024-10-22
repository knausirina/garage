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

            _inventoryController.AddedToInventoryAction += OnAddToInventoryAction;
        }

        private void Update()
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
                var rigidbody = inventoryObject.gameObject.AddComponent<Rigidbody>();

               // var direction = _playerController.transform.TransformDirection(Vector3.forward);
              //  rigidbody.AddForce(direction * 50f, ForceMode.Impulse);

               // Debug.DrawRay(origin, direction * _distance, Color.red);
            }
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
            _inventoryController.AddedToInventoryAction -= OnAddToInventoryAction;
        }
    }
}