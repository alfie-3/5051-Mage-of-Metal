using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public List<EventReference> soundEvent;
    [SerializeField] public float rate;
    [SerializeField] public GameObject player;

    private void Start()
    {
        PlaySound();
    }

    public void PlaySound()
    {
        RuntimeManager.PlayOneShotAttached(soundEvent[0], player);
    }
}
