using UnityEngine;

public class GatesController: MonoBehaviour
{
    private const float WidthGate = 5;
    private const float OpenVelocity = 0.5f;

    [SerializeField] private Gate _gateLeft;
    [SerializeField] private Gate _gateRight;

    private bool _isOpen = false;
    private bool _isOpened = false;
    private float _beginTime;

    private Vector3 _leftInitPosition;
    private Vector3 _newLeftPosition;
    private Vector3 _rightInitPosition;
    private Vector3 _newRightPosition;

    private void Awake()
    {
        _leftInitPosition = _gateLeft.gameObject.transform.position;
        _newLeftPosition = new Vector3(_leftInitPosition.x, _leftInitPosition.y, _leftInitPosition.z - WidthGate);

        _rightInitPosition = _gateRight.gameObject.transform.position;
        _newRightPosition = new Vector3(_rightInitPosition.x, _rightInitPosition.y, _rightInitPosition.z + WidthGate);
    }

    public void OpenGates()
    {
        if (!_isOpen)
        {
            _beginTime = Time.time;
        }
        _isOpen = true;
    }

    private void Update()
    {
        if (!_isOpen || _isOpened)
            return;

        var distance = (_gateLeft.transform.position - _newLeftPosition).magnitude;
        if (distance <= 0.1f)
        {
            _isOpened = true;
            return;
        }

        _gateLeft.transform.position =
            Vector3.Lerp(_leftInitPosition, _newLeftPosition, (Time.time - _beginTime) * OpenVelocity);
        _gateRight.transform.position =
            Vector3.Lerp(_rightInitPosition, _newRightPosition, (Time.time - _beginTime) * OpenVelocity);
    }
}
