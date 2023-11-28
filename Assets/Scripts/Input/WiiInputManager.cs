using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;
using static UnityEditor.LightingExplorerTableColumn;

public class WiiInputManager : MonoBehaviour
{
    public static SensorRemoteInput CursorWiiMote { get; private set; }
    public static GuitarRemoteInput GuitarWiiMote { get; private set; }

    public void Start() {
        FindWiimotes();
        SetupWiimotes();
    }

    public void FindWiimotes() {
        WiimoteManager.FindWiimotes();
        List<Wiimote> wiimotes = WiimoteManager.Wiimotes;

        if (wiimotes.Count == 0) {
            Debug.LogWarning("No Wiimotes Found");
            return;
        }

        GuitarWiiMote = new GuitarRemoteInput(GetGuitarWiiMote(wiimotes));
        wiimotes.Remove(GetGuitarWiiMote(wiimotes));

        CursorWiiMote = new SensorRemoteInput(wiimotes[0]);
    }

    private void SetupWiimotes() {
        if (CursorWiiMote != null) {
            CursorWiiMote.WiiMote.SendDataReportMode(InputDataType.REPORT_EXT21);
            CursorWiiMote.WiiMote.SetupIRCamera(IRDataType.BASIC);
        }
    }

    private Wiimote GetGuitarWiiMote(List<Wiimote> wiimotes) {
        Wiimote foundGuitarWiiMote = null;

        foreach (Wiimote wiimote in wiimotes) { 
            if (wiimote.current_ext == ExtensionController.GUITAR)
                foundGuitarWiiMote = wiimote;
        }

        if (foundGuitarWiiMote != null)
            return foundGuitarWiiMote;
        else {
            Debug.LogWarning("No Guitar WiiMote found");
            return null;
        }
    }
}
