using Unity.VisualScripting;
using UnityEngine;

public class WiiRemoteGuitarTest : MonoBehaviour
{
    private void Update()
    {

        if (!WiiInputManager.GuitarWiiMote.HasRemote) return;

        GuitarRemoteInput guitarWiiMote = WiiInputManager.GuitarWiiMote;

        guitarWiiMote.WiiMote.ReadWiimoteData();

        if (guitarWiiMote.ColorPressedThisFrame(GUITAR_COLORS.GREEN))
        {
            Debug.Log("Green Pressed This Frame");
        }

        if (guitarWiiMote.ColorPressedThisFrame(GUITAR_COLORS.RED))
        {
            Debug.Log("Red Pressed This Frame");
        }

        if (guitarWiiMote.ColorPressedThisFrame(GUITAR_COLORS.YELLOW))
        {
            Debug.Log("Yellow Pressed This Frame");
        }

        if (guitarWiiMote.ColorPressedThisFrame(GUITAR_COLORS.BLUE))
        {
            Debug.Log("Blue Pressed This Frame");
        }

        if (guitarWiiMote.ColorPressedThisFrame(GUITAR_COLORS.ORANGE))
        {
            Debug.Log("Orange Pressed This Frame");
        }

        if (guitarWiiMote.GetWhammy() > 0)
        {
            Debug.Log(guitarWiiMote.GetWhammy());
        }

        if (guitarWiiMote.GetPlus())
        {
            Debug.Log("Plus");
        }

        if (guitarWiiMote.GetStar())
        {
            Debug.Log("FUUUUUCK!!!");
        }
    }
}
