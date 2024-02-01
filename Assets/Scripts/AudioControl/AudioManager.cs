using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using System;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class AudioManager : MonoBehaviour {

    [SerializeField] public NoteController _noteController;

    public static AudioManager managerInstance { get; private set; }
    [SerializeField] EventReference audioRef;
    EventInstance instance;

    //Time stuff
    TimelineInfo timelineInfo;
    GCHandle timelineHandle;
    FMOD.Studio.EVENT_CALLBACK beatCallback;

    string lastMarker;

    [SerializeField]
    [Range(0f, 1f)]
    private float musicPercentage;
    [SerializeField] int musicStartDelay=2000;

    private void Awake()
    {
        if (managerInstance == null)
        {
            managerInstance = this;
        }
    }

    private void Start()
    {
        MusicPlayWait(musicStartDelay);
    }

    async void MusicPlayWait(int milliseconds)
    {
        await Task.Delay(milliseconds);
        MusicPlay();
    }

    public void MusicPlay()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(audioRef);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));

        //Time stuff
        timelineInfo = new TimelineInfo();
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
        timelineHandle = GCHandle.Alloc(timelineInfo);
        instance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        instance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        //EndTimestuff

        instance.start();
        lastMarker = (string)timelineInfo.LastMarker;
    }
    public void MusicStop()
    {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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


    //Fmod website code that gives code thing
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance callBackInstance = new FMOD.Studio.EventInstance(instancePtr);

        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = callBackInstance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.CurrentMusicBar = parameter.bar;
                        break;
                    }
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.LastMarker = parameter.name;
                        _noteController.SpawnNote((string)timelineInfo.LastMarker);
                        break;
                    }
                case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
                    {
                        // Now the event has been destroyed, unpin the timeline memory so it can be garbage collected
                        timelineHandle.Free();
                        break;
                    }
            }
        }
        return FMOD.RESULT.OK;
    }

    private void OnApplicationPause(bool pause)
    {
        instance.setPaused(true);
    }

    private void OnApplicationQuit()
    {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}

class TimelineInfo
{
    public int CurrentMusicBar = 0;
    public FMOD.StringWrapper LastMarker = new FMOD.StringWrapper();
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
        if (GUILayout.Button("Stop Music"))
        {
            audioManager.MusicStop();
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