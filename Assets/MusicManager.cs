using UnityEngine;
using FMODUnity;
using System;
using System.Runtime.InteropServices;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField]
    [EventRef]
    private string music = null;

    public TimelineInfo timelineInfo = null;
    private GCHandle timelineHandle;

    private FMOD.Studio.EventInstance musicInstance;

    private FMOD.Studio.EVENT_CALLBACK beatCallback;

    //todo

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo {
        public int currentBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    private void Awake() {
        if (music != null) {
            musicInstance = RuntimeManager.CreateInstance(music);
            musicInstance.start();
        }
    }

    private void Start() {
        if (music != null) {
            timelineInfo = new TimelineInfo();
            beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
            timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
            musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
            musicInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        }
    }

#if UNITY_EDITOR
    private void OnGUI() {
        GUILayout.Box($"Current Beat = {timelineInfo.currentBeat}, last Marker = {(string)timelineInfo.lastMarker}");
    }
#endif

    private void OnDestroy() {
        musicInstance.setUserData(IntPtr.Zero);
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
        timelineHandle.Free();
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr) {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK) {
            Debug.LogError("Timeline Callback Error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero) {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type) {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT: {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentBeat = parameter.beat;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER: {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                    break;
            }
            
        }
        return FMOD.RESULT.OK;
    }
}
