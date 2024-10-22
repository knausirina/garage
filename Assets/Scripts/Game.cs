using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game: MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GatesController _gatesController;

        private void Awake()
        {
            _playerController.TouchGagesAction += OnTouchGagesAction;
        }

        private void OnTouchGagesAction()
        {
            _gatesController.OpenGates();
        }

        private void OnDestroy()
        {
            _playerController.TouchGagesAction -= OnTouchGagesAction;
        }
    }
}