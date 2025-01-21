using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    public Action TouchGagesAction;
    public Action<InventoryObject> PickObjectAction;
    public Action TouchBasketAction;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _inventoryPlaceTransform;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private EventSystem _eventSystem;

    private Vector2 _rotateDelta = Vector2.zero;
    private float m_HorizontalRot = 0;
    private float m_VerticalRot = 0;
    private List<string> _availableTouchesId = new List<string>();

    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _maxLookAngle = 180f;
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _maxVelocityChange = 10f;
    [SerializeField] private float _distance = 1.75f;
    [SerializeField] private float m_BottomClamp = 90f;
    [SerializeField] private float m_TopClamp = 90f;
    [SerializeField] private int _touchLimit = 10;
    [SerializeField] private float _rotateSpeedX = 5;
    [SerializeField] private float _rotateSpeedY = 2;


    public void AddInventoryObject(InventoryObject inventoryObject)
    {
        inventoryObject.transform.parent = _inventoryPlaceTransform;
        inventoryObject.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        CalcRotate();

        CheckGates();

        TryPickObject();

        CheckBasket();
    }

    private void CalcRotate()
    {
        if (Input.touchCount != 0)
        {
            foreach (var touch in Input.touches)
            {

                if ((touch.phase == TouchPhase.Began) &&
                    !_eventSystem.IsPointerOverGameObject(touch.fingerId) &&
                    _availableTouchesId.Count <= _touchLimit)
                    _availableTouchesId.Add(touch.fingerId.ToString());

                if (_availableTouchesId.Count == 0) continue;

                if (touch.fingerId.ToString() == _availableTouchesId[0])
                {
                    _rotateDelta += new Vector2(touch.deltaPosition.x, touch.deltaPosition.y);
                    if (touch.phase == TouchPhase.Ended) _availableTouchesId.RemoveAt(0);
                }
                else if (touch.phase == TouchPhase.Ended) _availableTouchesId.Remove(touch.fingerId.ToString());
            }
        }
    }

    private void LateUpdate()
    {
        if (_rotateDelta == Vector2.zero)
            return;
        m_HorizontalRot = _rotateDelta.x * _rotateSpeedX * Time.deltaTime;
        m_VerticalRot += _rotateDelta.y * _rotateSpeedY * Time.deltaTime;
        m_VerticalRot = Mathf.Clamp(m_VerticalRot, -m_BottomClamp, m_TopClamp);

        _playerCamera.transform.localRotation = Quaternion.Euler(m_VerticalRot, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * m_HorizontalRot);

        _rotateDelta = Vector2.zero;
    }

    private void FixedUpdate()
    {
        var h = _joystick.Horizontal;
        var v = _joystick.Vertical;

        var targetVelocity = new Vector3(h, 0, v);
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

    private void CheckBasket()
    {
        var rayCastHit = RayCast();
        if (rayCastHit != null && rayCastHit.Value.transform.gameObject.layer == Layers.Basket)
        {
            TouchBasketAction?.Invoke();
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
        if (Input.touchCount == 0)
        {
            return null;
        }
        var touch = Input.touches[0];
        var position = touch.position;
        if (touch.phase == TouchPhase.Began)
        {
            var ray = _playerCamera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, _distance))
            {
                return hit;
            }
        }
        return null;
    }
}