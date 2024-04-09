using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour, IDamage
{
    [SerializeField] int[] multiplierInts = { 1, 2, 4 };
    [SerializeField] float[] multUpgradeNumbers = { 0f, 0.15f, 0.5f, 1f };
    [SerializeField] float currentClampedScore = 0;

    public void Damage(int damage)
    {

    }

    private void Start()
    {
        LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x1";
        LevelManager.scoreSlider.GetComponent<Slider>().value = 0;
    }

    private void SetSliderScores(float addScore)
    {
        currentClampedScore = Mathf.Clamp01(currentClampedScore - addScore);
        if (currentClampedScore == 1)
        {
            LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x" + multiplierInts[multiplierInts.Length - 1];
            LevelManager.scoreSlider.GetComponent<Slider>().value = 1;
        }
        for (int i = 0; i < multUpgradeNumbers.Length - 1; i++)
        {
            if (currentClampedScore > multUpgradeNumbers[i] && currentClampedScore < multUpgradeNumbers[i + 1])
            {
                LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x" + multiplierInts[i];
                LevelManager.scoreSlider.GetComponent<Slider>().value = (currentClampedScore - multUpgradeNumbers[i]) / (multUpgradeNumbers[i + 1] - multUpgradeNumbers[i]);
                break;
            }
        }

    }
}
