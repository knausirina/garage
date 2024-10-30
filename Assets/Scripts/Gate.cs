using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string _animationName;

    private Animator _animator;
    private bool _isOpened;
    public bool IsOpened => _isOpened;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Open()
    {
        _isOpened = true;
        _animator.Play(_animationName);
    }
}