using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ComboCounter : MonoBehaviour
{
    static public ComboCounter current;

    public static int multiplier = 1;
    [SerializeField] TextMeshProUGUI tmproUGUI;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }

        tmproUGUI.text = "x1";
    }

    public static void IncreaseCombo()
    {
        if (current == null) { return; }
        else current.L_IncreaseCombo();
    }

    private void L_IncreaseCombo()
    {
        multiplier++;
        tmproUGUI.text = $"x{multiplier}";
    }

    public static void ResetCombo()
    {
        if (current == null) { return; }
        else current.L_ResetCombo();
    }

    private void L_ResetCombo()
    {
        multiplier = 1;
        tmproUGUI.text = "x1";
    }

    public void SubmitScore()
    {

    }
}