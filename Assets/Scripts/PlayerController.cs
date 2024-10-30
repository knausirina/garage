using System;
using System.Runtime.CompilerServices;
using Assets.Scripts;
using UnityEngine;
using static UnityEngine.UI.Image;


public class PlayerController : MonoBehaviour
{
    public Action TouchGagesAction;
    public Action<InventoryObject> PickObjectAction;
    public Action TouchBacketAction;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _inventoryPlaceTransform;

    [SerializeField]  public float _mouseSensitivity = 2f;
    [SerializeField]  public float _maxLookAngle = 180f;
    [SerializeField]  private float _walkSpeed = 5f;
    [SerializeField]  private float _maxVelocityChange = 10f;
    [SerializeField]  private float _distance = 1.75f;

    private float pitch = 0.0f;

    public void AddInventoryObject(InventoryObject inventoryObject)
    {
        inventoryObject.transform.parent = _inventoryPlaceTransform;
        inventoryObject.transform.localPosition = Vector3.zero;
    }
    
    private void Update()
    {
        var yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _mouseSensitivity;

        pitch -= _mouseSensitivity * Input.GetAxis("Mouse Y");
        transform.localEulerAngles = new Vector3(0, yaw, 0);
        _playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        CheckGates();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickObject();
        }

        CheckBacket();
    }

    private void FixedUpdate()
    {
        var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity) * _walkSpeed;

        var velocity = _rigidbody.velocity;
        var velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
        velocityChange.y = 0;

        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void CheckGates()
    {
        var rayCastHit = RayCast();
        if (rayCastHit != null && rayCastHit.Value.transform.gameObject.layer == Layers.Gate)
        {
            TouchGagesAction?.Invoke();
        }
    }

    private void CheckBacket()
    {
        var rayCastHit = RayCast();
        if (rayCastHit != null && rayCastHit.Value.transform.gameObject.layer == Layers.Backet)
        {
            TouchBacketAction?.Invoke();
        }
    }

    private void TryPickObject()
    {
        var rayCastHit = RayCast();
        if (rayCastHit != null && rayCastHit.Value.transform.gameObject.layer == Layers.Inventory)
        {
            var inventoryObject = rayCastHit.Value.transform.gameObject.GetComponent<InventoryObject>();
            PickObjectAction?.Invoke(inventoryObject);
        }
    }

    private RaycastHit? RayCast()
    {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _distance))
        {
            return hit;
        }

        return null;
    }
}