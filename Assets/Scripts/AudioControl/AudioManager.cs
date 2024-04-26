//Audio manager takes FMOD song inputs and sends rune messages to RuneFMODBridge script
//Control centre for music-related functions

using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using System;
using System.Threading.Tasks;
using System.Collections;

public class AudioManager : MonoBehaviour {
    
    [Header("FMOD song reference")]
    [SerializeField] EventReference audioRef;
    public static AudioManager managerInstance { get; private set; }
    [HideInInspector] public EventInstance musicInstance;

    [Header("FMOD values")]
    TimelineInfo timelineInfo;
    GCHandle timelineHandle;
    FMOD.Studio.EVENT_CALLBACK beatCallback;
    string lastMarker;

    [Header("Music track values")]
    [SerializeField] int musicStartDelay=2000;
    [SerializeField] float beatLeadUpRef=2, tempoRef=160;
    static public float beatLeadUp, bpm;

    [Header("Safe playtime quit button")]
    [SerializeField] GameObject UIButton;

    private void Awake()
    {
#if UNITY_EDITOR
        UIButton.SetActive(true);
#else
        UIButton.SetActive(false);
#endif
        beatLeadUp = beatLeadUpRef;
        bpm = tempoRef;
        if (managerInstance == null)
        {
            managerInstance = this;
        }
        MusicPlay();
    }

    private void Start()
    {
        //StartCoroutine(MusicPlayWait(musicStartDelay));
    }

    #region Music instance interactions
    //Plays music at start of level after wait time
    IEnumerator MusicPlayWait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        MusicPlay();
    }

    public void MusicPlay()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(audioRef);
        musicInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));

        //Retrieve FMOD track info
        timelineInfo = new TimelineInfo();
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
        timelineHandle = GCHandle.Alloc(timelineInfo);
        musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        musicInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        lastMarker = (string)timelineInfo.LastMarker;

        //Setup track to play
        musicInstance.setVolume(GameManager.musicVolume);
        musicInstance.start();
    }

    public void MusicStop()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public void MusicPause()
    {
        musicInstance.setPaused(true);
    }
    public void MusicResume()
    {
        musicInstance.setPaused(false);
    }
    public async void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        //Debug.Log("I have quitted");
        MusicStop();
        await Task.Delay(1000);
        Application.Quit();
    }
    #endregion

    //Fmod website code that interprets FMOD track events
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

            //Code performed based off of type of event retrieved
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
                        RuneFMODBridge.Instance.SpawnNote((string)timelineInfo.LastMarker); //
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
}

//Information holder for events of FMOD instance
class TimelineInfo
{
    public int CurrentMusicBar = 0;
    public FMOD.StringWrapper LastMarker = new FMOD.StringWrapper();
}

//Added buttons to editor window for more control in edit mode
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
        if (GUILayout.Button("Stop PlayTime"))
        {
            audioManager.Quit();
        }
    }
}
#endif