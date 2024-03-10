using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SplineAnimate splineInfo;
    [SerializeField] Renderer quadTransitioner;
    float duration;
    [SerializeField] GameObject playerRef, splineRef, runeManagerRef;

    [SerializeField] static public GameObject player, spline, runeManager;

    private void Awake()
    {
        player = playerRef;
        spline = splineRef;
        runeManager = runeManagerRef;
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
