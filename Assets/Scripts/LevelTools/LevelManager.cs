using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;
using UnityEngine.InputSystem;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SplineAnimate splineInfo;
    [SerializeField] Renderer quadTransitioner;
    float duration;
    [SerializeField] GameObject playerRef, splineRef, runeManagerRef, pointerRef;
    [SerializeField] float beatLeadUpRef, tempoRef;

    static public GameObject player, spline, runeManager, pointer;
    static public float beatLeadUp, bpm;

    private Controls _controlsKnm;
    InputAction controls;

    static public bool isPaused=false;
    private bool isUnpausing=false;
    [SerializeField] TextMeshProUGUI pauseTimer;

    private void Awake()
    {
        player = playerRef;
        spline = splineRef;
        runeManager = runeManagerRef;
        pointer = pointerRef;
        beatLeadUp = beatLeadUpRef;
        bpm = tempoRef;
        _controlsKnm = new Controls();
    }

    private void Start()
    {
        if (splineInfo != null)
        {
            duration = splineInfo.Duration;
            StartCoroutine(CheckSpline());
        }
        StartCoroutine(ChangeAlpha(0, 0.5f, quadTransitioner.material,1));
    }
    private void OnEnable()
    {
        controls = _controlsKnm.GuitarControls.Pause;
        _controlsKnm.GuitarControls.Pause.performed += PauseGame;
        _controlsKnm.GuitarControls.Pause.Enable();

    }
    private void OnDisable()
    {
        _controlsKnm.GuitarControls.Strum.performed -= PauseGame;
        _controlsKnm.GuitarControls.Strum.Disable();
    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        if (!isPaused && !isUnpausing) {
            isPaused = true;
            AudioManager.managerInstance.MusicPause();
            Time.timeScale = 0;
            foreach (Transform child in pointerRef.transform) {
                child.gameObject.SetActive(false);
            }
        }
        else if (isPaused && !isUnpausing)
        {
            foreach (Transform child in pointerRef.transform)
            {
                child.gameObject.SetActive(true);
            }
            StartCoroutine(ResumeGame(3));
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
        yield return ChangeAlpha(0.5f, 0, quadTransitioner.material, 1);
        SceneManager.LoadSceneAsync(1);
    }
}
//
