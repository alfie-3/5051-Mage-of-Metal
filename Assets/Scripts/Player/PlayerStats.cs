//Contains info for the player's score and score multiplier
//The score multiplier affects many other aspects of the game (music texture, vignette visuals)

using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IScore
{
    [Header("Score multiplier variables")]
    int[] multiplierInts = { 1, 2, 4, 8 };
    float[] multUpgradeNumbers = { 0f, 0.15f, 0.5f, 1f };
    int scoreMultiplier = 1;
    float currentClampedScore = 0;

    [Header("Score variables")]
    public static int playerScore = 0;

    private void Start()
    {
        AudioManager.managerInstance.musicInstance.setParameterByName("InstrumentAudio", 1);
        LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x1";
        LevelManager.scoreSlider.value = 0;
    }

    #region Score functions
    //Adds score and adds to score multiplier
    public void AddScore(float scoreMult, int score)
    {
        currentClampedScore = Mathf.Clamp01(currentClampedScore + scoreMult);
        //AudioManager.managerInstance.musicInstance.setParameterByName("InstrumentAudio", currentClampedScore);
        playerScore += (score * scoreMultiplier);
        LevelManager.scoreSlider.transform.parent.GetComponent<TextMeshProUGUI>().text = "Score: " + playerScore;
        SetSliderScores();
    }

    //Removes from score multiplier and activates vignette effect
    public void DamageScore(float scoreMult, Color vignCol, float vignTime) {
        currentClampedScore -= scoreMult;
        StartCoroutine(GlobalVolumeManager.Instance.PlayerVignetteEffect(vignTime,vignCol));
        //AudioManager.managerInstance.musicInstance.setParameterByName("InstrumentAudio", currentClampedScore);
        LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x" + 1;
        LevelManager.scoreSlider.value = 0;
        SetSliderScores() ;
    }
    public void DamageScore(float scoreMult)
    {
        currentClampedScore -= scoreMult;
        //AudioManager.managerInstance.musicInstance.setParameterByName("InstrumentAudio", currentClampedScore);
        LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x" + 1;
        LevelManager.scoreSlider.value = 0;
        SetSliderScores();
    }

    //Sets onscreen sliders and text current scores
    private void SetSliderScores()
    {
        if (currentClampedScore == 1)
        {
            scoreMultiplier = multiplierInts[multiplierInts.Length - 1];
            LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x" + multiplierInts[multiplierInts.Length - 1];
            LevelManager.scoreSlider.value = 1;
        }
        for (int i = 0; i < multUpgradeNumbers.Length - 1; i++)
        {
            if ((currentClampedScore > multUpgradeNumbers[i] && currentClampedScore < multUpgradeNumbers[i + 1]) || currentClampedScore == multUpgradeNumbers[i])
            {
                scoreMultiplier = multiplierInts[i];
                LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x" + multiplierInts[i];
                LevelManager.scoreSlider.value = (currentClampedScore - multUpgradeNumbers[i]) / (multUpgradeNumbers[i + 1] - multUpgradeNumbers[i]);
                break;
            }
        }
    }
    #endregion
}