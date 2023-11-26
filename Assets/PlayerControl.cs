using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    PlayerInput _playerInput;

    InputAction screenPointInput;
    Vector2 screenPoint;

    [SerializeField] float lookPower;

    float screenWidth = Screen.width;
    float screenHeight = Screen.height;

    private void Start() {
        _playerInput = GetComponent<PlayerInput>();
        screenPointInput = _playerInput.actions["ScreenPoint"];
    }

    private void Update() {
        screenPoint = screenPointInput.ReadValue<Vector2>();
        screenPoint.x = ((screenPoint.x * 2 / screenWidth) - 1) * lookPower;
        screenPoint.y = ((screenPoint.y * 2 / screenHeight) - 1) * lookPower;
        Debug.Log(screenPoint);
        gameObject.transform.localEulerAngles = new Vector3(-screenPoint.y, screenPoint.x, 0);
    }
}
