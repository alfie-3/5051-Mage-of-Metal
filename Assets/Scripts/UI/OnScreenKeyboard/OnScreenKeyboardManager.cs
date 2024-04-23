using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class OnScreenKeyboardManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] int maxChars = 10;
    [SerializeField] UnityEvent<string> endEvent;

    [SerializeField] List<Key> keys;

    private void Start()
    {
        foreach (var key in keys)
        {
            key.onKeyEntered += InputKey;
        }
    }

    public void InputKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Backspace:
                if (inputField.text.Length > 0)
                    inputField.text = inputField.text.Remove(inputField.text.Length - 1, 1);
                break;

            case KeyCode.Return:
                endEvent.Invoke(inputField.text);
                break;

            default:
                if (inputField.text.Length + 1 > maxChars) { return; }
                inputField.text += key;
                break;
        }
    }
}
