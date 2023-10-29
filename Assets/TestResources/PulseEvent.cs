using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseEvent : MonoBehaviour
{
    BeatManager beatManager;

    private void Start() {
        beatManager = GameObject.Find("BeatController").GetComponent<BeatManager>();
    }

    public void BeatEvent() {
        Debug.Log("!");
    }
}
