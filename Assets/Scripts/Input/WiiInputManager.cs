using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiiInputManager : MonoBehaviour
{
    Wiimote cursorWiiMote;
    Wiimote guitarWiiMote;

    public void Start() {
        List<Wiimote> wiimotes = WiimoteManager.Wiimotes;
    }

    public Wiimote getGuitarWiiMote() {

    }
}
