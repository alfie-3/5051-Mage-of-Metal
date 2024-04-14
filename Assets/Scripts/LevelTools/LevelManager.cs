using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;
using UnityEngine.InputSystem;
using TMPro;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Spline and quads")]
    [SerializeField] private SplineAnimate splineInfo;
    [SerializeField] Renderer transitionerQuad;
    [SerializeField] Renderer levelWinLoseQuad;
    [SerializeField] float maxAlpha;
    bool showingResults=false;
    bool isTransitioning=false;

    float duration;
    [Header("Object references for static values")]
    [SerializeField] GameObject playerRef, splineRef, runeManagerRef, pointerRef, scoreMultiplierTextRef;
    [SerializeField] UnityEngine.UI.Slider scoreSliderRef;
    [SerializeField] float beatLeadUpRef, tempoRef;

    static public GameObject player, spline, runeManager, pointer, scoreMultiplierText;
    static public UnityEngine.UI.Slider scoreSlider;
    static public float beatLeadUp, bpm;

    [Header("Controls")]
    private Controls _controlsKnm;
    InputAction controls;
    static public bool isPaused=false;
    private bool isUnpausing=false;

    [Header("Pause menu objects")]
    [SerializeField] GameObject pauseSettingsMenu;
    [SerializeField] TextMeshProUGUI pauseTimer;

    private void Awake()
    {
        player = playerRef;
        spline = splineRef;
        runeManager = runeManagerRef;
        pointer = pointerRef;
        beatLeadUp = beatLeadUpRef;
        bpm = tempoRef;
        scoreMultiplierText = scoreMultiplierTextRef;
        scoreSlider = scoreSliderRef;
        _controlsKnm = new Controls();
    }

    private void Start()
    {
        pauseSettingsMenu.SetActive(false);
        if (splineInfo != null)
        {
            duration = splineInfo.Duration;
            StartCoroutine(CheckSpline());
        }
        StartCoroutine(ChangeAlpha(0, 0.5f, transitionerQuad.material,1));
    }
    private void OnEnable()
    {
        _controlsKnm.GuitarControls.Pause.performed += PauseGame;
        _controlsKnm.GuitarControls.Pause.Enable();

        _controlsKnm.GuitarControls.WinDie.performed += WinLossState;
        _controlsKnm.GuitarControls.WinDie.Enable();
    }
    private void OnDisable()
    {
        _controlsKnm.GuitarControls.Strum.performed -= PauseGame;
        _controlsKnm.GuitarControls.Strum.Disable();

        _controlsKnm.GuitarControls.WinDie.performed -= WinLossState;
        _controlsKnm.GuitarControls.WinDie.Disable();
    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        if (!isPaused && !isUnpausing) {
            isPaused = true;
            levelWinLoseQuad.material.SetFloat("_AlphaRange", maxAlpha);
            levelWinLoseQuad.material.SetColor("_Color", Color.green);
            AudioManager.managerInstance.MusicPause();
            pauseSettingsMenu.SetActive(true);
            Time.timeScale = 0;
            foreach (Transform child in pointerRef.transform) {
                child.gameObject.SetActive(true);
            }
        }
        else if (isPaused && !isUnpausing)
        {
            levelWinLoseQuad.material.SetFloat("_AlphaRange", 0);
            foreach (Transform child in pointerRef.transform)
            {
                child.gameObject.SetActive(true);
            }
            pauseSettingsMenu.SetActive(false);
            StartCoroutine(ResumeGame(3));
        }
    }

    private void WinLossState(InputAction.CallbackContext obj)
    {
        if (showingResults && !isTransitioning && !isPaused)
        {
            Debug.Log("LERP1");
            AudioManager.managerInstance.instance.getPitch(out float thing);
            Debug.Log(thing);
            StartCoroutine(GameSpeedLerp(true,1));
        }
        else if (!showingResults && !isTransitioning && !isPaused)
        {
            Debug.Log("LERP2");
            StartCoroutine(GameSpeedLerp(false, 1, Color.blue));
        }
    }

    private IEnumerator ResumeGame(float leadInTime)
    {
        isUnpausing = true;
        float time = leadInTime;
        float roundedTime;
        while (time > -1)
        {
            time -= Time.unscaledDeltaTime * 1.5f;
            roundedTime = Mathf.Ceil(time);
            if (roundedTime > 0)
            {
                pauseTimer.text = roundedTime.ToString();
            }
            else
            {
                pauseTimer.text = "";
            }
            yield return null;
        }
        AudioManager.managerInstance.MusicResume();
        isPaused = false;
        isUnpausing = false;
        Time.timeScale = 1;
    }

    IEnumerator CheckSpline()
    {
        while (splineInfo.ElapsedTime < duration)
        {
            yield return null;
        }
        StartCoroutine(SceneEnd());
    }

    private IEnumerator GameSpeedLerp(bool isStarting, float speedMod, [Optional] Color quadColor)
    {
        isTransitioning = true;
        levelWinLoseQuad.gameObject.SetActive(true);
        if (isStarting )
        {
            AudioManager.managerInstance.MusicResume();
        }
        if (quadColor != null)
        {
            levelWinLoseQuad.material.SetColor("_Color",quadColor);
        }
        if (isStarting)
        {
            float time = 0;
            while (time != 1)
            {
                time = Mathf.Clamp(time + Time.unscaledDeltaTime, 0, 1);
                Time.timeScale = time;
                levelWinLoseQuad.material.SetFloat("_AlphaRange", maxAlpha - (time * maxAlpha));
                AudioManager.managerInstance.instance.setPitch(time);
                yield return null;
            }
        }
        else
        {
            float time = 1;
            while (time != 0)
            {
                time = Mathf.Clamp(time - Time.unscaledDeltaTime, 0, 1);
                Time.timeScale = time;
                levelWinLoseQuad.material.SetFloat("_AlphaRange", maxAlpha - (time * maxAlpha));
                AudioManager.managerInstance.instance.setPitch(time);
                yield return null;
            }
        }
        if (isStarting)
        {
            levelWinLoseQuad.material.SetFloat("_AlphaRange", 0);
            Time.timeScale = 1;
            AudioManager.managerInstance.instance.setPitch(1);
            showingResults = false;
        }
        else
        {
            levelWinLoseQuad.material.SetFloat("_AlphaRange", maxAlpha);
            Time.timeScale = 0;
            AudioManager.managerInstance.instance.setPitch(0);
            AudioManager.managerInstance.MusicPause();
            showingResults = true;
        }
        isTransitioning = false;
        yield return null;
    }

    public static IEnumerator ChangeAlpha(float start,float end, Material mat, float alphaSpeed)
    {
        if (start < end)
        {
            float alpha = start;
            while (alpha < end)
            {
                alpha += Time.deltaTime * alphaSpeed;
                mat.SetFloat("_AlphaRange", alpha);
                yield return null;
            }
        }
        else if (start > end)
        {
            float alpha = start;
            while (alpha > end)
            {
                alpha -= Time.deltaTime * alphaSpeed;
                mat.SetFloat("_AlphaRange", alpha);
                yield return null;
            }
        }
    }

    private IEnumerator SceneEnd()
    {
        yield return ChangeAlpha(0.5f, 0, transitionerQuad.material, 1);
        SceneManager.LoadSceneAsync(1);
    }
}
//
