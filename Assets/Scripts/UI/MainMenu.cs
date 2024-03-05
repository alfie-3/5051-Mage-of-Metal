using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Vector3 menuPos, menuRot, settingsPos, settingsRot;
    [SerializeField] GameObject player;
    [SerializeField] float alphaRate;
    [SerializeField] Renderer quadTransition;

    bool isTransitioning = false;
    float alpha;
    Vector3 movingPos;
    Vector3 movingRot;

    private void Start()
    {
        StartCoroutine(LevelManager.ChangeAlpha(0, 0.5f, quadTransition.material, 0.5f));
    }

    public void Play()
    {
        StartCoroutine(ExitScene(2));
    }

    public void Settings()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(SettingsLerp(menuPos, menuRot, settingsPos, settingsRot));
        }
    }

    IEnumerator SettingsLerp(Vector3 startPos, Vector3 startRot, Vector3 endPos, Vector3 endRot)
    {
        alpha = 0;
        while (alpha < 1.0f)
        {
            alpha = Mathf.Clamp(alpha + (Time.deltaTime * alphaRate),0,1);
            movingPos = Lerp.EaseInOutSine(startPos, endPos, alpha);
            movingRot = Lerp.EaseInOutSine(startRot,endRot, alpha);

            player.transform.position = movingPos;
            player.transform.eulerAngles = movingRot;
            yield return null;
        }
        isTransitioning = false;
        yield return null;
    }

    public void MenuLerp()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(SettingsLerp(settingsPos, settingsRot, menuPos, menuRot));
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void AddVolume()
    {
        slider.value = Mathf.Clamp(slider.value + 0.1f,0,1);
    }

    public void MinusVolume()
    {
        slider.value = Mathf.Clamp(slider.value - 0.1f, 0, 1);
    }

    IEnumerator ExitScene(int nextLevel)
    {
        yield return LevelManager.ChangeAlpha(0.5f, 0, quadTransition.material, 0.5f);
        SceneManager.LoadScene(nextLevel);
    }
}

public class Lerp
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 EaseInOutSine(Vector3 start, Vector3 end, float alpha)
    {
        float val = -(Mathf.Cos(Mathf.PI * alpha) - 1) / 2;
        return ((end - start) * val) + start; 
    }
}