using System;
using System.Runtime.CompilerServices;
using Assets.Scripts;
using UnityEngine;
using static UnityEngine.UI.Image;


public class PlayerController : MonoBehaviour
{
    public Action TouchGagesAction;
    public Action PickObjectAction;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _inventoryPlaceTransform;

    [SerializeField]  public float _mouseSensitivity = 2f;
    [SerializeField]  public float _maxLookAngle = 50f;
    [SerializeField]  private float _walkSpeed = 5f;
    [SerializeField]  private float _maxVelocityChange = 10f;
    [SerializeField]  private float _distance = 5.75f;

    public void AddInventoryObject(InventoryObject inventoryObject)
    {
        _inventoryPlaceTransform.position = Vector3.zero;
        _inventoryPlaceTransform.localPosition = Vector3.zero;
        inventoryObject.transform.parent = _inventoryPlaceTransform;
    }

    private void Update()
    {
        var yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _mouseSensitivity;

        float pitch = 0;
        pitch -= _mouseSensitivity * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -_maxLookAngle, _maxLookAngle);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        _playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        CheckGates();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickObjects();
        }
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

    private void TryPickObjects()
    {
        var rayCastHit = RayCast();
        if (rayCastHit != null && rayCastHit.Value.transform.gameObject.layer == Layers.Rack)
        {
            PickObjectAction?.Invoke();
        }
    }

    private RaycastHit? RayCast()
    {
        var origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f),
            transform.position.z);
        var direction = transform.TransformDirection(Vector3.forward);


        if (Physics.Raycast(origin, direction, out RaycastHit hit, _distance))
        {
            Debug.DrawRay(origin, direction * _distance, Color.red);

            return hit;
        }

        return null;
    }
}