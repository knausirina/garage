using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Action OpenAction;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("xxx OnTriggerEnter " + other);

        OpenAction?.Invoke();
    }
}
