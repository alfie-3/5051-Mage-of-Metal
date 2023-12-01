using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

//Specific to the wii remote with the guitar extension
public class GuitarRemoteInput : RemoteInput
{
    //Strumming
    public event Action Strummed = delegate { };
    bool strum;

    public GuitarRemoteInput(Wiimote _wiiMote = null) {
        WiiMote = _wiiMote;
    }

    public override void CheckInputs()
    {
        base.CheckInputs();

        CheckStrummedThisFrame();
    }

    //Gets if the requested colour fret is being held down on this frame
    public bool ColorPressedThisFrame(GUITAR_COLORS color) {
        if (WiiMote.current_ext == ExtensionController.GUITAR) {
            switch (color) {
                case GUITAR_COLORS.GREEN:
                    return WiiMote.Guitar.green_fret;
                case GUITAR_COLORS.RED:
                    return WiiMote.Guitar.red_fret;
                case GUITAR_COLORS.YELLOW:
                    return WiiMote.Guitar.yellow_fret;
                case GUITAR_COLORS.BLUE:
                    return WiiMote.Guitar.blue_fret;
                case GUITAR_COLORS.ORANGE:
                    return WiiMote.Guitar.orange_fret;
                default:
                    return false;
            }
        }

        return false;
    }

    //Checks to see if this guitar was strummed this frame, is triggered once and reset by releasing the strum to resting position
    public void CheckStrummedThisFrame()
    {
        WiiMote.ReadWiimoteData();

        if (WiiMote.Guitar.strum && !strum)
        {
            Strummed.Invoke();
            strum = true;
            return;
        }
        else if (!WiiMote.Guitar.strum)
        {
            strum = false;
            return;
        }

        return;
    }

    //Gets 0 - 1 on the whammy bar, whammy unpushed is 0 and when pushed down is 1 but detects anything between
    public float GetWhammy() => WiiMote.Guitar.GetWhammy01();

    public bool GetPlus() => WiiMote.Guitar.plus;

    public bool GetStar() => WiiMote.Guitar.minus;
}

public enum GUITAR_COLORS {
    GREEN,
    RED,
    YELLOW,
    BLUE,
    ORANGE
}