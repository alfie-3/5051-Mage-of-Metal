using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorPointer : MonoBehaviour
{
    [SerializeField] RectTransform ir_pointer;

    private void Update() {
        UpdateCursorPos();
    }

    private void UpdateCursorPos() {
        if (WiiInputManager.CursorWiiMote != null) {
            ir_pointer.transform.SetParent(transform.GetChild(0));


            Vector2 pointerPos = WiiInputManager.CursorWiiMote.IRPointScreenPos();

            if (pointerPos.x != -1 && pointerPos.y != -1) {
                ir_pointer.anchorMin = pointerPos;
                ir_pointer.anchorMax = pointerPos;

                ir_pointer.anchoredPosition = Vector2.zero;
                return;
            }
        }

        ir_pointer.transform.SetParent(transform);

        Vector2 mappedCusor = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

        ir_pointer.anchorMin = mappedCusor;
        ir_pointer.anchorMax = mappedCusor;

        ir_pointer.anchoredPosition = Vector2.zero;
    }
}
