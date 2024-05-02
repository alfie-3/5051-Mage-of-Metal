//Script for operating Main menu

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menu positioning settings")]
    [SerializeField] Vector3 menuPos;
    [SerializeField] Vector3 menuRot;
    [SerializeField] Vector3 settingsPos;
    [SerializeField] Vector3 settingsRot;
    [SerializeField] Vector3 levelSelPos;
    [SerializeField] Vector3 levelSelRot;

    [Space]
    [Header("Player and screen settings")]
    [SerializeField] float alphaRate;
    [SerializeField] Renderer quadTransition;
    GameObject player; //Don't replace with LevelManager.player

    bool isTransitioning = false;
    bool isLeaving = false;
    float alpha;
    Vector3 movingPos;
    Vector3 movingRot;

    //Load in screen
    private void Start()
    {
        player = GameObject.Find("Main Camera");
        player.transform.position = menuPos;
        player.transform.eulerAngles = menuRot;
        StartCoroutine(LevelManager.ChangeAlpha(0, 0.5f, quadTransition.material, 0.5f));
    }

    #region On-screen button functions
    public void LevelSelectLerp()
    {
        if (!isLeaving && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(CameraLerp(menuPos, menuRot, levelSelPos, levelSelRot));
        }
    }

    public void LevelOpen(int levelNum) //int Level1 = 2
    {
        if (!isLeaving && !isTransitioning)
        {
            isLeaving = true;
            StartCoroutine(ExitScene(levelNum));
        }
    }

    public void SettingsLerp()
    {
        if (!isLeaving && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(CameraLerp(menuPos, menuRot, settingsPos, settingsRot));
        }
    }


    public void SettingsToMainMenu()
    {
        if (!isLeaving && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(CameraLerp(settingsPos, settingsRot, menuPos, menuRot));
        }
    }
    public void LevelSelectToMainMenu()
    {
        if (!isLeaving && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(CameraLerp(levelSelPos, levelSelRot, menuPos, menuRot));
        }
    }

    public void Quit()
    {
        if (!isLeaving && !isTransitioning)
        {
            isLeaving = true;
            StartCoroutine(ExitGame());
        }
    }
    #endregion

    #region Load in-out functionality
    IEnumerator CameraLerp(Vector3 startPos, Vector3 startRot, Vector3 endPos, Vector3 endRot)
    {
        alpha = 0;
        while (alpha < 1.0f)
        {
            alpha = Mathf.Clamp(alpha + (Time.deltaTime * alphaRate), 0, 1);
            movingPos = Lerp.EaseInOutSine(startPos, endPos, alpha);
            movingRot = Lerp.EaseInOutSine(startRot, endRot, alpha);

            player.transform.position = movingPos;
            player.transform.eulerAngles = movingRot;
            yield return null;
        }
        isTransitioning = false;
        yield return null;
    }

    IEnumerator ExitScene(int nextLevel)
    {
        yield return LevelManager.ChangeAlpha(0.5f, 0, quadTransition.material, 0.5f);
        yield return SceneManager.LoadSceneAsync(nextLevel);
    }

    IEnumerator ExitGame()
    {
        yield return LevelManager.ChangeAlpha(0.5f, 0, quadTransition.material, 0.5f);
        Application.Quit();
    }
    #endregion
}

//Custom lerp class for camera movements
public class Lerp
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 EaseInOutSine(Vector3 start, Vector3 end, float alpha)
    {
        float val = -(Mathf.Cos(Mathf.PI * alpha) - 1) / 2;
        return ((end - start) * val) + start; 
    }
}