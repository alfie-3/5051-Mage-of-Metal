using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

//WiiInputManager should eventually be loaded at the start menu and be made persistent accross the game, and holds static references
//To the wiimote inputs, that have magical stuff for controlling the game with the wii remotes

public class WiiInputManager : MonoBehaviour
{
    public static SensorRemoteInput CursorWiiMote { get; private set; } = new SensorRemoteInput();
    public static GuitarRemoteInput GuitarWiiMote { get; private set; } = new GuitarRemoteInput();

    public void Start() {
        FindWiimotes();
    }

    private void FixedUpdate()
    {
        //If the wii guitar isnt found it checks 50 times a frame to see if it gets activated
        //Can take a few frames for the extension to be registered by the wii remote so this has to be done
        if (GuitarWiiMote.WiiMote == null)
        {
            Wiimote guitarWiiMote = TryGetGuitarWiimote();
            if (guitarWiiMote != null)
            {
                Debug.Log("Guitar Found");
                GuitarWiiMote.SetWiiMote(guitarWiiMote);
            }
        }
    }

    private void Update()
    {
        GuitarWiiMote.CheckInputs();
        CursorWiiMote.CheckInputs();
    }

    //Finds all the wii remotes using the API and assigns them to the inputs.
    public void FindWiimotes() {
        WiimoteManager.FindWiimotes();
        List<Wiimote> wiimotes = WiimoteManager.Wiimotes;

        if (wiimotes.Count == 0) {
            Debug.LogWarning("No Wiimotes Found");
            return;
        }

        GuitarWiiMote.SetWiiMote(GetGuitarWiiMote(wiimotes));
        wiimotes.Remove(GuitarWiiMote.WiiMote);

        CursorWiiMote.SetWiiMote(wiimotes[0]);
        SetupWiimote(CursorWiiMote.WiiMote);
    }

    //Sets up the wii remotes "DataReportMode" and "IRCamera" - Just required to activate the wii remotes sending appropriate data to unity.
    private void SetupWiimote(Wiimote wiimote, InputDataType inputDataType = InputDataType.REPORT_EXT21, IRDataType dataType = IRDataType.BASIC) {
        if (CursorWiiMote != null) {
            wiimote.SendDataReportMode(inputDataType);
            wiimote.SetupIRCamera(dataType);
        }
    }

    //Tries to find guitar with guitar and returns it.
    private Wiimote TryGetGuitarWiimote()
    {
        List<Wiimote> wiimotes = WiimoteManager.Wiimotes;

        return GetGuitarWiiMote(wiimotes);
    }

    //Checks the list of wii remotes to see if any has a guitar, if it finds one it sets it up and all is well in the world.
    private Wiimote GetGuitarWiiMote(List<Wiimote> wiimotes) {
        Wiimote foundGuitarWiiMote = null;

        foreach (Wiimote wiimote in wiimotes) {
            wiimote.ReadWiimoteData();

            if (wiimote.current_ext == ExtensionController.GUITAR)
                foundGuitarWiiMote = wiimote;
        }

        if (foundGuitarWiiMote != null)
        {
            Debug.Log("Found Guitar WiimMote");
            SetupWiimote(foundGuitarWiiMote);
            return foundGuitarWiiMote;
        }
        else
        {
            Debug.LogWarning("No Guitar WiiMote found");
            return null;
        }
    }
}
