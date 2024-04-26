//Script that deals with game settings by taking in button input and
//changing onscreen slider and game settings

using UnityEngine;
using UnityEngine.UI;

public class SliderSettings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Awake()
    {
        GameManager.Instance.MusicChange(GameManager.musicVolume);
        GameManager.Instance.SFXChange(GameManager.sfxVolume);
        musicSlider.value = GameManager.musicVolume;
        sfxSlider.value = GameManager.sfxVolume;
    }

    #region Slider change functions
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
    #endregion
}
