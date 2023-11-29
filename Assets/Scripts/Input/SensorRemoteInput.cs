using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class SensorRemoteInput : IHasRemote
{
    public Wiimote WiiMote { get; private set; }

    public bool HasRemote => WiiMote != null;   

    Vector2[] ir_dots = new Vector2[6];
    Vector2[] ir_bb = new Vector2[5];

    public SensorRemoteInput(Wiimote _wiiMote = null) {
        WiiMote = _wiiMote;
    }

    public void SetWiiMote(Wiimote _wiiMote)
    {
        WiiMote = _wiiMote;
    }

    public Vector2 IRPointScreenPos() {
        WiiMote.ReadWiimoteData();

        if (ir_dots.Length < 4)
            return new Vector2(-1, -1);

        float[,] ir = WiiMote.Ir.GetProbableSensorBarIR();
        for (int i = 0; i < 2; i++) {
            float x = (float)ir[i, 0] / 1023f;
            float y = (float)ir[i, 1] / 767f;
            if (x == -1 || y == -1) {
                ir_dots[i] = new Vector2(0, 0);
                ir_dots[i] = new Vector2(0, 0);
            }

            ir_dots[i] = new Vector2(x, y);
            ir_dots[i] = new Vector2(x, y);

            if (ir[i, 2] != -1) {
                int index = (int)ir[i, 2];
                float xmin = (float)WiiMote.Ir.ir[index, 3] / 127f;
                float ymin = (float)WiiMote.Ir.ir[index, 4] / 127f;
                float xmax = (float)WiiMote.Ir.ir[index, 5] / 127f;
                float ymax = (float)WiiMote.Ir.ir[index, 6] / 127f;
                ir_bb[i] = new Vector2(xmin, ymin);
                ir_bb[i] = new Vector2(xmax, ymax);
            }
        }

        float[] pointer = WiiMote.Ir.GetPointingPosition();

        Vector2 pointerPos = new Vector2(pointer[0], pointer[1]);

        if (pointer[0] != -1 && pointer[1] != -1) {
            return pointerPos;
        }

        return pointerPos;
    }
}
