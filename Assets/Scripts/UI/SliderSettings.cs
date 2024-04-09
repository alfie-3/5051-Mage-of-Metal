using UnityEngine;
using UnityEngine.UI;

public class SliderSettings : MonoBehaviour
{

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Awake()
    {
        GameManager.Instance.MusicChange(GameManager.musicVolume);
        GameManager.Instance.SFXChange(GameManager.sfxVolume);
        musicSlider.value = GameManager.musicVolume;
        sfxSlider.value = GameManager.sfxVolume;
    }

    public void ChangeMusicVolume(float value)
    {
        musicSlider.value = Mathf.Clamp(musicSlider.value + value, 0, 1);
        GameManager.Instance.MusicChange(musicSlider.value);
    }
    public void ChangeSFXVolume(float value)
    {
        sfxSlider.value = Mathf.Clamp(sfxSlider.value + value, 0, 1);
        GameManager.Instance.SFXChange(sfxSlider.value);
    }
}
