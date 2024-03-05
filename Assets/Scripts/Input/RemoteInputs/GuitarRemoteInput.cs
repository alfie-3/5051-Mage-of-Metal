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

        // Variables to store the previous state of each color key
    private bool previousGreenState;
    private bool previousRedState;
    private bool previousYellowState;
    private bool previousBlueState;
    private bool previousOrangeState;
    private bool released;
    public GuitarRemoteInput(Wiimote _wiiMote = null) {
        WiiMote = _wiiMote;
    }

    public override void CheckInputs()
    {
        base.CheckInputs();

        if (WiiMote == null) return;

        CheckStrummedThisFrame();
    }

    //Gets if the requested colour fret is being held down on this frame
    public bool ColorPressedThisFrame(GUITAR_COLORS color) {
        if (WiiMote.current_ext == ExtensionController.GUITAR) {
            switch (color) {
                case GUITAR_COLORS.GREEN:
                    previousGreenState = WiiMote.Guitar.green_fret;
                    return previousGreenState;
                case GUITAR_COLORS.RED:
                    previousRedState = WiiMote.Guitar.red_fret;
                    return previousRedState;
                case GUITAR_COLORS.YELLOW:
                    previousYellowState = WiiMote.Guitar.yellow_fret;
                    return previousYellowState;
                case GUITAR_COLORS.BLUE:
                    previousBlueState = WiiMote.Guitar.blue_fret;
                    return previousBlueState;
                case GUITAR_COLORS.ORANGE:
                    previousOrangeState = WiiMote.Guitar.orange_fret;
                    return previousOrangeState;
                default:
                    return false;
            }
        }

        return false;
    }

     // Checks if the requested color fret was released this frame
    public bool ColorReleasedThisFrame(GUITAR_COLORS color)
    {
        if (WiiMote.current_ext == ExtensionController.GUITAR)
        {
            switch (color)
            {
                case GUITAR_COLORS.GREEN:
                    released = !WiiMote.Guitar.green_fret && previousGreenState;
                    previousGreenState = !WiiMote.Guitar.green_fret;
                    return released;
                case GUITAR_COLORS.RED:
                    released = !WiiMote.Guitar.red_fret && previousRedState;
                    previousRedState = !WiiMote.Guitar.red_fret;
                    return released;
                case GUITAR_COLORS.YELLOW:
                    released = !WiiMote.Guitar.yellow_fret && previousYellowState;
                    previousYellowState = !WiiMote.Guitar.yellow_fret;
                    return released;
                case GUITAR_COLORS.BLUE:
                    released = !WiiMote.Guitar.blue_fret && previousBlueState;
                    previousBlueState = !WiiMote.Guitar.blue_fret;
                    return released;
                case GUITAR_COLORS.ORANGE:
                    released = !WiiMote.Guitar.orange_fret && previousOrangeState;
                    previousOrangeState = !WiiMote.Guitar.orange_fret;
                    return released;
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