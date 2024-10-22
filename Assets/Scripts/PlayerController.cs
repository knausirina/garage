using System;
using Assets.Scripts;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public Action TouchGagesAction;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField]  public const float _mouseSensitivity = 2f;
    [SerializeField]  public const float _maxLookAngle = 50f;
    [SerializeField]  private const float _walkSpeed = 5f;
    [SerializeField]  private const float _maxVelocityChange = 10f;

    private void Update()
    {
        var yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _mouseSensitivity;

        float pitch = 0;
        pitch -= _mouseSensitivity * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -_maxLookAngle, _maxLookAngle);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        _playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        CheckGates();
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
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        float distance = 5.75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);

            if (hit.transform.gameObject.layer == Layers.Gate)
            {
                TouchGagesAction?.Invoke();
            }
        }
        else
        {
            
        }
    }
}