using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [SerializeField] KeyCode keyCode;
    public Action<KeyCode> onKeyEntered = delegate { };

    public void OnInput()
    {
        onKeyEntered.Invoke(keyCode);
    }
}
