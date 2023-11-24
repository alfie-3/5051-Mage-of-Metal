using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiiRemoteTest : MonoBehaviour {
    List<Wiimote> wiimotes = null;

    public RectTransform ir_pointer;

    public RectTransform[] ir_dots;
    public RectTransform[] ir_bb;

    // Start is called before the first frame update
    void Start() {
        FindWiiMotes();
    }

    private void FindWiiMotes() {
        WiimoteManager.FindWiimotes();

        wiimotes = WiimoteManager.Wiimotes;

        for (int i = 0; i < wiimotes.Count; i++) {
            EnableLED(i);
            wiimotes[i].SendDataReportMode(InputDataType.REPORT_EXT21);
            wiimotes[i].SetupIRCamera(IRDataType.BASIC);
        }

        Debug.Log($"{wiimotes.Count} wiimotes detected");
    }

    private void Update() {
        for (int i = 0; i < wiimotes.Count; i++) {
            wiimotes[i].ReadWiimoteData();

            DetectWiimoteButtons(i);
        }

        DetectWiimoteIR(0);
    }

    private void DetectWiimoteButtons(int index) {
        if (wiimotes[index].Button.a)
            Debug.Log($"Controller {index}: A Button Held");
    }

    private void DetectWiimoteIR(int wiiMoteindex) {
        Wiimote wiiMote = wiimotes[wiiMoteindex];

        if (ir_dots.Length < 4) return;

        float[,] ir = wiiMote.Ir.GetProbableSensorBarIR();
        for (int i = 0; i < 2; i++) {
            float x = (float)ir[i, 0] / 1023f;
            float y = (float)ir[i, 1] / 767f;
            if (x == -1 || y == -1) {
                ir_dots[i].anchorMin = new Vector2(0, 0);
                ir_dots[i].anchorMax = new Vector2(0, 0);
            }

            ir_dots[i].anchorMin = new Vector2(x, y);
            ir_dots[i].anchorMax = new Vector2(x, y);

            if (ir[i, 2] != -1) {
                int index = (int)ir[i, 2];
                float xmin = (float)wiiMote.Ir.ir[index, 3] / 127f;
                float ymin = (float)wiiMote.Ir.ir[index, 4] / 127f;
                float xmax = (float)wiiMote.Ir.ir[index, 5] / 127f;
                float ymax = (float)wiiMote.Ir.ir[index, 6] / 127f;
                ir_bb[i].anchorMin = new Vector2(xmin, ymin);
                ir_bb[i].anchorMax = new Vector2(xmax, ymax);
            }
        }

        float[] pointer = wiiMote.Ir.GetPointingPosition();

        ir_pointer.anchorMin = new Vector2(pointer[0], pointer[1]);
        ir_pointer.anchorMax = new Vector2(pointer[0], pointer[1]);

        Debug.Log($"{new Vector2(pointer[0], pointer[1])}");
    }

    private void EnableLED(int num) {
        wiimotes[num].SendPlayerLED(
            true,
            num >= 1,
            num >= 2,
            num >= 3
            );
    }

    void OnApplicationQuit() {
        WiimoteManager.Cleanup(wiimotes[0]);
    }
}
