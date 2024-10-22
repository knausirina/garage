using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Backet: MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        public Transform Point => _transform;
    }
}