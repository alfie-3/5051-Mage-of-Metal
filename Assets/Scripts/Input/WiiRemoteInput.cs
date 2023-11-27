using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiiRemoteInput : MonoBehaviour
{
    private Vector2 DetectWiimoteIR(Wiimote wiiMote) {
        Vector2[] ir_dots = new Vector2[4];
        Vector2[] ir_bb = new Vector2[4];

        if (ir_dots.Length < 4)
            return new Vector2(-1, -1);

        float[,] ir = wiiMote.Ir.GetProbableSensorBarIR();
        for (int i = 0; i < 2; i++) {
            float x = (float)ir[i, 0] / 1023f;
            float y = (float)ir[i, 1] / 767f;
            if (x == -1 || y == -1) {
                ir_dots[i] = new Vector2(0, 0);
                ir_dots[i] = new Vector2(0, 0);
            }

            ir_dots[i]= new Vector2(x, y);
            ir_dots[i] = new Vector2(x, y);

            if (ir[i, 2] != -1) {
                int index = (int)ir[i, 2];
                float xmin = (float)wiiMote.Ir.ir[index, 3] / 127f;
                float ymin = (float)wiiMote.Ir.ir[index, 4] / 127f;
                float xmax = (float)wiiMote.Ir.ir[index, 5] / 127f;
                float ymax = (float)wiiMote.Ir.ir[index, 6] / 127f;
                ir_bb[i] = new Vector2(xmin, ymin);
                ir_bb[i] = new Vector2(xmax, ymax);
            }
        }

        float[] pointer = wiiMote.Ir.GetPointingPosition();

        Vector2 pointerPos = new Vector2(pointer[0], pointer[1]);

        if (pointer[0] != -1 && pointer[1] != -1) {
            return pointerPos;
        }

        return pointerPos;
    }

    private bool ColorPressedThisFrame(Wiimote wiiMote, GUITAR_COLORS color) {
        if (wiiMote.current_ext == ExtensionController.GUITAR) {
            switch (color) {
                case GUITAR_COLORS.GREEN:
                    return wiiMote.Guitar.green;
                case GUITAR_COLORS.RED:
                    return wiiMote.Guitar.red;
                case GUITAR_COLORS.BLUE:
                    return wiiMote.Guitar.blue;
                case GUITAR_COLORS.ORANGE:
                    return wiiMote.Guitar.orange;
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