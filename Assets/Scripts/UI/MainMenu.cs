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

    float alpha;
    Vector3 movingPos;
    Vector3 movingRot;

    public void Play()
    {
        SceneManager.LoadScene(2);
    }

    public void Settings()
    {
        StartCoroutine(SettingsLerp(menuPos, menuRot, settingsPos, settingsRot));
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
        yield return null;
    }

    public void MenuLerp()
    {
        StartCoroutine(SettingsLerp(settingsPos, settingsRot, menuPos, menuRot));
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