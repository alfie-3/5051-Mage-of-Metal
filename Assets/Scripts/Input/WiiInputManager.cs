using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiiInputManager : MonoBehaviour
{
    [field: SerializeField] public static SensorRemoteInput CursorWiiMote { get; private set; }
    [field: SerializeField] public static GuitarRemoteInput GuitarWiiMote { get; private set; }

    public void Start() {
        FindWiimotes();
    }

    public void FindWiimotes() {
        WiimoteManager.FindWiimotes();
        List<Wiimote> wiimotes = WiimoteManager.Wiimotes;

        GuitarWiiMote = new GuitarRemoteInput(GetGuitarWiiMote(wiimotes));
        wiimotes.Remove(GetGuitarWiiMote(wiimotes));

        CursorWiiMote = new SensorRemoteInput(wiimotes[0]);
    }

    public Wiimote GetGuitarWiiMote(List<Wiimote> wiimotes) {
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
