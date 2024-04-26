using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio; 
using FMODUnity;

public class SongProgress : MonoBehaviour
{
    private Slider progressSlider;
    public AudioManager audioManagerScript;
    private EventInstance musicEvent; 
    private int songLength; 
    private EventDescription eventDesc;


    void Start()
    {
        //yoink slider
        progressSlider = GetComponent<Slider>();
        //yoink song from audio manager
        musicEvent = audioManagerScript.musicInstance;
        //get description of event
        musicEvent.getDescription(out eventDesc);
        //get length from desc
        eventDesc.getLength(out songLength);
        //set max of slider to total song length
        progressSlider.maxValue = songLength;
    }

    void Update()
    {
        //if the song exists/is currently being played
        if (musicEvent.isValid())
        {
            //get current place on track
            musicEvent.getTimelinePosition(out int position);
            //set slider to progress
            progressSlider.value = position;
        }
    }
}