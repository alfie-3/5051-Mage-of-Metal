using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioManager : MonoBehaviour {

    public static AudioManager managerInstance { get; private set; }
    [SerializeField] EventReference audioRef;
    EventInstance instance;

    [SerializeField]
    [Range(0f, 1f)]
    private float musicPercentage;

    private void Awake()
    {
        if (managerInstance == null)
        {
            managerInstance = this;
        }
    }

    public void MusicPlay()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(audioRef);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        instance.start();
        //RuntimeManager.PlayOneShot(sound, worldPos);
    }
    public void AddScore()
    {
        musicPercentage += 0.1f;
        instance.setParameterByName("InstrumentAudio", musicPercentage);
    }
    public void RemoveScore()
    {
        musicPercentage -= 0.1f;
        instance.setParameterByName("InstrumentAudio", musicPercentage);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AudioManager audioManager = (AudioManager)target;
        DrawDefaultInspector();
        
        if (GUILayout.Button("Start Music"))
        {
            audioManager.MusicPlay();
        }
        if (GUILayout.Button("Add Score"))
        {
            audioManager.AddScore();
        }
        if (GUILayout.Button("Remove Score"))
        {
            audioManager.RemoveScore();
        }
    }
}
#endif