using Unity.VisualScripting;
using UnityEngine;

public class WiiRemoteGuitarTest : MonoBehaviour
{
    private void Update()
    {

        if (!WiiInputManager.GuitarWiiMote.HasRemote) return;

        GuitarRemoteInput guitarWiiMote = WiiInputManager.GuitarWiiMote;

        guitarWiiMote.WiiMote.ReadWiimoteData();

        if (guitarWiiMote.GetWhammy() > 0)
        {
            //Debug.Log(guitarWiiMote.GetWhammy());
        }

        if (guitarWiiMote.GetPlus())
        {
            //Debug.Log("Plus");
        }

        if (guitarWiiMote.GetStar())
        {
            //Debug.Log("FUUUUUCK!!!");
        }
    }
}
