using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour, IDamage
{
    int[] multiplierInts = { 1, 2, 4 };
    float[] multUpgradeNumbers = { 0f, 0.15f, 0.5f, 1f };
    float currentClampedScore = 0;

    [SerializeField] Volume _volume;
    [SerializeField] AnimationCurve damageAnim;
    [SerializeField] Vignette _vignette;
    private void Start()
    {
        LevelManager.scoreMultiplierText.GetComponent<TextMeshProUGUI>().text = "x1";
        Debug.Log("SADJBSADSAHND");
        LevelManager.scoreSlider.GetComponent<Slider>().value = 0;
        _volume.TryGetComponent<Vignette>(out _vignette);
        if (_vignette != null)
        {
            Debug.Log("Success");
        }
        else
        {
            Debug.Log("Not success");
        }
    }

    public void Damage(int damage)
    {
        StartCoroutine(DamageScreen());
    }

    private IEnumerator DamageScreen()
    {
        float time = 0;

        while (time < 1)
        {
            time += (Time.unscaledDeltaTime*1.5f);
            _vignette.intensity.value = damageAnim.Evaluate(time);
        }
        _vignette.intensity.value = damageAnim.Evaluate(0);
        yield return null;
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

#if UNITY_EDITOR

[CustomEditor(typeof(PlayerStats))]
public class PlayerStatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerStats playerManager = (PlayerStats)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Override"))
        {
            playerManager.Damage(1);
        }
    }
}
#endif
