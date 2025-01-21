using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    public Transform Point => _transform;
}