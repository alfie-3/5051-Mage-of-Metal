using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class GuitarRemoteInput : MonoBehaviour
{
   public Wiimote WiiMote { get; private set; }

    public GuitarRemoteInput(Wiimote _wiiMote) {
        WiiMote = _wiiMote;
    }

    public bool ColorPressedThisFrame(GUITAR_COLORS color) {
        if (WiiMote.current_ext == ExtensionController.GUITAR) {
            switch (color) {
                case GUITAR_COLORS.GREEN:
                    return WiiMote.Guitar.green;
                case GUITAR_COLORS.RED:
                    return WiiMote.Guitar.red;
                case GUITAR_COLORS.BLUE:
                    return WiiMote.Guitar.blue;
                case GUITAR_COLORS.ORANGE:
                    return WiiMote.Guitar.orange;
                default:
                    return false;
            }
        }

        return false;
    }
}

public enum GUITAR_COLORS {
    GREEN,
    RED,
    YELLOW,
    BLUE,
    ORANGE
}