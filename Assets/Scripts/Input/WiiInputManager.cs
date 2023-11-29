using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiiInputManager : MonoBehaviour
{
    public static SensorRemoteInput CursorWiiMote { get; private set; } = new SensorRemoteInput();
    public static GuitarRemoteInput GuitarWiiMote { get; private set; } = new GuitarRemoteInput();

    public void Start() {
        FindWiimotes();
    }

    private void FixedUpdate()
    {
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
    }

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

    private void SetupWiimote(Wiimote wiimote, InputDataType inputDataType = InputDataType.REPORT_EXT21, IRDataType dataType = IRDataType.BASIC) {
        if (CursorWiiMote != null) {
            wiimote.SendDataReportMode(inputDataType);
            wiimote.SetupIRCamera(dataType);
        }
    }

    private Wiimote TryGetGuitarWiimote()
    {
        List<Wiimote> wiimotes = WiimoteManager.Wiimotes;

        return GetGuitarWiiMote(wiimotes);
    }

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
