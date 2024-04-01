using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static GameManager Instance;
    [Header("Settings")]
    public static float musicVolume = 0.5f;
    public static float sfxVolume = 0.5f;
    public static int difficulty = 0;

    private bool isLoaded=false;

    private void Awake()
    {
        if (!isLoaded)
        {
            if (GameObject.FindGameObjectsWithTag("MainGameManager").Length > 1)
            {
                Destroy(gameObject);
            }
        }
        isLoaded = true;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void MusicChange(float volume)
    {
        musicVolume = volume;
        if (AudioManager.managerInstance != null)
        {
            AudioManager.managerInstance.SetMusicSettings(musicVolume);
        }
    }
    public void SFXChange(float volume)
    {
        sfxVolume = volume;
    }
    
}