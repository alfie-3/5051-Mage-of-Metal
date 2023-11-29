using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class GuitarRemoteInput : IHasRemote
{
   public Wiimote WiiMote { get; private set; }

    public bool HasRemote => WiiMote != null;

    //Strumming
    public event Action Strummed = delegate { };
    bool strum;

    public GuitarRemoteInput(Wiimote _wiiMote = null) {
        WiiMote = _wiiMote;
    }

    public void SetWiiMote(Wiimote _wiiMote)
    {
        WiiMote = _wiiMote;
    }

    public void CheckInputs()
    {
        if (WiiMote == null) return;

        CheckStrummedThisFrame();
    }

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