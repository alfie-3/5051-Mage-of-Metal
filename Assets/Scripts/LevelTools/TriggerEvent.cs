using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] UnityEvent enterEvent;
    [SerializeField] UnityEvent exitEvent;

    [SerializeField] bool retriggerable;
    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        enterEvent.Invoke();

        if (!retriggerable)
        {
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        exitEvent.Invoke();
    }
}
