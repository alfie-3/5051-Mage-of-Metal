//Contains info for the player's score and score multiplier
//The score multiplier affects many other aspects of the game (music texture, vignette visuals)

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour, IScore
{
    [Header("Score variables")]
    int[] multiplierInts = { 1, 2, 4, 8 };
    float[] multUpgradeNumbers = { 0f, 0.15f, 0.5f, 1f };
    int scoreMultiplier = 1;
    float currentClampedScore = 0;
    public static int playerScore = 0;

    [Header("Visual effects variables")]
    [SerializeField] Volume _volume;
    [SerializeField] AnimationCurve damageAnim;
    UnityEngine.Rendering.Universal.Vignette _vignette;

    private void Start()
    {
        AudioManager.managerInstance.musicInstance.setParameterByName("InstrumentAudio", currentClampedScore);
        LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x1";
        _volume.profile.TryGet(out _vignette);
        _vignette.intensity.value = 0;
        LevelManager.scoreSlider.value = 0;
    }

    #region Score functions
    //Adds score and adds to score multiplier
    public void AddScore(float scoreMult, int score)
    {
        currentClampedScore = Mathf.Clamp01(currentClampedScore + scoreMult);
        AudioManager.managerInstance.musicInstance.setParameterByName("InstrumentAudio", currentClampedScore);
        playerScore += (score * scoreMultiplier);
        LevelManager.scoreSlider.transform.parent.GetComponent<TextMeshProUGUI>().text = "Score: " + playerScore;
        SetSliderScores();
    }

    //Removes from score multiplier and activates vignette effect
    public void DamageScore(float scoreMult, Color vignCol, float vignTime) {
        currentClampedScore -= scoreMult;
        StartCoroutine(DamageScreen(vignTime,vignCol));
        AudioManager.managerInstance.musicInstance.setParameterByName("InstrumentAudio", currentClampedScore);
        LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x" + 1;
        LevelManager.scoreSlider.value = 0;
        SetSliderScores() ;
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

    //Screen effect
    private IEnumerator DamageScreen(float screenTime, Color vignCol)
    {
        float time = 0;
        _vignette.color.value = vignCol;

        while (time < screenTime)
        {
            time += (Time.unscaledDeltaTime);
            _vignette.intensity.value = damageAnim.Evaluate(time/screenTime);
            yield return null;
        }
        _vignette.intensity.value = damageAnim.Evaluate(0);
        yield return null;
    }

}