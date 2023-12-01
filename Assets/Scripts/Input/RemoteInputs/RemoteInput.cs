using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;


//Generic attributes shared by all types of input from wii remotes
public abstract class RemoteInput
{
    public Wiimote WiiMote { get; protected set; }

    public bool HasRemote => WiiMote != null;

    public RemoteInput(Wiimote _wiiMote = null) {
        WiiMote = _wiiMote;
    }

    public void SetWiiMote(Wiimote _wiiMote) {
        WiiMote = _wiiMote;
    }

    public virtual void CheckInputs() {
        if (!HasRemote) return;
    }
}
